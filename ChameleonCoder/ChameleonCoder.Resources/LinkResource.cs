﻿using System;
using System.Collections;
using System.Windows.Controls;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// a class representing a resource that serves as link to another resource
    /// inherits from ResourceBase
    /// </summary>
    class LinkResource : ResourceBase
    {
        /// <summary>
        /// creates a new instance of the LinkResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        internal LinkResource(ref XmlDocument xml, string xpath, string datafile) : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.link;
        }

        #region properties

        /// <summary>
        /// the GUID of the resource the link points to
        /// </summary>
        public Guid Destination
        {
            get { return new Guid(this.XML.SelectSingleNode(this.XPath + "/@destination").Value); }
            private set { this.XML.SelectSingleNode(this.XPath + "/@destination").Value = value.ToString(); }
        }

        /// <summary>
        /// the name of the link which can either be an own, independant name or the destination's name
        /// </summary>
        public new string Name
        {
            get
            {
                string result = string.Empty;
                try
                {
                    result = base.Name;
                }
                catch (NullReferenceException) { }
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                try
                {
                    result = this.Resolve().Name;
                }
                catch (NullReferenceException) { }
                return result;
            }
            protected set { base.Name = value; }
        }

        public new string Description
        {
            get
            {
                string result = string.Empty;
                try
                {
                    result = base.Description;
                }
                catch (NullReferenceException) { }
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                try
                {
                    result = this.Resolve().Description;
                }
                catch (NullReferenceException) { }
                return result;
            }
            protected set { base.Description = value; }
        }

        #endregion

        #region methods

        internal override void Open()
        {
            this.Resolve().Open();
        }

        internal override void Package()
        {
            this.Resolve().Package();
        }

        internal override void Save()
        {
            base.Save();
            this.Resolve().Save();
        }

        internal override SortedList ToSortedList()
        {
            SortedList list = base.ToSortedList();

            list.Add("Destination", this.Destination);

            return list;
        }

        #endregion

        /// <summary>
        /// gets the destination instance
        /// </summary>
        /// <returns>the Resource object the link points to</returns>
        public ResourceBase Resolve()
        {
            return ResourceManager.FlatList.GetInstance(this.Destination);
        }
    }
}
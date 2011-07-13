﻿using System;
using System.Windows.Media;
using System.Xml;
using System.Collections.Generic;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a task,
    /// inherits from ResourceBase
    /// </summary>
    public class TaskResource : ResourceBase
    {
        public TaskResource(XmlNode node)
            : base(node)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/task.png")); } }

        #endregion

        #region IEnumerable

        public override IEnumerator<PropertyDescription> GetEnumerator()
        {
            IEnumerator<PropertyDescription> baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            yield return new PropertyDescription("end-time", this.EndTime.ToString(), "task");
        }

        #endregion

        public DateTime EndTime
        {
            get
            {
                try { return DateTime.Parse(this.XMLNode.Attributes["enddate"].Value); }
                catch (ArgumentNullException) { return DateTime.MaxValue; }
                catch (FormatException) { return DateTime.MaxValue; }
                catch (NullReferenceException) { return DateTime.MaxValue; }
            }
            protected set
            {
                this.XMLNode.Attributes["enddate"].Value = value.ToString();
                this.OnPropertyChanged("EndTime");
            }
        }
    }
}
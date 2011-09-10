﻿using System;
using System.Windows.Media;
using System.Xml;
using System.ComponentModel;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource : INotifyPropertyChanged, IComponent
    {
        /// <summary>
        /// a user-defined short description of the resource
        /// </summary>
        string Description { get; }

        /// <summary>
        /// any user-defined notes related to the resource
        /// </summary>
        string Notes { get; }

        /// <summary>
        /// an image representing an important information related to the resource
        /// </summary>
        ImageSource SpecialVisualProperty { get; }

        /// <summary>
        /// contains the XmlNode representing the resource
        /// Any changes to the resource should be immediately saved to this XmlNode.
        /// </summary>
        XmlElement Xml { get; }

        /// <summary>
        /// holds a reference to the resource's parent
        /// </summary>
        IResource Parent { get; }

        /// <summary>
        /// a ResourceCollection containing the (direct) child resources
        /// </summary>
        ResourceCollection Children { get; }

        /// <summary>
        /// initializes the resource
        /// </summary>
        /// <param name="data">the XmlNode containing the resource data.</param>
        /// <param name="parent">the parent resource for the resource</param>
        void Initialize(XmlElement data, IResource parent);
    }
}

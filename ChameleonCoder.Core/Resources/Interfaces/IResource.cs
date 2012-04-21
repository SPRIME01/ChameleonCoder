﻿using System.ComponentModel;
using System.Windows.Media;
using System.Xml;

namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to be implemented by resource types
    /// </summary>
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
        /// will contain the file in which the resource is defined
        /// </summary>
        Files.DataFile File { get; }

        /// <summary>
        /// holds a reference to the resource's parent
        /// </summary>
        IResource Parent { get; }

        /// <summary>
        /// an array containing the (direct) child resources
        /// </summary>
        IResource[] Children { get; }

        /// <summary>
        /// a collection containing the references this resource contains
        /// </summary>
        ResourceReference[] References { get; }

        /// <summary>
        /// updates the resource data
        /// </summary>
        /// <param name="data">the XmlNode containing the resource data.</param>
        /// <param name="parent">the parent resource for the resource</param>
        void Update(XmlElement data, IResource parent, Files.DataFile file);

        /// <summary>
        /// adds a new direct child resource to the resource
        /// </summary>
        /// <param name="child">the child resource to add</param>
        /// <remarks>Always raise the <c>PropertyChanged</c> event on the <see cref="Children"/> property when this method is called.</remarks>
        void AddChild(IResource child);

        /// <summary>
        /// removes a direct child resource from the resource
        /// The resource must free all references to the child when this is called.
        /// </summary>
        /// <param name="child">the child resource to remove</param>
        /// <remarks>Always raise the <c>PropertyChanged</c> event on the <see cref="Children"/> property when this method is called.</remarks>
        void RemoveChild(IResource child);

        /// <summary>
        /// adds a reference to another resource to the resource's child scope
        /// </summary>
        /// <param name="reference">the reference to add</param>
        /// <remarks>Always raise the <c>PropertyChanged</c> event on the <see cref="References"/> property when this method is called.</remarks>
        void AddReference(ResourceReference reference);

        /// <summary>
        /// removes a reference to another resource from the resource's child scope
        /// </summary>
        /// <param name="reference"></param>
        /// <remarks>Always raise the <c>PropertyChanged</c> event on the <see cref="References"/> property when this method is called.</remarks>
        void RemoveReference(ResourceReference reference);
    }
}

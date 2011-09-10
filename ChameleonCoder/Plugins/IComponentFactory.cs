﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an interface used to manage components
    /// </summary>
    public interface IComponentFactory : IPlugin
    {
        /// <summary>
        /// gets the DisplayName for a resource- or RichContent-type
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the localized DisplayName</returns>
        string GetDisplayName(Type type);

        /// <summary>
        /// gets the TypeIcon for a resource- or RichContent-type
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the TypeIcon</returns>
        ImageSource GetTypeIcon(Type type);

        /// <summary>
        /// gets the background brush for a resource- or RichContent-type
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the brush</returns>
        Brush GetBackground(Type type);

        /// <summary>
        /// provides information about a newly created resource,
        /// for example by letting the user modifiy its properties
        /// </summary>
        /// <param name="type">the type of the resource to create</param>
        /// <param name="name">the name for the resource</param>
        /// <param name="parent">the parent resource</param>
        /// <returns>a dictionary containing the attributes the resource's XML element should have</returns>
        /// <remarks>this function must not create an instance of the specified resource!</remarks>
        IDictionary<string, string> CreateResource(Type type, string name, IResource parent);

        /// <summary>
        /// creates a new ContentMember of the given Type, using the given name and parent member
        /// </summary>
        /// <param name="type">the type of the member to create</param>
        /// <param name="name">the name of the new member</param>
        /// <param name="parent">the parent member or null</param>
        /// <returns>the newly created IContentMember instance</returns>
        IContentMember CreateMember(Type type, string name, IContentMember parent);

        /// <summary>
        /// gets a list of all types registered by this factory
        /// </summary>
        /// <returns>the Type-Array</returns>
        Type[] GetRegisteredTypes();
    }
}

﻿using System;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Plugins
{
    public interface ITemplate : IPlugin
    {
        /// <summary>
        /// the default name for a new resource
        /// </summary>
        string DefaultName { get; }

        /// <summary>
        /// The name of a template group the template should be added to.
        /// If the template should not be added to a group, use null.
        /// </summary>
        string Group { get; }

        /// <summary>
        /// the resource type the template creates
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// creates a new instance of the resource type and does additional operations.
        /// The actual job of creating an instance and initializing it should be carried out
        /// to the public <code>ResourceTypeManager.CreateInstanceOf(...)</code> method.
        /// </summary>
        /// <param name="parent">the parent resource or null is it is a new top resource</param>
        /// <returns>the newly created resource</returns>
        IResource Create(IResource parent, string Name);
    }
}
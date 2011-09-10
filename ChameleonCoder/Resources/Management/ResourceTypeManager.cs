﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Media;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    /// <summary>
    /// manages the registered resource types
    /// </summary>
    public static class ResourceTypeManager
    {
        /// <summary>
        /// the collection holding the resource types
        /// </summary>
        private static ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        /// <summary>
        /// a dictionary associating the resource types with the registering component factory
        /// </summary>
        private static ConcurrentDictionary<Type, IComponentFactory> Factories = new ConcurrentDictionary<Type, IComponentFactory>();

        /// <summary>
        /// gets the resource type that was registered with the specified alias
        /// </summary>
        /// <param name="alias">the alias</param>
        /// <returns>the type</returns>
        internal static Type GetResourceType(string alias)
        {
            return ResourceTypes.GetResourceType(alias);
        }

        /// <summary>
        /// gets the alias a type was registered with
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the alias</returns>
        internal static string GetAlias(Type type)
        {
            return ResourceTypes.GetAlias(type);
        }

        /// <summary>
        /// creates an instance of the type registered with the specified alias
        /// </summary>
        /// <param name="alias">the alias</param>
        /// <returns>the instance</returns>
        internal static IResource CreateInstanceOf(string alias)
        {
            Type type = GetResourceType(alias);
            if (type == null)
                return null;
            return Activator.CreateInstance(type) as IResource;
        }

        /// <summary>
        /// creates an instance of the specified type,
        /// creates a new XmlElement representing the resource using the name and the specified attributes,
        /// and initializes the resource using the XmlElement and the given parent resource.
        /// </summary>
        /// <param name="type">the type to create an instance of</param>
        /// <param name="name">the name for the new resource</param>
        /// <param name="attributes">a list of attributes for the XmlElement</param>
        /// <param name="parent">the parent resource or null if a top-level resource is being created</param>
        /// <returns>the new resource</returns>
        public static IResource CreateInstanceOf(Type type, string name, IDictionary<string, string> attributes, IResource parent)
        {
            var resource = Activator.CreateInstance(type) as IResource;

            if (resource != null)
            {
                var document = resource.GetResourceFile().Document;

                var element = document.CreateElement(GetAlias(type));
                if (parent == null)
                    document.DocumentElement.SelectSingleNode("Resources").AppendChild(element);
                else
                    parent.Xml.AppendChild(element);

                foreach (var attribute in attributes)
                {
                    element.SetAttribute(attribute.Key, attribute.Value);
                }

                element.SetAttribute("name", name);

                resource.Initialize(element, parent);
                ResourceManager.Add(resource, parent);

                return resource;
            }

            return null;
        }

        /// <summary>
        /// creates an instance of the type with the specified alias,
        /// creates a new XmlElement representing the resource using the name and the specified attributes,
        /// and initializes the resource using the XmlElement and the given parent resource.
        /// </summary>
        /// <param name="alias">the alias of the type to create an instance of</param>
        /// <param name="name">the name for the new resource</param>
        /// <param name="attributes">a list of attributes for the XmlElement</param>
        /// <param name="parent">the parent resource or null if a top-level resource is being created</param>
        /// <returns>the new resource</returns>
        public static IResource CreateInstanceOf(string alias, string name, IDictionary<string, string> attributes, IResource parent)
        {
            return CreateInstanceOf(GetResourceType(alias), name, attributes, parent);
        }

        /// <summary>
        /// gets a list of all registered resource types
        /// </summary>
        /// <returns>the list</returns>
        internal static IEnumerable<Type> GetResourceTypes()
        {
            return ResourceTypes.GetList();
        }

        /// <summary>
        /// gets the factory that registered the given resource type
        /// </summary>
        /// <param name="component">the resource type</param>
        /// <returns>the IComponentFactory instance</returns>
        internal static IComponentFactory GetFactory(Type component)
        {
            IComponentFactory factory;
            if (Factories.TryGetValue(component, out factory))
                return factory;
            throw new ArgumentException("this is not a registered resource type", "component");
        }

        public static string GetDisplayName(Type component)
        {
            return GetFactory(component).GetDisplayName(component);
        }

        public static ImageSource GetTypeIcon(Type component)
        {
            return GetFactory(component).GetTypeIcon(component);
        }

        public static Brush GetBackground(Type component)
        {
            return GetFactory(component).GetBackground(component);
        }

        public static bool IsRegistered(string alias)
        {
            return ResourceTypes.IsRegistered(alias);
        }

        public static bool IsRegistered(Type type)
        {
            return ResourceTypes.IsRegistered(type);
        }

        public static void RegisterComponent(Type component, string alias, Plugins.IComponentFactory factory)
        {
            if (component.GetInterface(typeof(IResource).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic
                && !IsRegistered(alias) && !IsRegistered(component)
                && !string.Equals(alias, "metadata", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(alias, "RichContent", StringComparison.OrdinalIgnoreCase))
            {
                ResourceTypes.RegisterResourceType(alias, component);
                Factories.TryAdd(component, factory);
            }
        }

        internal static ResourceTypeCollection GetCollection()
        {
            lock (ResourceTypes)
            {
                return ResourceTypes;
            }
        }
    }
}

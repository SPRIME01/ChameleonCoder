﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources
{
    internal sealed class ResourceTypeManager
    {
        private static ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        internal static void Load()
        {
            var internTypes = from type in Assembly.GetEntryAssembly().GetTypes()
                              where type.GetInterface(typeof(IResource).FullName) != null
                              select type;

            var externTypes = from dll in Directory.GetFiles(Environment.CurrentDirectory + "\\Components", "*.dll")
                              let assembly = Assembly.LoadFrom(dll)
                              from type in assembly.GetTypes()
                              where type.GetInterface(typeof(IResource).FullName) != null
                              select type;

            var types = internTypes.Concat(externTypes);

            foreach (Type type in types)
            {
                if (!type.IsAbstract && !type.IsInterface && !type.IsNotPublic)
                    ResourceTypes.RegisterResourceType(type);
            }
        }

        internal static Type GetResourceType(string alias)
        {
            return ResourceTypes.GetResourceType(alias);
        }

        internal static IResource CreateInstanceOf(string alias)
        {
            return (IResource)Activator.CreateInstance(ResourceTypes.GetResourceType(alias));
        }

        internal static IEnumerable<Type> GetResourceTypes()
        {
            return ResourceTypes.GetList();
        }
    }
}

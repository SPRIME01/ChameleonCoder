﻿using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ResourceCore
{
    public sealed class ResourceProvider : ChameleonCoder.IComponentProvider
    {
        public void Init(Action<Type, ContentMemberTypeInfo> registerContentMember, Action<Type, ResourceTypeInfo> registerResourceType)
        {
            registerResourceType(typeof(LinkResource),
                new ResourceTypeInfo("link", "link", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/link.png")), Brushes.LightGreen,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(FileResource),
                new ResourceTypeInfo("file", "file", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/file.png")), Brushes.BurlyWood,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(CodeResource),
                new ResourceTypeInfo("code", "source code", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/code.png")), Brushes.LightSalmon,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(LibraryResource),
                new ResourceTypeInfo("library", "library", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/library.png")), Brushes.SandyBrown,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(ProjectResource),
                new ResourceTypeInfo("project", "project", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/project.png")), Brushes.Khaki,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(TaskResource),
                new ResourceTypeInfo("task", "task", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/task.png")), Brushes.LightCoral,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(GroupResource),
                new ResourceTypeInfo("group", "group", new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/group.png")), Brushes.DodgerBlue,
                    ResourceProvider.Create, ResourceProvider.Load));
        }

        public static void Load(IResource resource)
        {

        }

        public static void Create(Type target, IResource parent, Action<IResource, IResource> register)
        {
            if (parent == null)
            {
                string s = string.Empty;
                if (parent != null)
                    s = parent.ToString();

                ResourceCreator creator = new ResourceCreator(target, s);
                creator.ShowDialog();

                if (creator.DialogResult != false)
                {
                    System.Xml.XmlNode node = creator.GetXmlNode();

                    if (parent == null)
                    {
                        string path = Environment.CurrentDirectory + @"\Data\" + node.Attributes["name"].Value;
                        while (System.IO.File.Exists(path + ".xml"))
                            path = "_" + path;
                        node.OwnerDocument.Save(path + ".xml");

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.Load(path + ".xml");
                        node = doc.DocumentElement;
                    }
                    else
                        parent.Xml.AppendChild(node.CloneNode(false));

                    IResource resource = Activator.CreateInstance(target, node) as IResource;
                    register(resource, parent);
                }
            }
        }
    }
}

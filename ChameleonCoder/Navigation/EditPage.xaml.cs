﻿using System.Windows.Controls;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the edit control to edit resources
    /// </summary>
    internal sealed partial class EditPage : Page, UIAbstraction.IResourcePresenter
    {
        /// <summary>
        /// creates a new instance of the page, given an IEditable resource
        /// </summary>
        /// <param name="resource">the resource to edit</param>
        internal EditPage(IEditable resource)
        {
            InitializeComponent();

            ResourceManager.Open(EditableResource = resource);
            Editor.Text = resource.GetText();
            Editor.FontSize = Settings.ChameleonCoderSettings.Default.CodeFontSize;
            Editor.FontFamily = new System.Windows.Media.FontFamily(Settings.ChameleonCoderSettings.Default.CodeFont);

            ILanguageResource langRes = resource as ILanguageResource;
            if (langRes != null && PluginManager.IsModuleRegistered(langRes.Language))
            {
                Editor.SyntaxHighlighting = PluginManager.GetModule(langRes.Language).Highlighting;
            }
        }

        /// <summary>
        /// the resource which is edited
        /// </summary>
        internal IEditable EditableResource { get; private set; }

        public IResource Resource { get { return EditableResource; } }

        /// <summary>
        /// saves the changes to the resource
        /// </summary>
        internal void Save() // to be called from ribbon
        {
            EditableResource.SaveText(Editor.Text);
        }
    }
}

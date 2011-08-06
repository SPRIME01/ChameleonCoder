﻿using System.Windows.Controls;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        internal EditPage(IEditable resource)
        {
            InitializeComponent();
            this.Resource = resource;
            this.Editor.Text = resource.GetText();
            ILanguageResource langRes = resource as ILanguageResource;
            if (langRes != null)
            {
                if (ComponentManager.ActiveModule != null)
                    ComponentManager.UnloadModule();
                ComponentManager.LoadModule(langRes.language);
            }
        }

        internal IEditable Resource { get; private set; }

        internal void Save() // to be called from ribbon
        {
            Resource.SaveText(Editor.Text);
        }
    }
}

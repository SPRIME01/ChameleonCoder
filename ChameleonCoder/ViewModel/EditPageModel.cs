﻿using System.Windows.Input;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ICSharpCode.AvalonEdit.Document;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a view model class for editing an IEditable resource
    /// </summary>
    internal sealed class EditPageModel : ViewModelBase
    {
        /// <summary>
        /// creates a new instance for the given resource
        /// </summary>
        /// <param name="resource">the resource to edit</param>
        internal EditPageModel(IEditable resource)
        {
            resourceInstance = resource;
            Resources.Management.ResourceManager.Open(resource);

            Commands.Add(new CommandBinding(ChameleonCoderCommands.SaveResource,
                SaveResourceCommandExecuted));
            Commands.Add(new CommandBinding(NavigationCommands.Search,
                SearchCommandExecuted));
            Commands.Add(new CommandBinding(ApplicationCommands.Replace,
                ReplaceCommandExecuted));
            Commands.Add(new CommandBinding(NavigationCommands.Zoom,
                ZoomCommandExecuted));

            SettingsPageModel.Instance.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "CodeFont")
                    {
                        Update("CodeFont");
                    }
                };

            Document.Text = resource.GetText();
            Document.UndoStack.ClearAll();
        }

        #region commanding

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.SaveResource command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void SaveResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Resource.GetText() != Document.Text)
                Resource.SaveText(Document.Text);
        }

        /// <summary>
        /// implements the logic for the NavigationCommands.Search command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void SearchCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new CCSearchReplaceDialog(
                            () => Document.Text,
                            (offset, length, replaceBy) => Document.Replace(offset, length, replaceBy),
                            (offset, length) =>
                            {
                                //editor.Select(offset, length);
                                var loc = Document.GetLocation(offset);
                                //editor.ScrollTo(loc.Line, loc.Column);
                            },
                            false);
            dialog.ShowDialog();
        }

        /// <summary>
        /// implements the logic for the ApplicationCommands.Replace command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void ReplaceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new CCSearchReplaceDialog(
                            () => Document.Text,
                            (offset, length, replaceBy) => Document.Replace(offset, length, replaceBy),
                            (offset, length) =>
                            {
                                //editor.Select(offset, length);
                                var loc = Document.GetLocation(offset);
                                //editor.ScrollTo(loc.Line, loc.Column);
                            },
                            true);
            dialog.ShowDialog();
        }

        /// <summary>
        /// implements the logic for the NavigationCommands.Zoom command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void ZoomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter as string == "in")
                CodeFontSize += 3;
            else if (e.Parameter as string == "out")
                CodeFontSize -= 3;
        }

        #endregion

        /// <summary>
        /// gets the font size
        /// </summary>
        public int CodeFontSize
        {
            get { return fontSize; }
            private set
            {
                fontSize = value;
                Update("CodeFontSize");
            }
        }

        private int fontSize = Settings.ChameleonCoderSettings.Default.CodeFontSize;

        /// <summary>
        /// gets the code font
        /// </summary>
        public static System.Windows.Media.FontFamily CodeFont
        {
            get { return SettingsPageModel.Instance.CodeFont; }
        }

        /// <summary>
        /// gets the highlighting definition
        /// </summary>
        public ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition Highlighting
        {
            get
            {
                ILanguageResource langRes = Resource as ILanguageResource;
                if (langRes != null && PluginManager.IsModuleRegistered(langRes.Language))
                {
                    return PluginManager.GetModule(langRes.Language).Highlighting;
                }
                return null;
            }
        }

        /// <summary>
        /// gets the document to display
        /// </summary>
        public TextDocument Document
        {
            get
            {
                return textDocument;
            }
        }

        private readonly TextDocument textDocument = new TextDocument();

        /// <summary>
        /// gets the resource being edited
        /// </summary>
        public IEditable Resource
        {
            get { return resourceInstance; }
        }

        private readonly IEditable resourceInstance;
    }
}

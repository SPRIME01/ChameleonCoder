﻿using System;
using System.IO;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Shared
{
    /// <summary>
    /// a delegate for resource events
    /// </summary>
    /// <param name="sender">the resource raising the event</param>
    /// <param name="e">additional data</param>
    [Obsolete("Use ResourceManager infrastructure directly!")]
    public delegate void ResourceEventHandler(object sender, EventArgs e);

    /// <summary>
    /// a delegate for LanguageModule events
    /// </summary>
    /// <param name="sender">the LanguageModule raising the event</param>
    /// <param name="e">additional data</param>
    [Obsolete("Use PluginManager infrastructure directly!")]
    public delegate void LanguageModuleEventHandler(object sender, EventArgs e);

    /// <summary>
    /// a delegate for Service Events
    /// </summary>
    /// <param name="sender">the service raising the event</param>
    /// <param name="e">additional data</param>
    [Obsolete("Use PluginManager infrastructure directly!")]
    public delegate void ServiceEventHandler(object sender, EventArgs e);

    /// <summary>
    /// a delegate for Settings events
    /// </summary>
    /// <param name="newValue">the setting's new value</param>
    public delegate void SettingsEventHandler(object newValue);

    /// <summary>
    /// a public class providing information and notification for plugins
    /// </summary>
    [Obsolete]
    public static class InformationProvider
    {
        #region settings

        /// <summary>
        /// the user's Language as LCID code
        /// </summary>
        public static int Language { get { return Settings.ChameleonCoderSettings.Default.Language; } }

        #endregion

        #region information

        /// <summary>
        /// gets the type of page that is currently active
        /// </summary>
        public static CCTabPage CurrentPage
        {
            get
            {
                return CCTabPage.None; // ViewModel.MainWindowModel.Instance.ActiveTab.Type; // TODO!
            }
        }

        #endregion

        #region tools

#if ALL_STUFF
        /// <summary>
        /// registers a new CodeGenerator
        /// </summary>
        /// <param name="clicked">a delegate to invoke when the generator is invoked</param>
        /// <param name="image">the image to display for the CodeGenerator</param>
        /// <param name="text">the name of the CodeGenerator</param>
        public static void RegisterCodeGenerator(CodeGeneratorEventHandler clicked, ImageSource image, string text)
        {
            RibbonButton button = new RibbonButton() { Content = text, LargeImage = image };
            button.Click += (s, e) =>
                {
                    CodeGeneratorEventArgs args = new CodeGeneratorEventArgs();
                    clicked(CurrentResource, args);
                    if (!args.Handled)
                        InsertCode(args.Code);
                };
            ChameleonCoderApp.Gui.CustomGroup1.Controls.Add(button);
        }

        /// <summary>
        /// registers a new StubCreator
        /// </summary>
        public static void RegisterStubCreator()
        {
        } // IStubCreator

#endif

        #endregion

        #region Editing

#if ALL_STUFF
        /// <summary>
        /// appends code to the currently edited resource
        /// </summary>
        /// <param name="code">the code to insert</param>
        public static void AppendCode(string code)
        {
            var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content  as Navigation.EditPage;
            if (edit != null)
                edit.Editor.AppendText(code);
        }

        /// <summary>
        /// inserts code in the currently edited resource
        /// </summary>
        /// <param name="code">the code to insert</param>
        /// <param name="position">the position to use</param>
        public static void InsertCode(string code, int position)
        {
            var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content as Navigation.EditPage;
            if (edit != null)
                edit.Editor.Text = edit.Editor.Text.Insert(position, code);
        }

        /// <summary>
        /// inserts code in the currently edited resource at cursor position
        /// </summary>
        /// <param name="code">the code to insert</param>
        public static void InsertCode(string code)
        {
            var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content as Navigation.EditPage;
            if (edit != null)
                edit.Editor.Text = edit.Editor.Text.Insert(edit.Editor.CaretOffset, code);
        }

        /// <summary>
        /// gets the current edit control
        /// </summary>
        /// <returns>the edit control or null if no resource is currently edited</returns>
        public static ICSharpCode.AvalonEdit.TextEditor GetEditor()
        {
            if (ViewModel.MainWindowModel.Instance.ActiveTab != null)
            {
                var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content as Navigation.EditPage;
                if (edit != null)
                    return edit.Editor;
            }
            return null;
        }
#endif

        #endregion

        #region shared infrastructure

        /// <summary>
        /// finds a free path for a file or directory, given the directory and the base name.
        /// </summary>
        /// <param name="directory">the directory which should contain the file or directory</param>
        /// <param name="baseName">the base name for the new file or directory</param>
        /// <param name="isFile">true if the method should look for a free file path, false for a free directory path.</param>
        /// <returns></returns>
        public static string FindFreePath(string directory, string baseName, bool isFile)
        {
            directory = directory[directory.Length - 1] == Path.DirectorySeparatorChar
                ? directory : directory + Path.DirectorySeparatorChar;

            baseName = baseName.TrimStart(Path.DirectorySeparatorChar);

            string fileName = isFile
                ? Path.GetFileNameWithoutExtension(baseName) : baseName;

            string Extension = isFile
                ? Path.GetExtension(baseName) : string.Empty;

            string path = directory + fileName + Extension;
            int i = 0;

            while ((isFile ? File.Exists(path) : Directory.Exists(path)))
            {
                path = directory + fileName + "_" + i + Extension;
                i++;
            }

            return path;
        }

        #endregion

        #region events

        /// <summary>
        /// raised when a Language module was loaded
        /// </summary>
        [Obsolete("Use PluginManager infrastructure directly!")]
        public static event LanguageModuleEventHandler ModuleLoaded;

        /// <summary>
        /// raised when a Language module was unloaded
        /// </summary>
        [Obsolete("Use PluginManager infrastructure directly!")]
        public static event LanguageModuleEventHandler ModuleUnloaded;

        /// <summary>
        /// raised when a service is going to be executed
        /// </summary>
        [Obsolete("Use PluginManager infrastructure directly!")]
        public static event ServiceEventHandler ServiceExecute;

        /// <summary>
        /// raised when a service was executed
        /// </summary>
        [Obsolete("Use PluginManager infrastructure directly!")]
        public static event ServiceEventHandler ServiceExecuted;

        /// <summary>
        /// raised when a plugin is installed
        /// </summary>
        public static event EventHandler PluginInstalled;

        /// <summary>
        /// raised when a plugin was uninstalled
        /// </summary>
        public static event EventHandler PluginUninstalled;

        /// <summary>
        /// raised when the 'Language' setting changed
        /// </summary>
        public static event SettingsEventHandler LanguageChanged;

        #endregion

        #region event wrappers

        /// <summary>
        /// raises the ModuleLoaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        [Obsolete("Use PluginManager infrastructure directly!")]
        internal static void OnModuleLoaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleLoaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleUnloaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        [Obsolete("Use PluginManager infrastructure directly!")]
        internal static void OnModuleUnloaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleUnloaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ServiceExecute event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        [Obsolete("Use PluginManager infrastructure directly!")]
        internal static void OnServiceExecute(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecute;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ServiceExecuted event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        [Obsolete("Use PluginManager infrastructure directly!")]
        internal static void OnServiceExecuted(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecuted;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the LanguageChanged event
        /// </summary>
        internal static void OnLanguageChanged()
        {
            SettingsEventHandler handler = LanguageChanged;
            if (handler != null)
                handler(Language);
        }

        /// <summary>
        /// raises the PluginInstalled event
        /// </summary>
        /// <param name="item">the plugin being installed</param>
        internal static void OnPluginInstalled(IPlugin item)
        {
            var handler = PluginInstalled;
            if (handler != null)
                handler(item, new EventArgs());
        }

        /// <summary>
        /// raises the PluginUninstalled event
        /// </summary>
        /// <param name="item">the plugin being uninstalled</param>
        internal static void OnPluginUninstalled(IPlugin item)
        {
            var handler = PluginUninstalled;
            if (handler != null)
                handler(item, new EventArgs());
        }

        #endregion
    }
}

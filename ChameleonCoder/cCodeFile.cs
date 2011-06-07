﻿using System;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Collections;

namespace ChameleonCoder
{
    internal class cCodeFile : cFile
    {
        internal cCodeFile(ref XPathNavigator xmlnav, string xpath, string datafile)
            : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.code;
            
            this.Node.ImageIndex = 1;
            
            this.Language = Guid.Parse(xmlnav.SelectSingleNode(xpath + "/@guid").Value);

            try { this.CompilationPath = xmlnav.SelectSingleNode(xpath + "/@compilation-path").Value; }
            catch {
                if (!string.IsNullOrWhiteSpace(this.Path))
                    this.CompilationPath = this.Path + ".exe";
            }
        }

        internal override void Open()
        {
            base.Open();

            ListViewItem item;

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CodeLanguage"), Plugins.PluginManager.GetLanguageName(this.Language) }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CompilePath"), this.CompilationPath}));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_code");
        }

        internal override SortedList ToSortedList()
        {
            SortedList list = base.ToSortedList();

            list.Add("CompilationPath", this.CompilationPath);
            list.Add("Language", this.Language);

            return list;
        }

        #region cCodeFile properties

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        internal Guid Language { get; set; }

        /// <summary>
        /// the path to save the file if it is compiled.
        /// </summary>
        internal string CompilationPath { get; set; }

        #endregion

        #region cCodeFile methods

        internal static new void Create(object sender, EventArgs e)
        {
            Program.Gui.Enabled = true;
            Program.Selector.Close();
        }

        #endregion
    }
}

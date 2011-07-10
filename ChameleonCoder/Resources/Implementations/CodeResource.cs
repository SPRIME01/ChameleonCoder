﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a file containing code,
    /// inherits from FileResource
    /// </summary>
    public class CodeResource : FileResource, ICompilable, IEditable
    {
        /// <summary>
        /// creates a new instance of the CodeResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public CodeResource(XmlNode node)
            : base(node)
        {
            this.compatibleLanguages = new List<Guid>();
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/code.png")); } }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        public Guid language
        {
            get
            {
                try { return new Guid(this.XMLNode.Attributes["language"].Value); }
                catch (NullReferenceException) { return Guid.Empty; }
            }
            protected set
            {
                this.XMLNode.Attributes["language"].Value = value.ToString();
                this.OnPropertyChanged("language");
            }
        }

        public List<Guid> compatibleLanguages { get; set; }

        #endregion

        #region ICompilable

        /// <summary>
        /// the path to save the file if it is compiled.
        /// </summary>
        public string compilationPath
        {
            get
            {
                string result = null;

                try { result = this.XMLNode.Attributes["compilation-path"].Value; }
                catch (NullReferenceException) { }

                if (string.IsNullOrWhiteSpace(result))
                    result = this.Path + ".exe";
                return result;
            }
            protected set
            {
                this.XMLNode.Attributes["compilation-path"].Value = value;
                this.OnPropertyChanged("compilationPath");
            }
        }

        #endregion

        #region IEditable

        public string GetText()
        {
            return System.IO.File.ReadAllText(this.Path);
        }

        public void SaveText(string text)
        {
            System.IO.File.WriteAllText(this.Path, text);
        }

        #endregion

        #region IEnumerable

        System.Collections.IEnumerator baseEnum;

        public override System.Collections.IEnumerator GetEnumerator()
        {
            this.baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            string langName = null;
            try { langName = LanguageModules.LanguageModuleHost.GetModule(this.language).LanguageName; }
            catch (NullReferenceException) { }

            yield return new { Name = "language", Value = langName, Group = "source code" };

            string list = string.Empty;
            foreach (Guid lang in this.compatibleLanguages)
            {
                try { list += LanguageModules.LanguageModuleHost.GetModule(lang).LanguageName + "; "; }
                catch (NullReferenceException) { }
            }

            yield return new { Name = "compatible languages", Value = list, Group = "source code" };
            yield return new { Name = "compilation path", Value = this.compilationPath, Group = "source code" };
        }

        #endregion
    }
}

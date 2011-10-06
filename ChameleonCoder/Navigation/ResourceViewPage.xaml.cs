﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying resource details
    /// </summary>
    internal sealed partial class ResourceViewPage : CCPageBase
    {
        /// <summary>
        /// creates a new instance of this page, given a resource to display
        /// </summary>
        /// <param name="model">the view model to display</param>
        internal ResourceViewPage(ViewModel.ResourceViewPageModel model)
        {
            Initialize(model);
            ResourceManager.Open(model.Resource);
            InitializeComponent();            
        }

        [Obsolete("to be moved to model", false)]
        private void SaveMetadata(object sender, EventArgs e)
        {
        	var box = sender as TextBox;
            string value = box.Text;
            string key = ((KeyValuePair<string, string>)((box.TemplatedParent as ContentPresenter).Parent as GridViewRowPresenter).Content).Key;

            (DataContext as ViewModel.ResourceViewPageModel).Resource.SetMetadata(key, value);
            box.InvalidateProperty(System.Windows.FrameworkElement.WidthProperty);
        }
    }
}

﻿using System;
using System.Windows.Data;
using ChameleonCoder.Resources;

namespace ChameleonCoder.UI.Converters
{
    /// <summary>
    /// this converter returns the 'Background'-brush from a ResourceTypeInfo instance
    /// </summary>
    [ValueConversion(typeof(IResource), typeof(System.Windows.Media.Brush))]
    public sealed class ColorConverter : AppConverterBase, IValueConverter
    {
        /// <summary>
        /// returns the brush corresponding to the resource's type
        /// </summary>
        /// <param name="value">the IResource instance</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>the brush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // check value to be null
            if (value != null)
            {
                // get the background
                var background = App.ResourceTypeMan.GetBackground(value.GetType());
                if (background != null) // check if the type was registered with a null-background
                    return background.GetAsFrozen(); // use GetAsFrozen() to avoid multi-threading issues
                return null;
            }
            // if null: throw exception
            throw new ArgumentNullException("value");
        }

        /// <summary>
        /// this converter does not support converting back.
        /// </summary>
        /// <param name="value">not used</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

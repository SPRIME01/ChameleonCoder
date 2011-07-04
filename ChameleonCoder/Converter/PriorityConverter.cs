﻿using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(ResourceBase), typeof(ImageSource))]
    class PriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;

            while (resource is IResolvable)
                resource = (resource as IResolvable).Resolve();

            ProjectResource project = resource as ProjectResource;

            if (project != null)
            {
                switch (project.Priority)
                {
                    case ProjectResource.ProjectPriority.basic: return Imaging.CreateBitmapSourceFromHBitmap(Priority.low.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(20, 20));
                    case ProjectResource.ProjectPriority.middle: return Imaging.CreateBitmapSourceFromHBitmap(Priority.middle.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(20, 20));
                    case ProjectResource.ProjectPriority.high: return Imaging.CreateBitmapSourceFromHBitmap(Priority.high.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(20, 20));
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}

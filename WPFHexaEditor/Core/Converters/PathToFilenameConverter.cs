﻿//////////////////////////////////////////////
// Apache 2.0  - 2019
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace WpfHexaEditor.Core.Converters
{
    /// <summary>
    /// Used to get the filename with extention.
    /// </summary>
    public sealed class PathToFilenameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is string filename)
                ? Path.GetFileName(filename)
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}
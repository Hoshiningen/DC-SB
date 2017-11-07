using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace DC_SB.Utils
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return !original;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return !original;
        }
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public class PlayerToWMP : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int inputMethod = (int)value;
            return inputMethod == Settings.WMP;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isWMP = (bool)value;
            if (isWMP) return Settings.WMP;
            else return Settings.NAUDIO;
        }
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public class PlayerToNAudio : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int inputMethod = (int)value;
            return inputMethod == Settings.NAUDIO;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNAudio = (bool)value;
            if (isNAudio) return Settings.NAUDIO;
            else return Settings.WMP;
        }
    }

    [ValueConversion(typeof(ObservableCollection<Input.VKeys>), typeof(string))]
    public class VKeysToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keys = value as ObservableCollection<Input.VKeys>;
            if (keys != null)
            {
                string text = string.Join(" + ", keys);
                return text;
            }
            return "Wrong value";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(double), typeof(Visibility))]
    public class SizeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                int threshold = int.Parse((string)parameter);
                double size = (double)value;
                if (size < threshold) return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(List<string>), typeof(string))]
    public class ListOfStringsToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var separator = parameter as string;
            if (separator == "new line") separator = "\n";
            var list = value as List<string>;
            if (separator == null || list == null)
            {
                return list;
            }
            return String.Join(separator, list);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemToIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() >= 2)
            {
                var group = values[0] as Collection<Sound>;
                var groups = values[1] as Collection<Collection<Sound>>;
                if (group != null && groups != null)
                    return groups.IndexOf(group);
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

//#define EMRON

using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenPKW_Mobile
{
    /// <summary>
    /// Konwersja stanu strony na widoczność elementu.
    /// Dodatkowy parametr umożliwia zmianę funkcjonowania:
    /// - invert : negacja widoczności 
    /// </summary>  
    public class PageStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            PageState state = (PageState)value;

            string[] parameters = (parameter != null) ? ((string)parameter).Split('|') : new string[0];
            bool invert = parameters.Contains("invert");
            bool valid = parameters.Contains(state.ToString());

            return valid ^ invert  ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Konwersja stanu strony na kolor elementu.
    /// </summary>
    public class PageStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            PageState state = (PageState)value;
            
            SolidColorBrush brush = (SolidColorBrush)Application.Current.Resources["BackgroundColor"];
            switch(state)
            {
                case PageState.Fail:
                case PageState.Error: 
                    brush = (SolidColorBrush)Application.Current.Resources["ErrorColor"]; 
                    break;
                case PageState.Ready:
                    brush = (SolidColorBrush)Application.Current.Resources["DistinctColor"];
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Konwersja wartości liczbowej na kolor elementu.
    /// </summary>
    public class ValueEntryToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            ValueEntry entry = (ValueEntry)value;
#if EMRON
            if (entry.IsDirty)
                return (SolidColorBrush)Application.Current.Resources["ComplementaryColor"];
#else
            if (entry.IsDirty)
                return null;
#endif

            SolidColorBrush brush = entry.IsValid ?
                (SolidColorBrush)Application.Current.Resources["DistinctColor"] :
                (SolidColorBrush)Application.Current.Resources["ErrorColor"];

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Konwersja wartości liczbowej na tekst elementu.
    /// </summary>
    public class ValueEntryToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            ValueEntry entry = (ValueEntry)value;
#if EMRON
            return entry.ToString()
                + String.Join(",", entry.Hints.Values.Select(hint => String.Format("({0},{1})", hint.Minimum, hint.Maximum)));
#else
            return entry < 0 ? String.Empty : entry.ToString();
#endif
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            string text = (string)value;
            int output;
            if (int.TryParse(text, out output))
                return new ValueEntry(output);
            else return new ValueEntry();
        }
    }

    /// <summary>
    /// Konwersja tablicy elementów na widoczność elementu.
    /// Element jest widoczny, gdy tablica nie jest pusta.
    /// </summary>
    public class ArrayToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            if (value.GetType().IsArray == false)
                return null;

            IEnumerable<object> items = (IEnumerable<object>)value;
            return (items.Count() == 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return default(int);
            
            try
            {                
                return value.GetHashCode();
            }
            catch
            {
                return default(int);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

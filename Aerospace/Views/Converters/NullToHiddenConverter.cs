using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Aerospace.Views.Converters
{
    internal class NullToHiddenConverter : MarkupExtension, IValueConverter
    {
        #region Constants and Fields

        private static readonly IValueConverter Instance = new NullToHiddenConverter();

        #endregion

        #region IValueConverter Implementation

        /// <inheritdoc />
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture) =>
            value is null ? Visibility.Hidden : Visibility.Visible;

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new InvalidOperationException();

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider) => Instance;

        #endregion
    }
}
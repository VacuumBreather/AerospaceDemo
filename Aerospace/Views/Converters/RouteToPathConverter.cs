using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Aerospace.Model;

namespace Aerospace.Views.Converters
{
    internal class RouteToPathConverter : MarkupExtension, IValueConverter
    {
        #region Constants and Fields

        private static readonly IValueConverter Instance = new RouteToPathConverter();

        #endregion

        #region IValueConverter Implementation

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ObservableCollection<Planet> route)
            {
                return DependencyProperty.UnsetValue;
            }

            const double YPos = 478.0;
            const double YOffset = 150.0;
            const double XStart = 200.0;
            const double XOffset = 125.0;

            var pathGeometry = new PathGeometry();

            if (route.Count < 2)
            {
                return pathGeometry;
            }

            var start = new Point(XStart + (route[0].Index * XOffset), YPos);

            var segments = new List<PathSegment>();

            for (var i = 1; i <= route.Count; i++)
            {
                int fromIndex = route[i - 1].Index;
                int toIndex = i < route.Count ? route[i].Index : 3;

                double yOffsetFactor = fromIndex > toIndex ? -1.0 : 1.0;

                double fromX = XStart + (fromIndex * XOffset);
                double toX = XStart + (toIndex * XOffset);

                var midPoint = new Point(fromX + ((toX - fromX) * 0.5), YPos - (YOffset * yOffsetFactor));
                var endPoint = new Point(XStart + (toIndex * XOffset), YPos);

                var segment = new QuadraticBezierSegment(midPoint, endPoint, true);

                segments.Add(segment);
            }

            var pathFigure = new PathFigure(start, segments, false);

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

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
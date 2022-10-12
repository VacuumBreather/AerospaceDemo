using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Jace;

namespace Aerospace.Views.Converters
{
    /// <summary>Converts a mathematical expression into its result.</summary>
    /// <seealso cref="IValueConverter" />
    /// <seealso cref="IMultiValueConverter" />
    /// <remarks>See https://github.com/pieterderycke/Jace/wiki/Getting-Started for supported expressions.</remarks>
    public class MathExpressionConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        #region Constants and Fields

        private static readonly IDictionary<string, Func<IDictionary<string, double>, double>> CachedFormulas =
            new Dictionary<string, Func<IDictionary<string, double>, double>>();

        private static readonly CalculationEngine CalculationEngine = new(CultureInfo.InvariantCulture);

        private static readonly IValueConverter Instance = new MathExpressionConverter();

        private static readonly Dictionary<string, double> Parameters = new();

        #endregion

        #region IMultiValueConverter Implementation

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string expression) || string.IsNullOrEmpty(expression) || values is null)
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                return CalculateResult(values, expression);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();

        #endregion

        #region IValueConverter Implementation

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string expression) || string.IsNullOrEmpty(expression))
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                return CalculateResult(new[] {value}, expression);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider) => Instance;

        #endregion

        #region Private Methods

        private static object CalculateResult(object[] values, string expression)
        {
            object result;
            double[] doubleValues = ToValidDoubles(values).ToArray();

            if (!doubleValues.Any(double.IsNaN))
            {
                // Plan A - All values are doubles.
                Parameters.Clear();

                foreach ((double val, int i) in doubleValues.Select((val, i) => (val, i)))
                {
                    Parameters.Add($"var{i}", val);
                }

                var formula = GetFormula(values, expression);

                result = formula.Invoke(Parameters);
            }
            else
            {
                // Plan B - Values might be interpretable strings.
                //          Rebuild the expression by directly inserting the values.
                expression = string.Format(CultureInfo.InvariantCulture, expression, values);

                result = CalculationEngine.Calculate(expression);
            }

            return result;
        }

        private static string CreateVariableExpression(IEnumerable<object> values, string expression)
        {
            string variableExpression = string.Format(CultureInfo.InvariantCulture,
                expression,
                values.Select((_, i) => $"var{i}").Cast<object>().ToArray());

            return variableExpression;
        }

        private static Func<IDictionary<string, double>, double> GetFormula(IEnumerable<object> values,
            string expression)
        {
            string variableExpression = CreateVariableExpression(values, expression);

            if (!CachedFormulas.TryGetValue(variableExpression, out var formula))
            {
                // Build formula and cache it.
                formula = CalculationEngine.Build(variableExpression);
                CachedFormulas[variableExpression] = formula;
            }

            return formula;
        }

        private static double ToValidDouble(object value)
        {
            if (value is double d1)
            {
                return d1;
            }

            return double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double d2)
                ? d2
                : double.NaN;
        }

        private static IEnumerable<double> ToValidDoubles(IEnumerable<object> values) => values.Select(ToValidDouble);

        #endregion
    }
}
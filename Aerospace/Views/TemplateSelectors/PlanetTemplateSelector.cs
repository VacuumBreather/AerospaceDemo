using System.Windows;
using System.Windows.Controls;
using Aerospace.Model;

namespace Aerospace.Views.TemplateSelectors
{
    internal class PlanetTemplateSelector : DataTemplateSelector
    {
        #region Public Properties

        public DataTemplate CeresTemplate { get; set; }

        public DataTemplate EarthTemplate { get; set; }

        public DataTemplate FallbackTemplate { get; set; }

        public DataTemplate JupiterTemplate { get; set; }

        public DataTemplate MarsTemplate { get; set; }
        public DataTemplate MercuryTemplate { get; set; }

        public DataTemplate NeptuneTemplate { get; set; }

        public DataTemplate PlutoTemplate { get; set; }

        public DataTemplate SaturnTemplate { get; set; }

        public DataTemplate UranusTemplate { get; set; }

        public DataTemplate VenusTemplate { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is not Planet planet)
            {
                return null;
            }

            return planet.Index switch
            {
                1 => MercuryTemplate,
                2 => VenusTemplate,
                3 => EarthTemplate,
                4 => MarsTemplate,
                5 => CeresTemplate,
                6 => JupiterTemplate,
                7 => SaturnTemplate,
                8 => UranusTemplate,
                9 => NeptuneTemplate,
                10 => PlutoTemplate,
                _ => FallbackTemplate
            };
        }

        #endregion
    }
}
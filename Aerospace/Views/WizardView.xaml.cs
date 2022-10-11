using System.Windows;
using Aerospace.ViewModels;
using Xceed.Wpf.Toolkit;

namespace Aerospace.Views
{
    /// <summary>
    ///     Interaction logic for WizardView.xaml
    /// </summary>
    public partial class WizardView
    {
        #region Event Handlers

        private void Wizard_OnPageChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is WizardViewModel viewModel &&
                sender is Wizard wizard &&
                wizard.CurrentPage.Content is FrameworkElement {DataContext: IWizardStepViewModel model})
            {
                viewModel.ActiveItem = model;
            }
        }

        #endregion
    }
}
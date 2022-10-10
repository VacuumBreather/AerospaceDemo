using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aerospace.ViewModels;
using Xceed.Wpf.Toolkit;

namespace Aerospace.Views
{
    /// <summary>
    /// Interaction logic for WizardView.xaml
    /// </summary>
    public partial class WizardView : UserControl
    {
        public WizardView()
        {
            InitializeComponent();
        }

        private void Wizard_OnPageChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is WizardViewModel viewModel &&
                sender is Wizard wizard &&
                wizard.CurrentPage.Content is FrameworkElement {DataContext: IWizardStepViewModel model})
            {
                viewModel.ActiveItem = model;
            }
        }
    }
}

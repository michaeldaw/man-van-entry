using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ManVan
{
    /// <summary>
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        public ConfirmWindow()
        {
            InitializeComponent();
        }

        private void Negative(object sender, RoutedEventArgs e)
        {
            var context = (ConfirmViewModel)DataContext;
            context.Result = ConfirmResult.Negative;
            DialogResult = false;
            Close();
        }

        private void Positive(object sender, RoutedEventArgs e)
        {
            var context = (ConfirmViewModel) DataContext;
            context.Result = ConfirmResult.Affirmative;
            DialogResult = true;
            Close();
        }
    }
}

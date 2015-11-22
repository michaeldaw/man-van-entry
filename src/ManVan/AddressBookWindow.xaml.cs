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
    /// Interaction logic for AddressBookWindow.xaml
    /// </summary>
    public partial class AddressBookWindow : Window
    {
        public AddressBookWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Names_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var context = (AddressBookViewModel)DataContext;
            var item = ItemsControl.ContainerFromElement(Names, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                if (((string)item.Content) != context.Name)
                    context.Name = (string)item.Content;
            }
        }

        private void AddressBookWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ManVan
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        public DetailsWindow()
        {
            InitializeComponent();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel this new entry?", "Cancel",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            DialogResult = false;
            Close();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void DetailsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxFirst.Focus();
        }

        private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void DetailsWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            if (CancelButton.IsFocused)
                Cancel_OnClick(this, null);
            Save_OnClick(this, null);
        }
    }
}

using System;
using System.Windows;
using System.Windows.Input;

namespace ManVan
{
    public class OpenDetailsCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public OpenDetailsCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var details = new DetailsWindow
            {
                DataContext = _viewModel.SelectedEntry,
                CancelButton = {Visibility = Visibility.Hidden},
                DeleteButton = {Visibility = Visibility.Visible},
            };
            var result = details.ShowDialog();

            if (result != null && result.Value == false)
            {
                _viewModel.Entries.Remove(_viewModel.SelectedEntry);
                _viewModel.SelectedEntry = null;
            }

            LocalDataService.Save(_viewModel.Entries);
            _viewModel.Refresh();
        }

        public event EventHandler CanExecuteChanged;

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
using System;
using System.Windows;
using System.Windows.Input;

namespace ManVan
{
    public class NewEntryCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public NewEntryCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var entry = new EntryViewModel()
            {
                Index = _viewModel.Entries.Count + 1
            };

            var details = new DetailsWindow
            {
                DataContext = entry,
                CancelButton = {Visibility = Visibility.Visible},
                DeleteButton = {Visibility = Visibility.Hidden}
            };

            var result = details.ShowDialog();

            if (result.HasValue && result.Value)
                _viewModel.Entries.Add(entry);

            if (_viewModel.Entries.Count > 0)
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
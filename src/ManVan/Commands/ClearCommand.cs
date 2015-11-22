using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ManVan
{
    public class ClearCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public ClearCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var vm = new ConfirmViewModel()
            {
                Caption = "Clear Entries?",
                Message = "Are you sure you want to clear all" +
                          " the listed entries?\n" +
                          "\nThis action cannot be undone."
            };

            var window = new ConfirmWindow
            {
                DataContext = vm,
            };
            window.ShowDialog();
            var result = vm.Result;

            if (result == ConfirmResult.Affirmative)
            {
                _viewModel.Entries = new ObservableCollection<EntryViewModel>();
                LocalDataService.Save(_viewModel.Entries);
                _viewModel.Refresh();
            }
        }

        public event EventHandler CanExecuteChanged;

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
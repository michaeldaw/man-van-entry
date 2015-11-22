using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace ManVan
{
    public class ExportLabelsCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public ExportLabelsCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var directory = new DirectoryInfo(
                LeitzAddressBookService.AddressBookDirectoryPath);
            if (!directory.Exists)
                throw new Exception("Directory should always exist here");

            var exportWindow = new AddressBookWindow();
            var result = exportWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var context = (AddressBookViewModel) exportWindow.DataContext;
                if (LeitzAddressBookService.ExistingAddressBookNames.Contains(
                    context.Name))
                {
                    LeitzAddressBookService.AppendTo(context.Name,
                        _viewModel.Entries.Select(x => x.ToAddressModel));
                }
                else
                {
                    LeitzAddressBookService.CreateNew(context.Name,
                        _viewModel.Entries.Select(x => x.ToAddressModel));
                }
            }
        }

        public event EventHandler CanExecuteChanged;

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
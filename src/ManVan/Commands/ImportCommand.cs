using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace ManVan
{
    public class ImportCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public ImportCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dlg = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "ManVan Entry Files|*.manvan",
                InitialDirectory = Environment.GetFolderPath(
                    Environment.SpecialFolder.Desktop),
            };
            var result = dlg.ShowDialog();
            if (result.HasValue && !result.Value)
            {
                return;
            }
            try
            {
                var path = dlg.FileName;
                var file = new FileInfo(path);
                if (!file.Exists)
                    throw new FileNotFoundException("The file does not exist.");
                var str = File.ReadAllText(file.FullName);
                var entries = _viewModel.Entries;
                var xs = new XmlSerializer(entries.GetType());
                var list = (ObservableCollection<EntryViewModel>)xs.Deserialize(new StringReader(str));
                list.ToList().ForEach(x =>
                {
                    if(!_viewModel.EntriesContainsId(x.Id))
                        entries.Add(x);
                });
                _viewModel.Refresh();
            }
            catch (Exception exception)
            {
                var vm = new ConfirmViewModel()
                {
                    Caption = "Unexpected Error",
                    Message = exception.Message,
                    NegativeVisibility = Visibility.Hidden,
                    AffirmativeLabel = "Okay",
                };

                var window = new ConfirmWindow
                {
                    DataContext = vm,
                };
                window.ShowDialog();
            }
        }

        public event EventHandler CanExecuteChanged;

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
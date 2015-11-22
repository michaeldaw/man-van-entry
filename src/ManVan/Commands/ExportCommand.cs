using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace ManVan
{
    public class ExportCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public ExportCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dlg = new SaveFileDialog()
            {
                DefaultExt = "manvan",
                Filter = "ManVan Entry Files|*.manvan",
                RestoreDirectory = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = "manvan_" + DateTime.Now.ToString("yyyy-MM-dd"),
            };
            var result = dlg.ShowDialog();
            if (result.HasValue && !result.Value)
            {
                return;
            }
            try
            {
                var entries = _viewModel.Entries;

                var xs = new XmlSerializer(entries.GetType());

                var path = dlg.FileName;

                using (TextWriter writer = new StreamWriter(path))
                {
                    xs.Serialize(writer, entries);
                }
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
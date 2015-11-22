using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace ManVan
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            OpenDetailsCommand = new OpenDetailsCommand(this);
            NewEntryCommand = new NewEntryCommand(this);
            ImportCommand = new ImportCommand(this);
            ExportCommand = new ExportCommand(this);
            ExportLabelsCommand = new ExportLabelsCommand(this);
            ClearCommand = new ClearCommand(this);
            Entries = LocalDataService.Load();

            _exportVisibility = LeitzAddressBookService.IsLeitzIconInstalled
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        
        public ClearCommand ClearCommand { get; set; }

        public ExportLabelsCommand ExportLabelsCommand { get; set; }

        public ExportCommand ExportCommand { get; set; }

        public ImportCommand ImportCommand { get; set; }

        public NewEntryCommand NewEntryCommand { get; }

        public OpenDetailsCommand OpenDetailsCommand { get; }

        private Visibility _exportVisibility;

        public Visibility ExportVisibility
        {
            get { return _exportVisibility; }
            set
            {
                _exportVisibility = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EntryViewModel> Entries { get; set; }

        public EntryViewModel SelectedEntry { get; set; }

        public void Refresh()
        {
            OnPropertyChangedExplicit("Entries");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChangedExplicit(
            string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EntryViewModel : INotifyPropertyChanged
    {

        private int _year = 1900;
        private int _month = 01;
        private int _day = 01;
        private int _age;
        private string _lastName;
        private string _firstName;
        private string _province = "AB";

        public Guid Id { get; } = new Guid();
        public DateTime DateEntered { get; } = DateTime.Now;
        public int Index { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        public AddressModel ToAddressModel =>
            new AddressModel(
                FirstName, LastName, Address, City, Province, PostalCode);

        public bool PriorPsa { get; set; }
        public bool FamilyDoctor { get; set; }
        public string ReferralMethod { get; set; }
        public bool FamilyHistory { get; set; }
        public double PsaValue { get; set; }

        public string Province
        {
            get { return _province; }
            set
            {
                _province = value;
                OnPropertyChanged();
            }
        }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTime Birthdate { get; set; } = new DateTime(1900, 01, 01);
        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged();
                Birthdate = new DateTime(_year, Birthdate.Month, Birthdate.Day);
                CalculateAge();
            }
        }

        public int Month
        {
            get { return _month; }
            set
            {
                _month = value;
                OnPropertyChanged();
                Birthdate = new DateTime(Birthdate.Year, _month, Birthdate.Day);
                CalculateAge();
            }
        }

        public int Day
        {
            get { return _day; }
            set
            {
                _day = value;
                OnPropertyChanged();
                Birthdate = new DateTime(Birthdate.Year, Birthdate.Month, _day);
                CalculateAge();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - Birthdate.Year;
            if (Birthdate > today.AddYears(-age)) age--;
            Age = age;
        }
    }

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
                list.ToList().ForEach(x => entries.Add(x));
                _viewModel.Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unexpected Error: " + exception.Message);
            }
        }

        public event EventHandler CanExecuteChanged;

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

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
                MessageBox.Show("Unexpected Error: " + exception.Message);
            }
        }

        public event EventHandler CanExecuteChanged;

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

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
            var result = MessageBox.Show(
                "Are you sure you want to clear all the listed entries?\n" +
                "\nThis action cannot be undone.", "Clear Entries?",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
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
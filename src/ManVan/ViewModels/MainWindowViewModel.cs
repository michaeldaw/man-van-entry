using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

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

        public bool EntriesContainsId(Guid id)
        {
            return Entries.Any(entryViewModel => entryViewModel.Id == id);
        }

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
}
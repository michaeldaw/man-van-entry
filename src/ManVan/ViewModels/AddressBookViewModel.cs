using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManVan
{
    public class AddressBookViewModel : INotifyPropertyChanged
    {
        public AddressBookViewModel()
        {
            Name = "ManVan Addresses_" + DateTime.Now.ToString("yyyy-MM-dd");
            BookNames = new ObservableCollection<string>(
                LeitzAddressBookService.ExistingAddressBookNames);
        }

        public ObservableCollection<string> BookNames { get; set; }

        private string _selectedName;

        public string SelectedName
        {
            get { return _selectedName; }
            set
            {
                _selectedName = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(_selectedName))
                    Name = _selectedName;
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
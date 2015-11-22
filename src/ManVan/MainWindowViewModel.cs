using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ManVan
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

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

        public ObservableCollection<MainEntryViewModel> Entries { get; set; } =
                new ObservableCollection<MainEntryViewModel>();

        public MainEntryViewModel SelectedEntry { get; set; }

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

    public class MainEntryViewModel : INotifyPropertyChanged
    {

        private int _year = 1900;
        private int _month = 01;
        private int _day = 01;
        private int _age;

        public Guid Id { get; set; } = new Guid();
        public DateTime DateEntered { get; set; } = DateTime.Now;
        public int Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
        public string Province { get; set; }
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
}
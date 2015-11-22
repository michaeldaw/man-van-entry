using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManVan
{
    public class EntryViewModel : INotifyPropertyChanged
    {

        private int _year = 1900;
        private int _month = 01;
        private int _day = 01;
        private int _age;
        private string _lastName;
        private string _firstName;
        private string _province = "AB";
        private string _referralMethod;
        private bool _familyHistory;
        private double _psaValue;
        private string _address;
        private string _city;
        private string _postalCode;
        private DateTime _birthdate = new DateTime(1900, 01, 01);
        private bool _familyDoctor;
        private bool _priorPsa;

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

        public bool PriorPsa
        {
            get { return _priorPsa; }
            set
            {
                _priorPsa = value;
                OnPropertyChanged();
            }
        }

        public bool FamilyDoctor
        {
            get { return _familyDoctor; }
            set
            {
                _familyDoctor = value;
                OnPropertyChanged();
            }
        }

        public string ReferralMethod
        {
            get { return _referralMethod; }
            set
            {
                _referralMethod = value;
                OnPropertyChanged();
            }
        }

        public bool FamilyHistory
        {
            get { return _familyHistory; }
            set
            {
                _familyHistory = value;
                OnPropertyChanged();
            }
        }

        public double PsaValue
        {
            get { return _psaValue; }
            set
            {
                _psaValue = value;
                OnPropertyChanged();
            }
        }

        public string Province
        {
            get { return _province; }
            set
            {
                _province = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged();
            }
        }

        public DateTime Birthdate
        {
            get { return _birthdate; }
            set
            {
                _birthdate = value;
                OnPropertyChanged();
            }
        }

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
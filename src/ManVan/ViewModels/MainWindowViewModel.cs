﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ManVan
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string AddressBookDirectoryPath { get; } =
            Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments) 
                + "\\Leitz Icon\\Address Books";




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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
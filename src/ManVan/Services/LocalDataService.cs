using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace ManVan
{
    public class LocalDataService
    {
        private static readonly string StoragePath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            + "\\ManVan\\entries.manvan";

        public static void Save(IEnumerable<EntryViewModel> entries)
        {
            var xs = new XmlSerializer(entries.GetType());

            var path = StoragePath;
            var file = new FileInfo(path);
            if (file.DirectoryName == null)
                throw new Exception("DirectoryName should not be null");
            if (!Directory.Exists(file.DirectoryName))
                Directory.CreateDirectory(file.DirectoryName);

            using (TextWriter writer = new StreamWriter(path))
            {
                xs.Serialize(writer, entries);
            }
        }

        public static ObservableCollection<EntryViewModel> Load()
        {
            var file = new FileInfo(StoragePath);
            if (file.DirectoryName == null)
                throw new Exception("Invalid directory name");
            var dir = new DirectoryInfo(file.DirectoryName);
            if (!dir.Exists)
                Directory.CreateDirectory(file.DirectoryName);
            if (!file.Exists)
                return new ObservableCollection<EntryViewModel>();
            var str = File.ReadAllText(file.FullName);

            var xs = new XmlSerializer(typeof(ObservableCollection<EntryViewModel>));
            var list = (ObservableCollection<EntryViewModel>)xs.Deserialize(new StringReader(str));
            return list;
        }
    }
}
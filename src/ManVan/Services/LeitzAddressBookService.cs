using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Xml.Linq;

namespace ManVan
{
    public class LeitzAddressBookService
    {
        public static string AddressBookDirectoryPath { get; } =
            Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments)
            + "\\Leitz Icon\\Address Books";

        public static bool IsLeitzIconInstalled => new DirectoryInfo(
            AddressBookDirectoryPath).Exists;

        public static string[] ExistingAddressBookNames => new DirectoryInfo(
            AddressBookDirectoryPath).GetFiles("*.LeitzAB")
            .Select(x => Path.GetFileNameWithoutExtension(x.Name)).ToArray();

        public static void CreateNew(string name, IEnumerable<AddressModel> addresses)
        {
            var book = new AddressBookModel(name);
            book.Addresses.AddRange(addresses);
            var doc = book.ToDoc;
            doc.Save(GetFileForBookName(name));
        }

        public static void AppendTo(string name, IEnumerable<AddressModel> addresses)
        {
            var text = File.ReadAllText(GetFileForBookName(name));
            File.Delete(GetFileForBookName(name));
            var doc = XDocument.Parse(text);
            var book = new AddressBookModel(name, addresses);
            book.MergeWithExisting(doc);
            doc.Save(GetFileForBookName(name));
        }

        private static string GetFileForBookName(string name) =>
            AddressBookDirectoryPath + "\\" + name + ".LeitzAB";
    }
}
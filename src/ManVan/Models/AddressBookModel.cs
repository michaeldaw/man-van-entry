using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ManVan
{
    public class AddressBookModel
    {
        public AddressBookModel(string name)
        {
            Name = name;
        }

        public AddressBookModel(
            string name, IEnumerable<AddressModel> addresses )
        {
            Name = name;
            Addresses.AddRange(addresses);
        }

        public string Name { get; }

        public List<AddressModel> Addresses { get; } =
            new List<AddressModel>();

        public IEnumerable<XElement> AddressElements =>
            Addresses.Select(x => x.ToElement);

        public XDocument ToDoc =>
            new XDocument(
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement("Addresses",
                    AddressElements));

        public XDocument MergeWithExisting(XDocument doc)
        {
            var addresses = doc.Element("Addresses");
            if (addresses == null)
                return null;
            addresses.Add(AddressElements);
            return doc;
        }
    }
}
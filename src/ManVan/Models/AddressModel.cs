using System.Xml.Linq;

namespace ManVan
{
    public class AddressModel
    {
        public AddressModel(string firstName, string lastName, string address, string city, string province, string postalCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            Province = province;
            PostalCode = postalCode;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Address { get; }
        public string City { get; }
        public string Province { get; }
        public string PostalCode { get; }

        public XElement ToElement =>
            new XElement("Address",
                new XElement("FileAs", $"{LastName}, {FirstName}" ),
                new XElement("Data", DataString));

        private string DataString =>
            $"{FirstName} {LastName}\n" +
            $"{Address}\n" +
            $"{City}, {Province}\n" +
            $"{PostalCode}";
    }
}
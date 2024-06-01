using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToDB.Mapping;


namespace WebAddressbookTests
{
    [Table(Name = "addressbook")]
    public class ContactData : IEquatable<ContactData>, IComparable<ContactData>
    {
        private string allPhones;
        private string allEmails;
        private string accountProperties;
        private string allPhonesFromProperties;

        public ContactData()
        {
            
        }

        public ContactData(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
            Address = "TestAddress";
            MobilePhone = "11111111";
            WorkPhone = "22222222";
            HomePhone = "33333333";
            Email = "email 1";
            Email_2 = "email 2";
            Email_3 = "email 3";
        }

        public ContactData(string name, string lastName, string address)
        {
            Name = name;
            LastName = lastName;
            Address = address;
            MobilePhone = "121212";
            WorkPhone = "32323223";
            HomePhone = "600000000";
            Email = "wrong email";
            Email_2 = "wrong email 2";
            Email_3 = "wrong email 3";
        }


        [Column(Name = "firstname")]
        public string Name { get; set; }
        [Column(Name = "lastname")]
        public string LastName { get; set; }
        [Column(Name = "address")]
        public string Address { get; set; }
        [Column(Name = "mobile")]
        public string MobilePhone { get; set; }
        [Column(Name = "work")]
        public string WorkPhone { get; set; }
        [Column(Name = "home")]
        public string HomePhone { get; set; }
        [Column(Name = "email")]
        public string Email { get; set; }
        [Column(Name = "email2")]
        public string Email_2 { get; set; }
        [Column(Name = "email3")]
        public string Email_3 { get; set; }
        [Column(Name = "id"), PrimaryKey]
        public string Id { get; set; }
        [Column(Name = "Deprecated")]
        public string Deprecated { get; set; }

        public string HomePhonePresents(string homePhone)
        {
            if (string.IsNullOrEmpty(homePhone))
            {
                return "";
            }
            return "H: " + homePhone + "\r\n";
        }

        public string MobilePhonePresents(string mobilePhone)
        {
            if (string.IsNullOrEmpty(mobilePhone))
            {
                return "";
            }
            return "M: " + mobilePhone + "\r\n";
        }

        public string WorkPhonePresents(string workPhone)
        {
            if (string.IsNullOrEmpty(workPhone))
            {
                return "";
            }
            return "W: " + workPhone + "\r\n";
        }

        public string PropertiesPresents(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                return "";
            }
            return property + "\r\n";
        }

        public string NamePresents(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }
            return name + " ";
        }

        public string LastNamePresents(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                return "";
            }
            return lastName + " ";
        }

        public string AllPhones 
        { 
            get 
            {
                if (allPhones != null)
                {
                    return allPhones;
                }
                else
                {
                    return (CleanUp(HomePhone) + CleanUp(MobilePhone) + CleanUp(WorkPhone)).Trim();
                }
            } 
            set
            {
                allPhones = value;
            }
        }

        public string AllPhonesFromProperties
        {
            get
            {
                if (allPhonesFromProperties != null)
                {
                    return allPhonesFromProperties;
                }
                else
                { 
                    if (string.IsNullOrEmpty(HomePhone) && string.IsNullOrEmpty(MobilePhone) && string.IsNullOrEmpty(WorkPhone))
                    {
                        return "";
                    }
                    return (HomePhonePresents(HomePhone) + MobilePhonePresents(MobilePhone) + WorkPhonePresents(WorkPhone)) + "\r\n";
                }
            }
            set
            {
                allPhonesFromProperties = value;
            }
        }

        public string AllEmails
        {
            get
            {
                if (allEmails != null)
                {
                    return allEmails;
                }
                else
                {
                    return (CleanUp(Email) + CleanUp(Email_2) + CleanUp(Email_3)).Trim();
                }
            }
            set
            {
                allEmails = value;
            }
        }

        public string AccountProperties
        {
            get
            {
                if (accountProperties != null)
                {
                    return accountProperties;
                }
                else
                {
                    return ((NamePresents(Name) + LastNamePresents(LastName)).Trim() + "\r\n" + PropertiesPresents(Address) + "\r\n"
                            + AllPhonesFromProperties 
                            + PropertiesPresents(Email) + "\r\n" + PropertiesPresents(Email_2) + "\r\n" + PropertiesPresents(Email_3)).Trim();
                }
            }
            set
            {
                accountProperties = value;
            }
        }

        private string CleanUp(string phone)
        {
            if (phone == null || phone == "")
            {
                return "";
            }

            return Regex.Replace(phone, "[ -()]", "") + "\r\n";
        }

        public bool Equals(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            return Name == other.Name && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return "name" + Name + "\nlastname" + LastName + "\naddress" + Address;
        }

        public int CompareTo(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return 1;
            }
            int compareResult = LastName.CompareTo(other.LastName);
            if (compareResult != 0) {
                return compareResult; }
            return Name.CompareTo(other.Name);
        }

        public static List<ContactData> GetAll()
        {
            using (AddressBookDB db = new AddressBookDB())
            {
                return (from c in db.Accounts.Where(x => x.Deprecated == null) select c).ToList();
            }
        }

        public static string TopContactId()
        {
            using (AddressBookDB db = new AddressBookDB())
            {
                return (from c in db.Accounts.Where(x => x.Deprecated == null) select c).Max(y => y.Id);
            }
        }
    }
}

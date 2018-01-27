using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SMFS.Model
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [DebuggerDisplay("Person: {FirstName} {LastName}")]
    public class Person : IComparable<Person>
    {
        public Person()
        {
            Id = 0;
            FirstName = "";
            LastName = "";
            Address = "";
            Phone = "";
            Email = "";
            Notes = "";
            Hidden = false;
        }

        public Person(IDataRecord rs)
        {
            Id = (long)rs["id"];
            FirstName = (string)rs["firstname"];
            LastName = (string)rs["lastname"];
            Address = (string)rs["address"];
            Phone = (string)rs["phone"];
            Email = (string)rs["email"];
            Notes = (string)rs["notes"];
            Hidden = (bool)rs["hidden"];
        }

        public Person(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            Address = person.Address;
            Phone = person.Phone;
            Email = person.Email;
            Notes = person.Notes;
            Hidden = person.Hidden;
        }

        public string Address { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public bool Hidden { get; set; }
        public long Id { get; set; }
        public string LastName { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }

        public int CompareTo(Person other)
        {
            var result = String.Compare(LastName, other.LastName, StringComparison.Ordinal);
            if (result == 0) result = String.Compare(FirstName, other.FirstName, StringComparison.Ordinal);
            if (result == 0) result = String.Compare(Address, other.Address, StringComparison.Ordinal);
            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Email != null ? Email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Hidden.GetHashCode();
                hashCode = (hashCode * 397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(Person other)
        {
            return Id == other.Id
                && string.Equals(FirstName, other.FirstName)
                && string.Equals(LastName, other.LastName)
                && string.Equals(Address, other.Address)
                && string.Equals(Phone, other.Phone)
                && string.Equals(Email, other.Email)
                && Hidden == other.Hidden
                && string.Equals(Notes, other.Notes);
        }

        public bool InFilter(bool showHidden, string filter)
        {
            if (!showHidden && Hidden) return false;
            if (string.IsNullOrEmpty(filter)) return true;
            var fullName = FirstName + " " + LastName;
            return fullName.ToLower().Contains(filter.ToLower());
        }
    }
}
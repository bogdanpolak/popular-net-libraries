using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace PopularNetLibraries.AutoFixture
{
    public class AutoFixtureTests
    {
        private readonly Fixture _fixture = new();
        private const int LengthOfGuid = 36;
        private static readonly TimeSpan YearTimeSpan = TimeSpan.FromDays(366);

        [Fact]
        public void Create_String()
        {
            var firstName = _fixture.Create<string>();
            firstName.Should().HaveLength(LengthOfGuid);
        }

        [Fact]
        public void Create_Record_User()
        {
            var user = _fixture.Create<User>();
            user.Login.Should().HaveLength("Login".Length + LengthOfGuid);
            user.FirstName.Should().HaveLength("FirstName".Length + LengthOfGuid);
            user.LastName.Should().HaveLength("LastName".Length + LengthOfGuid);
        }

        [Fact]
        public void Create_ComplexClass_Friend()
        {
            _fixture.Register( () => ContactType.Mobile );
            var friend = _fixture.Create<Friend>();
            friend.Contacts.Should().HaveCount(3);
            friend.Contacts.ForEach(contact =>
            {
                contact.Type.Should().Be(ContactType.Mobile);
                contact.Created.Should().BeCloseTo(DateTime.Now, 2 * YearTimeSpan);
            });
        }
    }

    public record User(string Login, string FirstName, string LastName);

    public class Friend
    {
        public string FullName;
        public List<Contact> Contacts;
    }

    public enum ContactType { Mobile, Email, Skype, WhatsApp, Messenger };

    public record Contact(ContactType Type, string Address, DateTime Created);
}
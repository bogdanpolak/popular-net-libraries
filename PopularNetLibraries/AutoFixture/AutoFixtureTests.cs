using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            var friend = _fixture.Create<Friend>();
            friend.Contacts.Should().HaveCount(3);
            friend.Contacts.ForEach(contact =>
            {
                contact.Created.Should().BeCloseTo(DateTime.Now, 2 * YearTimeSpan);
            });
        }

        [Fact]
        public void Create_EmailAddress()
        {
            var email = _fixture.Create<MailAddress>();
            email.Address.Should().Contain("@example.");
            email.Address.Should().HaveLength(LengthOfGuid + "@example.".Length + 3);
        }

        [Fact]
        public void Create_Dictionary()
        {
            var dict = _fixture.Create<Dictionary<byte, string>>();
            dict.Should().HaveCountGreaterOrEqualTo(2); // Fixture can generate two same keys 
            dict.Keys.Max().Should().BeGreaterThan(0);  // Keys: 0 .. 255
            dict.Values.ToList().ForEach(
                value => value.Should().HaveLength("value".Length + LengthOfGuid));
        }

        [Fact]
        public void CreateMany_Ints()
        {
            const int count = 13;
            var numbers = _fixture.CreateMany<int>(count).ToArray();
            numbers.Should().HaveCount(count);
        }

        [Fact]
        public void Build_With()
        {
            var luke = _fixture.Build<Friend>()
                .With( user => user.FullName, "Luke Skywalker" )
                .Create();
            luke.FullName.Should().Be("Luke Skywalker");
        }

        [Fact]
        public void Build_With_UsingValueFactory()
        {
            var friends = _fixture.Build<Friend>()
                .With(p => p.FullName, (int ordinal) => $"Friend {ordinal}")
                .CreateMany(2)
                .ToList();
            friends.ForEach( f => f.FullName.Should().Contain("Friend "));
        }

        [Fact]
        public void Inject()
        {
            _fixture.Inject( ContactType.Mobile );
            var friend = _fixture.Create<Friend>();
            friend.Contacts.Should().HaveCount(3);
            friend.Contacts[0].Type.Should().Be(ContactType.Mobile);
            friend.Contacts[2].Type.Should().Be(ContactType.Mobile);
        }

        [Fact]
        public void InstantiateClasses()
        {
            var user = new User(
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>());
            user.Should().NotBeNull();

            var skypeContact = new Contact(
                ContactType.Skype, 
                _fixture.Create<string>(), 
                _fixture.Create<DateTime>());
            var friend = new Friend
            {
                FullName = _fixture.Create<string>(),
                Contacts = new List<Contact>(new[]
                {
                    skypeContact, _fixture.Create<Contact>()
                })
            };
            friend.Should().NotBeNull();
            friend.Contacts.Should().HaveCount(2);
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
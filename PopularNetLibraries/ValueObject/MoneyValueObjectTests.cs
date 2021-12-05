using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PopularNetLibraries.ValueObject
{
    public class MoneyValueObjectTests
    {
        [Fact]
        public void CreatePriceValueObject_VerifyProperties()
        {
            var giftCardPrice = new Money(100, Currency.PLN);
            giftCardPrice.Amount.Should().Be(100m);
            giftCardPrice.Currency.Symbol.Should().Be("PLN");
        }

        [Fact]
        public void PricesNotEqual_WhenTwoDifferentCurrencies()
        {
            var giftCardPrice = new Money(100, Currency.PLN);
            var speakerPrice = new Money(100, Currency.USD);
            giftCardPrice.Should().NotBe(speakerPrice);
        }

        [Fact]
        public void EqualSymbol_FindMoneyInTheWallet()
        {
            var wallet = new List<Money>
            {
                new(300, Currency.PLN),
                new(50, Currency.USD),
                new(200, Currency.CAD),
                new(11, Currency.USD),
                new(20, Currency.PLN)
            };
            var money = new Money(11, Currency.USD);
            var selected = wallet.FirstOrDefault(x => x == money);
            selected.Should().NotBeNull();
        }

        [Fact]
        public void AllowMoneyToBeComparable()
        {
            var wallet = new List<Money>
            {
                new(300, Currency.PLN),
                new(50, Currency.USD),
                new(200, Currency.CAD),
                new(11, Currency.USD),
                new(20, Currency.PLN)
            };
            wallet.Sort();
            wallet.Should().StartWith(new Money[]
            {
                new(200, Currency.CAD),
                new(20, Currency.PLN),
                new(300, Currency.PLN),
                new(11, Currency.USD)
            });
        }
    }

    public record Currency(string Symbol)
    {
        public static Currency CAD => new("CAD");
        public static Currency USD => new("USD");
        public static Currency PLN => new("PLN");
    }
    
    public class Money : ValueObject, IComparable<Money>
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;
        }
        
        public int CompareTo(Money other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var currencyComparison = string.Compare(Currency.Symbol, other.Currency.Symbol, 
                StringComparison.CurrentCultureIgnoreCase);
            return currencyComparison != 0 ? currencyComparison : Amount.CompareTo(other.Amount);
        }
    }
}
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
// ReSharper disable InconsistentNaming

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
    }

    public record Currency(string Symbol)
    {
        public static Currency CAD => new("CAD");
        public static Currency USD => new("USD");
        public static Currency PLN => new("PLN");
    }
    
    public class Money : ValueObject
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
    }
}
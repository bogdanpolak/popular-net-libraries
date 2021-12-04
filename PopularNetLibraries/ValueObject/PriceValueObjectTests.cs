using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PopularNetLibraries.ValueObject
{
    public class PriceValueObjectTests
    {
        [Fact]
        public void CreatePriceValueObject_VerifyProperties()
        {
            var giftCardPrice = new Price(100, "PLN");
            giftCardPrice.Value.Should().Be(100m);
            giftCardPrice.Currency.Should().Be("PLN");
        }

        [Fact]
        public void PricesNotEqual_WhenTwoDifferentCurrencies()
        {
            var giftCardPrice = new Price(100, "PLN");
            var speakerPrice = new Price(100, "GBP");
            giftCardPrice.Should().NotBe(speakerPrice);
        }
    }

    public class Price : ValueObject
    {
        public decimal Value { get; }
        public string Currency { get; }

        public Price(decimal value, string currency)
        {
            Value = value;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }
}
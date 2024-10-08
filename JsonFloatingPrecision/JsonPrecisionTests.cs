using System.Text.Json;
using FluentAssertions;

namespace JsonFloatingPrecision;

public class JsonPrecisionTests
{
    private const string jsonWithoutTrailingZeroes = /*lang=json,strict*/
        """{"Quantity":1}""";
    private const string jsonWithTrailingZeroes = /*lang=json,strict*/
        """{"Quantity":1.0}""";

    [TestCase(jsonWithoutTrailingZeroes)]
    [TestCase(jsonWithTrailingZeroes)]
    public void DecimalPreservesPrecisionInJson(string json)
    {
        var deserialized = JsonSerializer.Deserialize<DecimalQuantity>(json);
        var serialized = JsonSerializer.Serialize(deserialized);

        serialized.Should().Be(json);
    }

    [TestCase(jsonWithoutTrailingZeroes)]
    [TestCase(jsonWithTrailingZeroes)]
    public void DoubleDoesNotPreservePrecisionInJson(string json)
    {
        var deserialized = JsonSerializer.Deserialize<DoubleQuantity>(json);
        var serialized = JsonSerializer.Serialize(deserialized);

        serialized.Should().Be(jsonWithoutTrailingZeroes);
    }

    [Test]
    public void DecimalPreservesPrecision()
    {
        var deserialized = new DecimalQuantity { Quantity = 1.0m };
        var serialized = JsonSerializer.Serialize(deserialized);

        deserialized.Quantity.Scale.Should().Be(1);
        serialized.Should().Be(jsonWithTrailingZeroes);
    }

    [Test]
    public void DoubleDoesNotPreservePrecision()
    {
        var deserialized = new DoubleQuantity { Quantity = 1.0d };
        var serialized = JsonSerializer.Serialize(deserialized);

        serialized.Should().Be(jsonWithoutTrailingZeroes);
    }
}

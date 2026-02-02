namespace MdParser.Tests.OptionsValidation;

public static class GeneralOptionsValidatorTests
{
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("something", true)]
    public static void GeneralOptionsValidator_WithTestCases_ReturnExpected(string? value, bool expected)
    {
        var mockOptions = new Mock<IOptions>();
        mockOptions
            .Setup(p => p.Text)
            .Returns(value!);

        bool actual = GeneralValidation.IsValid(mockOptions.Object);
        Assert.That(actual, Is.EqualTo(expected));
    }
}
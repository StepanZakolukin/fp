using FluentAssertions;
using TagCloud.ImageGeneration.Settings.DTO;

namespace TagCloud.Tests.SettingsTests;

[TestFixture]
public class CompressionRatioDtoTests
{
    private readonly CompressionRatioDto _coefficient = new(5);
    
    [TestCase(-1)]
    [TestCase(11)]
    public void CompressionRatioDto_InvalidValueDuringInitialization_ThrowException(float coefficient)
    {
        var action = () => new CompressionRatioDto(coefficient);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCase(0.1f)]
    [TestCase(10)]
    [TestCase(3)]
    public void CompressionRatioDto_CorrectValueDuringInitialization_ThrowException(float coefficient)
    {
        var action = () => new CompressionRatioDto(coefficient);
        
        action.Should().NotThrow();
    }

    [TestCase(-1)]
    [TestCase(11)]
    public void GetValueOrThrow_IncorrectValue_ThrowException(float coefficient)
    {
        _coefficient.Value = coefficient.ToString();
        var action = () => _coefficient.GetValueOrThrow();
        
        action.Should().Throw<InvalidOperationException>();
    }
    
    [TestCase(0.1f)]
    [TestCase(10)]
    [TestCase(7)]
    public void GetValueOrThrow_CorrectValue_NotThrowException(float coefficient)
    {
        _coefficient.Value = coefficient.ToString();
        var action = () => _coefficient.GetValueOrThrow();
        
        action.Should().NotThrow();
    }
    
    [TestCase(0)]
    [TestCase(-1000f)]
    [TestCase(1000f)]
    [TestCase(10.01f)]
    public void CompressionRatioDto_ChangeToIncorrectValue(float coefficient)
    {
        _coefficient.Value = coefficient.ToString();

        _coefficient.IsCorrect.Should().BeFalse();
        _coefficient.Value.Should().Be(coefficient.ToString());
    }
    
    [TestCase(2)]
    [TestCase(10f)]
    [TestCase(0.1f)]
    public void CompressionRatioDto_ChangeToCorrectValue(float coefficient)
    {
        _coefficient.Value = coefficient.ToString();

        _coefficient.IsCorrect.Should().BeTrue();
        _coefficient.Value.Should().Be(coefficient.ToString());
    }
}
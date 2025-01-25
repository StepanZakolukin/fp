using FluentAssertions;
using TagCloud.ImageGeneration.Settings.DTO;

namespace TagCloud.Tests.SettingsTests;

[TestFixture]
public class FontFamilyDtoTests
{
    private readonly FontFamilyDto _fontFamily = new("Calibri");

    [TestCase("Arial")]
    [TestCase("Calibri")]
    public void FontFamilyDto_CorrectInitialization_NotThrowsException(string fontName)
    {
        var action = () => new FontFamilyDto(fontName);
        
        action.Should().NotThrow();
    }
    
    [Test]
    public void FontFamilyDto_NullDuringInitialization_ThrowException()
    {
        var action = () => new FontFamilyDto(null);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCase("")]
    public void FontFamilyDto_EmptyValueDuringInitialization_ThrowException(string fontName)
    {
        var action = () => new FontFamilyDto(fontName);
        
        action.Should().Throw<ArgumentException>();
    }

    [TestCase("ms")]
    public void FontFamilyDto_NonExistentFontDuringInitialization_ThrowException(string fontName)
    {
        var action = () => new FontFamilyDto(fontName);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCase("ms")]
    public void FontFamilyDto_ChangingToNonExistentFont(string fontName)
    {
        _fontFamily.Name = fontName;
        
        _fontFamily.IsCorrect.Should().BeFalse();
        _fontFamily.Name.Should().Be(fontName);
    }

    [TestCase("Arial")]
    public void FontFamilyDto_ChangingToSystemFont(string fontName)
    {
        _fontFamily.Name = fontName;
        
        _fontFamily.IsCorrect.Should().BeTrue();
        _fontFamily.Name.Should().Be(fontName);
    }

    [TestCase("ms")]
    public void GetValueOrThrow_NonExistentFont_ThrowException(string fontName)
    {
        _fontFamily.Name = fontName;
        var action = () => _fontFamily.GetValueOrThrow();
        
        action.Should().Throw<InvalidOperationException>();
    }

    [TestCase("Arial")]
    [TestCase("Calibri")]
    public void GetValueOrThrow_SystemFont_NotThrowException(string fontName)
    {
        _fontFamily.Name = fontName;
        var action = () => _fontFamily.GetValueOrThrow();
        
        action.Should().NotThrow();
    }
}
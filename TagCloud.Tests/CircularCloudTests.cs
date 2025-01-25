using System.Drawing;
using FluentAssertions;
using FluentAssertions.Extensions;
using TagCloud.ImageGeneration;

namespace TagCloud.Tests;

[TestFixture]
public class CircularCloudTests
{
    private readonly List<RectangleF> _listRectangles = [];

    [Test]
    public void CircularCloud_CorrectInitialization_NoExceptions()
    {
        var createAConstructor = () => new CircularCloud();

        createAConstructor
            .Should()
            .NotThrow();
    }

    [Test]
    public void PutNextRectangle_RandomSizes_MustBeRightSize()
    {
        var random = new Random();
        var placemarker = new CircularCloud();

        for (var i = 0; i < 50; i++)
        {
            var width = random.Next(30, 200);
            var actualSize = new SizeF(width, random.Next(width / 6, width / 3));

            var rectangle = placemarker.PutNextRectangle(actualSize);

            actualSize.Should().Be(rectangle.Size);
        }
    }

    [Test]
    public void PutNextRectangle_RandomSizes_ShouldNotIntersect()
    {
        var random = new Random();
        var placemarker = new CircularCloud();

        for (var i = 0; i < 100; i++)
        {
            var width = random.Next(30, 200);

            var rectangle = placemarker.PutNextRectangle(new SizeF(width, random.Next(width / 6, width / 3)));

            _listRectangles.Any(rect => rect.IntersectsWith(rectangle))
                .Should()
                .BeFalse("Прямоугольники не должны пересекаться");

            _listRectangles.Add(rectangle);
        }
    }

    [Test]
    public void PutNextRectangle_DegenerateRectangle_ShouldNotGoIntoEndlessLoop()
    {
        var call = () =>
        {
            var placemarker = new CircularCloud();
            placemarker.PutNextRectangle(new SizeF(10, 50));
            placemarker.PutNextRectangle(new SizeF(0, 50));
            placemarker.PutNextRectangle(new SizeF(10, 0));
        };
        
        call.ExecutionTime().Should().BeLessThanOrEqualTo(3000.Milliseconds());
    }
}
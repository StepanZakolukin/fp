using System.Drawing;
using FluentAssertions;
using FluentAssertions.Extensions;
using TagCloud.ImageGeneration;

namespace TagCloud.Tests;

[TestFixture]
public class CircularCloudTests
{
    private readonly List<RectangleF> listRectangles = [];

    [Test]
    public void CircularCloud_CorrectInitialization_NoExceptions()
    {
        var createAConstructor = () => new CircularCloud(new Point(960, 540));

        createAConstructor
            .Should()
            .NotThrow();
    }

    [Test]
    public void PutNextRectangle_RandomSizes_MustBeRightSize()
    {
        var random = new Random();
        var placemarker = new CircularCloud(new Point(960, 540));

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
        var placemarker = new CircularCloud(new Point(960, 540));

        for (var i = 0; i < 100; i++)
        {
            var width = random.Next(30, 200);

            var rectangle = placemarker.PutNextRectangle(new SizeF(width, random.Next(width / 6, width / 3)));

            listRectangles.Any(rect => rect.IntersectsWith(rectangle))
                .Should()
                .BeFalse("Прямоугольники не должны пересекаться");

            listRectangles.Add(rectangle);
        }
    }

    [Test]
    public void PutNextRectangle_DegenerateRectangle_ShouldNotGoIntoEndlessLoop()
    {
        var call = () =>
        {
            var placemarker = new CircularCloud(new Point(500, 500));
            placemarker.PutNextRectangle(new SizeF(10, 50));
            placemarker.PutNextRectangle(new SizeF(0, 50));
            placemarker.PutNextRectangle(new SizeF(10, 0));
        };
        
        call.ExecutionTime().Should().BeLessThanOrEqualTo(3000.Milliseconds());
    }
}
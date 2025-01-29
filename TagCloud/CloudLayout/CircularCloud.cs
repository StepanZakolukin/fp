using System.Drawing;

namespace TagCloud.CloudLayout;

public class CircularCloud(Point center) : ILayoutProvider
{
    private const double AngleChangeStep = Math.PI / 180;

    private readonly LinkedList<RectangleF> cloudOfRectangles = [];
    private int DistanceBetweenTurns { get; set; } = 30;
    private int InitialRadiusOfSpiral { get; set; }
    private double AngleOfRotationInRadians { get; set; }
    public string Name => "Круглая форма";

    public Point Center { get; set; } = center;

    public RectangleF PutNextRectangle(SizeF rectangleSize)
    {
        var halfOfMinSide = (int)(Math.Min(rectangleSize.Width, rectangleSize.Height) / 2);
        if (halfOfMinSide > 0)
            DistanceBetweenTurns = Math.Min(DistanceBetweenTurns, halfOfMinSide);

        if (cloudOfRectangles.Count == 0) InitialRadiusOfSpiral = halfOfMinSide;

        var rectangle = ChooseLocationForRectangle(rectangleSize);
        cloudOfRectangles.AddFirst(rectangle);

        return rectangle;
    }

    public void ResetLayout()
    {
        cloudOfRectangles.Clear();
        AngleOfRotationInRadians = 0;
    }

    private RectangleF ChooseLocationForRectangle(SizeF rectangleSize)
    {
        var currentPoint = GetNewPoint();
        var rectangle = GetNewRectangle(currentPoint, rectangleSize);

        while (cloudOfRectangles.Any(rect => rect.IntersectsWith(rectangle)))
        {
            AngleOfRotationInRadians += AngleChangeStep;
            currentPoint = GetNewPoint();
            rectangle = GetNewRectangle(currentPoint, rectangleSize);
        }

        return rectangle;
    }

    private RectangleF GetNewRectangle(PointF centerPoint, SizeF rectangleSize)
    {
        return new RectangleF(new PointF(centerPoint.X - rectangleSize.Width / 2,
            centerPoint.Y - rectangleSize.Height / 2), rectangleSize);
    }

    private PointF GetNewPoint()
    {
        var coefficient = InitialRadiusOfSpiral + AngleOfRotationInRadians * DistanceBetweenTurns;
        var x = (float)(coefficient * Math.Cos(AngleOfRotationInRadians) + Center.X);
        var y = (float)(coefficient * Math.Sin(AngleOfRotationInRadians) + Center.Y);

        return new PointF(x, y);
    }
}
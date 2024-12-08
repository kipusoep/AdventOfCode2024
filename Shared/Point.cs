// Thanks F 😉

namespace Shared;

public record struct Point(int X, int Y)
{
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y);
    }

    public static Point operator *(Point a, int b)
    {
        return new Point(a.X * b, a.Y + b);
    }
}
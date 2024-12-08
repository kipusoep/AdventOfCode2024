// Thanks F 😉

// ReSharper disable MemberCanBePrivate.Global

namespace Shared;

public class Grid<T>(int width, int height)
{
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    public T[] Data { get; set; } = new T[width * height];

    public Grid<T> Clone()
    {
        var grid = new Grid<T>(Width, Height);

        for (var i = 0; i < Data.Length; i++)
        {
            grid.Data[i] = Data[i];
        }

        return grid;
    }

    public void Set(int x, int y, T value)
    {
        Data[IndexOf(x, y)] = value;
    }

    public Point? Find(Func<T, bool> predicate)
    {
        return Where(predicate).FirstOrDefault();
    }

    public ICollection<Point> Where(Func<T, bool> predicate)
    {
        var result = new List<Point>();
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (predicate(Get(x, y)))
                {
                    result.Add(new Point(x, y));
                }
            }
        }

        return result;
    }

    public int IndexOf(int x, int y)
    {
        return x + y * Height;
    }

    public int IndexOf(Point p)
    {
        return IndexOf(p.X, p.Y);
    }

    public void Set(Point p, T value)
    {
        Set(p.X, p.Y, value);
    }

    public bool IsInBounds(Point p)
    {
        return IsInBounds(p.X, p.Y);
    }

    public bool IsInBounds(int x, int y)
    {
        if (x < 0) return false;
        if (x >= Width) return false;
        if (y < 0) return false;
        if (y >= Height) return false;

        return true;
    }

    public T Get(int x, int y)
    {
        return Data[IndexOf(x, y)];
    }

    public T Get(Point p)
    {
        return Data[IndexOf(p)];
    }

    public static Grid<T> Parse(string text, Func<char, T> converter)
    {
        var lines = text
            .Split(Environment.NewLine)
            .Select(x => x.Trim())
            .ToArray();

        var height = lines.Length;
        var width = lines[0].Length;

        var grid = new Grid<T>(width, height);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                grid.Set(x, y, converter(lines[y][x]));
            }
        }

        return grid;
    }
}
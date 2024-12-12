using Shared;
using Point = Shared.Point;

var input = await File.ReadAllTextAsync("input.txt");

var grid = Grid<char>.Parse(input, c => c);
var handledPoints = new List<Point>();
var regions = new List<List<Plot>>();

var region = new List<Plot>();
foreach (var point in grid)
{
    NewFunction(point, region);
    if (region.Any())
    {
        regions.Add(region);
        region = [];
    }
}

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    var price = 0;
    foreach (var region in regions)
    {
        price += region.Count * region.Sum(x => x.Perimeter);
    }

    Console.WriteLine("Part 1 price: {0}", price);
}

void Part2()
{
    var price = 0;
    foreach (var region in regions)
    {
        foreach (var plot in region)
        {
            var point = plot.Point;
            var neighbours = grid.GetNeighbours(point).ToList();
            var sameNeighbours = grid.GetNeighboursByValue(point).ToList();
            var differentNeighbours = neighbours.Except(sameNeighbours).ToList();
            switch (differentNeighbours.Count)
            {
                case 4:
                    plot.Corners += 4;
                    break;
                case 3:
                    plot.Corners += 2;
                    break;
                case 2:
                    if (differentNeighbours[0].X != differentNeighbours[1].X &&
                        differentNeighbours[0].Y != differentNeighbours[1].Y)
                    {
                        plot.Corners += 1;
                    }
                    break;
            }

            var offsets = new[] { new Point(1, 1), new Point(1, -1), new Point(-1, -1), new Point(-1, 1) };
            foreach (var offset in offsets)
            {
                var a = point with { X = point.X + offset.X };
                var b = point with { Y = point.Y + offset.Y };
                if (grid.Get(a) == plot.Plant &&
                    grid.Get(b) == plot.Plant &&
                    grid.Get(new Point(point.X + offset.X, point.Y + offset.Y)) != plot.Plant)
                {
                    plot.Corners += 1;
                }
            }
        }

        price += region.Count * region.Sum(x => x.Corners);
    }

    Console.WriteLine("Part 2 price: {0}", price);
}

void NewFunction(Point point, List<Plot> region)
{
    if (handledPoints.Contains(point))
    {
        return;
    }

    handledPoints.Add(point);
    var value = grid.Get(point)!.Value;
    var plot = new Plot
    {
        Plant = value,
        Point = point,
    };
    var samePlants = grid.GetNeighboursByValue(point).ToList();
    plot.Perimeter = 4 - samePlants.Count;
    region.Add(plot);

    foreach (var samePlant in samePlants.Where(x => !handledPoints.Contains(x)))
    {
        NewFunction(samePlant, region);
    }
}

class Plot
{
    public char Plant { get; set; }
    public int Perimeter { get; set; }
    public Point Point { get; set; }
    public int Corners { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Plant} | {Point}";
    }
}
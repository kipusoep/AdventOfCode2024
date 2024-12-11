using Shared;

var input = await File.ReadAllTextAsync("input.txt");

var grid = Grid<int>.Parse(input, c => c == '.' ? -1 : int.Parse(c.ToString()));

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    var sum = 0;

    var trailheads = grid.Where(x => x == 0);
    foreach (var trailhead in trailheads)
    {
        var trailendings = new HashSet<Point>();
        ResolvePaths([trailhead], trailendings);
        sum += trailendings.Count;
    }

    Console.WriteLine("Part 1 total score: {0}", sum);
}

void Part2()
{
    var sum = 0;

    var trailheads = grid.Where(x => x == 0);
    foreach (var trailhead in trailheads)
    {
        ResolvePathCounts([trailhead], ref sum);
    }

    Console.WriteLine("Part 2 total score: {0}", sum);
}

void ResolvePaths(ICollection<Point> collection, HashSet<Point> trailendings)
{
    foreach (var point in collection)
    {
        var newValue = grid.Get(point) + 1;
        var neighbours = grid.GetNeighboursByValue(point, newValue!.Value);

        if (newValue != 9)
        {
            ResolvePaths(neighbours, trailendings);
        }
        else
        {
            foreach (var neighbour in neighbours)
            {
                trailendings.Add(neighbour);
            }
        }
    }
}

void ResolvePathCounts(ICollection<Point> collection, ref int sum)
{
    foreach (var point in collection)
    {
        var newValue = grid.Get(point) + 1;
        var neighbours = grid.GetNeighboursByValue(point, newValue!.Value);

        if (newValue != 9)
        {
            ResolvePathCounts(neighbours, ref sum);
        }
        else
        {
            sum += neighbours.Count;
        }
    }
}
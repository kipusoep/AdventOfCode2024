using Shared;

var input = await File.ReadAllTextAsync("input.txt");

var grid = Grid<char>.Parse(input, c => c);

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    var antiNodes = new List<Point>();
    var uniqueAntennas = grid.Data.Where(x => x != '.').GroupBy(x => x).Select(x => x.Key).ToList();
    foreach (var uniqueAntenna in uniqueAntennas)
    {
        var antennasOfThisType = grid.Where(x => x == uniqueAntenna).ToList();
        for (var i = 0; i < antennasOfThisType.Count; i++)
        {
            var antenna = antennasOfThisType[i];
            for (var j = 0; j < antennasOfThisType.Count; j++)
            {
                var otherAntenna = antennasOfThisType[j];
                if (antenna == otherAntenna)
                {
                    continue;
                }

                var dx = antenna.X - otherAntenna.X;
                var dy = antenna.Y - otherAntenna.Y;

                var antiNode = new Point(antenna.X + dx, antenna.Y + dy);
                antiNodes.Add(antiNode);
            }
        }
    }

    Console.WriteLine("Part 1 antinodes: {0}", antiNodes.Distinct().Count(x => grid.IsInBounds(x)));
}

void Part2()
{
    var antiNodes = new List<Point>();
    var uniqueAntennas = grid.Data.Where(x => x != '.').GroupBy(x => x).Select(x => x.Key).ToList();
    foreach (var uniqueAntenna in uniqueAntennas)
    {
        var antennasOfThisType = grid.Where(x => x == uniqueAntenna).ToList();
        for (var i = 0; i < antennasOfThisType.Count; i++)
        {
            var antenna = antennasOfThisType[i];
            for (var j = 0; j < antennasOfThisType.Count; j++)
            {
                var otherAntenna = antennasOfThisType[j];
                if (antenna == otherAntenna)
                {
                    continue;
                }

                var count = 0;
                while (true)
                {
                    var dx = (antenna.X - otherAntenna.X) * count;
                    var dy = (antenna.Y - otherAntenna.Y) * count;

                    var antiNode = new Point(antenna.X + dx, antenna.Y + dy);
                    if (!grid.IsInBounds(antiNode))
                    {
                        break;
                    }

                    antiNodes.Add(antiNode);
                    count++;
                }
            }
        }
    }

    Console.WriteLine("Part 2 antinodes: {0}", antiNodes.Distinct().Count());
}
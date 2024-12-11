var input = await File.ReadAllTextAsync("input.txt");

var stones = input.Split(' ').Select(long.Parse).ToList();

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    Console.WriteLine("Part 2 stone count: {0}", GetStoneCount(25));
}

void Part2()
{
    Console.WriteLine("Part 2 stone count: {0}", GetStoneCount(75));
}

long GetStoneCount(int iterations)
{
    var tempStones = stones.ToDictionary(x => x, y => (long)stones.Count(z => z == y));
    for (var i = 1; i <= iterations; i++)
    {
        var newStones = new Dictionary<long, long>();
        foreach (var (stone, count) in tempStones)
        {
            var stonesToAdd = ProcessStone(stone);

            foreach (var stoneToAdd in stonesToAdd)
            {
                if (!newStones.TryAdd(stoneToAdd, count))
                {
                    newStones[stoneToAdd] += count;
                }
            }
        }
        tempStones = newStones;
    }

    return tempStones.Sum(x => x.Value);
}

List<long> ProcessStone(long stone)
{
    var stonesToAdd = new List<long>();
    var engrave = stone.ToString();
    if (stone == 0)
    {
        stonesToAdd.Add(1);
    }
    else if (engrave.Length % 2 == 0)
    {
        stonesToAdd.Add(long.Parse(engrave[..(engrave.Length / 2)]));
        stonesToAdd.Add(long.Parse(engrave[(engrave.Length / 2)..]));
    }
    else
    {
        stonesToAdd.Add(stone * 2024);
    }

    return stonesToAdd;
}
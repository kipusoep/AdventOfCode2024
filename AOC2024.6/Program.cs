using System.Collections.Concurrent;

var input = (await File.ReadAllLinesAsync("input.txt")).ToList();

var blockPositions = new List<(int y, int x)>();
var visited = new ConcurrentBag<(int y, int x)>();
(int y, int x) guardStartingPosition = (0, 0);
var rightBoundary = input[0].Length - 1;
var lowerBoundary = input.Count - 1;

for (var y = 0; y < input.Count; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {
        var character = input[y][x];
        switch (character)
        {
            case '#':
                blockPositions.Add((y, x));
                break;
            case '^':
                guardStartingPosition = (y, x);
                break;
        }
    }
}

visited.Add(guardStartingPosition);

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    var blocks = blockPositions.ToList();

    TryToGetOut(guardStartingPosition, blocks);

    Console.WriteLine("Part 1 visited: {0}", visited.Count);
}

void Part2()
{
    var newBlockOptionsThatWork = new ConcurrentBag<(int y, int x)>();
    var newBlockOptions = visited.ToList();

    newBlockOptions.AsParallel().ForAll(newBlockOption =>
    {
        var blocks = blockPositions.ToList();

        blocks.Add(newBlockOption);

        Console.WriteLine("{0}", newBlockOption);

        var gotOut = TryToGetOut(guardStartingPosition, blocks);
        if (!gotOut)
        {
            newBlockOptionsThatWork.Add(newBlockOption);
        }
    });

    Console.WriteLine("Part 2 block options: {0}", newBlockOptionsThatWork.Count);
}

bool TryToGetOut((int y, int x) guard, List<(int y, int x)> blocks)
{
    var direction = Direction.Up;
    var turningPoints = new List<(int y, int x, Direction dir)>();
    while (true)
    {
        var previousGuardPosition = guard;
        var outOfBounds = Move(ref guard, direction);
        if (turningPoints.Contains((guard.y, guard.x, direction)))
        {
            // Already been here
            return false;
        }

        if (blocks.Contains(guard))
        {
            // Blocked, so move back and turn right
            guard = previousGuardPosition;
            turningPoints.Add((guard.y, guard.x, direction));

            direction = direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }

        if (outOfBounds)
        {
            return true;
        }

        if (!visited.Contains(guard))
        {
            visited.Add(guard);
        }
    }
}

bool Move(ref (int y, int x) guard, Direction direction)
{
    switch (direction)
    {
        case Direction.Up:
            guard.y--;
            if (guard.y < 0)
            {
                return true;
            }

            break;
        case Direction.Right:
            guard.x++;
            if (guard.x > rightBoundary)
            {
                return true;
            }

            break;
        case Direction.Down:
            guard.y++;
            if (guard.y > lowerBoundary)
            {
                return true;
            }

            break;
        case Direction.Left:
            guard.x--;
            if (guard.x < 0)
            {
                return true;
            }

            break;
    }

    return false;
}

internal enum Direction
{
    Up,
    Right,
    Down,
    Left,
}
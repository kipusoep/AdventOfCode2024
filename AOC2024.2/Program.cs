var input = (await File.ReadAllLinesAsync("input.txt")).ToList();

var reports = new List<List<int>>();
foreach (var line in input)
{
    reports.Add(line.Split(' ').Select(int.Parse).ToList());
}

Part1(reports);
Console.WriteLine();
Part2(reports);

void Part1(List<List<int>> reports)
{
    var total = 0;

    foreach (var report in reports)
    {
        if (IsSafe(report))
        {
            total++;
        }
    }

    Console.WriteLine("Part 1 total: {0}", total);
}

void Part2(List<List<int>> reports)
{
    var total = 0;

    foreach (var report in reports)
    {
        if (IsSafe(report))
        {
            total++;
        }
        else
        {
            for (var i = 0; i < report.Count; i++)
            {
                var levels = report.Where((_, index) => index != i).ToList();
                if (IsSafe(levels))
                {
                    total++;
                    break;
                }
            }
        }
    }

    Console.WriteLine("Part 2 total: {0}", total);
}

bool IsSafe(List<int> levels)
{
    var isIncreasing = true;
    var isDecreasing = true;

    for (var i = 1; i < levels.Count; i++)
    {
        if (levels[i] > levels[i - 1])
        {
            isDecreasing = false;
        }
        else if (levels[i] < levels[i - 1])
        {
            isIncreasing = false;
        }
    }

    if (!isIncreasing && !isDecreasing)
    {
        return false;
    }

    var isSafe = true;
    for (var i = 1; i < levels.Count; i++)
    {
        var num1 = levels[i - 1];
        var num2 = levels[i];
        var biggest = Math.Max(num1, num2);
        var smallest = Math.Min(num1, num2);
        var diff = biggest - smallest;

        if (diff is < 1 or > 3)
        {
            isSafe = false;
            break;
        }
    }

    return isSafe;
}
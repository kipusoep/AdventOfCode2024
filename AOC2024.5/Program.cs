var input = (await File.ReadAllLinesAsync("input.txt")).ToList();

var rulesInput = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToList();
var updatesInput = input.Skip(rulesInput.Count + 1).ToList();

var rules = rulesInput.Select(x => x.Split('|').Select(int.Parse).ToList()).Select(x => (x[0], x[1])).ToList();

var rulesComparer = new RulesComparer(rules);

var updates = updatesInput.Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();

var invalidUpdates = updates.Take(0).ToList();

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    var total = 0;

    var validUpdates = updates.ToList();
    foreach (var update in updates)
    {
        for (var i = update.Count - 1; i > 0; i--)
        {
            var pageNumber = update[i];
            var previousNumbers = update.Take(i).ToList();
            var applicableRules = rules.Where(x => x.Item2 == pageNumber && previousNumbers.Contains(x.Item1)).ToList();
            if (applicableRules.Count != previousNumbers.Count)
            {
                validUpdates.Remove(update);
            }
        }
    }

    foreach (var validUpdate in validUpdates)
    {
        total += validUpdate.Skip(validUpdate.Count / 2).First();
    }

    invalidUpdates = updates.Except(validUpdates).ToList();

    Console.WriteLine("Part 1 total: {0}", total);
}

void Part2()
{
    var total = 0;

    var fixedUpdates = invalidUpdates.Take(0).ToList();
    foreach (var invalidUpdate in invalidUpdates)
    {
        fixedUpdates.Add(invalidUpdate.OrderBy(x => x, rulesComparer).ToList());
    }

    total += fixedUpdates.Sum(x => x.Skip(x.Count / 2).First());

    Console.WriteLine("Part 2 total: {0}", total);
}

/// <summary>
/// Compares two page numbers based on the rules.
/// </summary>
public class RulesComparer : IComparer<int>
{
    private readonly List<(int, int)> _rules;

    /// <summary>
    /// Constructor for RulesComparer.
    /// </summary>
    /// <param name="rules">The rules.</param>
    public RulesComparer(List<(int, int)> rules)
    {
        _rules = rules;
    }

    /// <inheritdoc />
    public int Compare(int x, int y)
    {
        if (_rules.Contains((x, y)))
        {
            return -1;
        }

        return 1;
    }
}
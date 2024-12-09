using AOC2024._9;

var input = await File.ReadAllTextAsync("input.txt");

var fs = new FileSystem(input);

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    Console.WriteLine(fs);

    Console.WriteLine("Defragmenting...");

    fs.Defragment(false);

    Console.WriteLine(fs);

    Console.WriteLine("Part 1 checksum: {0}", fs.Checksum);
}

void Part2()
{
    fs.ResetDefragmentation();

    Console.WriteLine(fs);

    Console.WriteLine("Defragmenting...");

    fs.Defragment(true);

    Console.WriteLine(fs);

    Console.WriteLine("Part 2 checksum: {0}", fs.Checksum);
}
namespace AOC2024._9;

internal class FileSystem
{
    private readonly string _diskMap;
    private readonly List<int?> _files = [];
    private int?[] _defragmentedFiles = [];

    public long Checksum => _defragmentedFiles.Select(t => (long?)t).Select((number, i) => i * (number ?? 0)).Sum();

    public FileSystem(string diskMap)
    {
        _diskMap = diskMap;

        var i = 0;
        foreach (var item in _diskMap.Select(x => int.Parse(x.ToString())))
        {
            var isFile = i % 2 == 0;

            int? value = isFile ? i / 2 : null;

            _files.AddRange(Enumerable.Range(0, item).Select(x => value).ToList());

            i++;
        }
    }

    public void ResetDefragmentation()
    {
        _defragmentedFiles = _files.ToArray();
    }

    public void Defragment(bool keepWholeFiles)
    {
        ResetDefragmentation();

        if (!keepWholeFiles)
        {
            for (var i = _defragmentedFiles.Length - 1; i > 0; i--)
            {
                var file = _defragmentedFiles[i];
                var firstSpace = Array.IndexOf(_defragmentedFiles, null);

                var remaining = _defragmentedFiles.Skip(firstSpace).ToList();
                if (remaining.All(x => x == null))
                {
                    break;
                }

                if (firstSpace > -1)
                {
                    _defragmentedFiles[firstSpace] = file;
                    _defragmentedFiles[i] = null;
                }
            }
        }
        else
        {
            var partitioned = new List<List<int?>>();
            var currentChunk = new List<int?>
            {
                _defragmentedFiles[0],
            };
            for (var i = 1; i < _defragmentedFiles.Length; i++)
            {
                if (_defragmentedFiles[i] == currentChunk.Last())
                {
                    currentChunk.Add(_defragmentedFiles[i]);
                }
                else
                {
                    partitioned.Add(currentChunk);
                    currentChunk = [_defragmentedFiles[i]];
                }
            }
            partitioned.Add(currentChunk);

            for (var i = partitioned.Count - 1; i >= 0; i--)
            {
                var partition = partitioned[i];
                if (partition[0] == null)
                {
                    continue;
                }

                var indexOfPartition = partitioned[..i].Sum(x => x.Count);
                var firstFreeSpot = FindFirstOccurrence(_defragmentedFiles, Enumerable.Range(0, partition.Count).Select(x => (int?)null).ToList());
                if (firstFreeSpot > -1 && firstFreeSpot < indexOfPartition)
                {
                    var spot = _defragmentedFiles.AsSpan(firstFreeSpot, partition.Count);
                    partition.CopyTo(spot);
                    var original = _defragmentedFiles.AsSpan(indexOfPartition, partition.Count);
                    original.Fill(null);
                }
            }
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var files = new List<string>();
        var collection = _defragmentedFiles.Length > 0 ? _defragmentedFiles.ToList() : _files;
        foreach (var file in collection)
        {
            files.Add(file == null ? "." : file.ToString()!);
        }

        return string.Join(string.Empty, files);
    }

    private static int FindFirstOccurrence(int?[] list, List<int?> range)
    {
        for (var i = 0; i <= list.Length - range.Count; i++)
        {
            if (list.Skip(i).Take(range.Count).SequenceEqual(range))
            {
                return i;
            }
        }
        return -1;
    }
}

namespace AdventOfCode_AlgoLib.Utils;

internal class Generation
{
    static IEnumerable<T[]> Permutations<T>(T[] items, int k)
    {
        var used = new bool[items.Length];
        var buffer = new T[k];

        return Permute(0);

        IEnumerable<T[]> Permute(int depth)
        {
            if (depth == k)
            {
                yield return buffer.ToArray();
                yield break;
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (used[i]) continue;

                used[i] = true;
                buffer[depth] = items[i];

                foreach (var p in Permute(depth + 1))
                    yield return p;

                used[i] = false;
            }
        }
    }
}

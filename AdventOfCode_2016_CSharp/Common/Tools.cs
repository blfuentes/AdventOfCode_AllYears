using System.Numerics;

namespace AdventOfCode_2016_CSharp.Common;

internal static class Tools
{
    public static string ConcatStrNumber(string str, int n)
    {
        var pool = System.Buffers.ArrayPool<char>.Shared;
        char[] rented = pool.Rent(64); // choose a size that fits most cases
        try
        {
            Span<char> buf = rented;
            int pos = 0;

            str.AsSpan().CopyTo(buf);
            pos += str.Length;

            if (!n.TryFormat(buf[pos..], out int written))
                throw new InvalidOperationException("Formatting failed");
            pos += written;

            // Create final string (copies the used portion)
            string result = new(rented, 0, pos);
            return result;
        }
        finally
        {
            pool.Return(rented); // or pool.Return(rented, clearArray: true) for secrets
        }
    }

    public static string ToHexLower(this byte[] bytes)
    {
        const string HexDigitsLower = "0123456789abcdef";
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length == 0) return string.Empty;

        return string.Create(bytes.Length * 2, bytes, (span, src) =>
        {
            for (int i = 0; i < src.Length; i++)
            {
                byte b = src[i];
                span[i * 2] = HexDigitsLower[b >> 4];
                span[i * 2 + 1] = HexDigitsLower[b & 0xF];
            }
        });
    }

    public static string ToBinary(uint value)
    {
        if (value == 0) return "0";

        int len = 32 - BitOperations.LeadingZeroCount(value);
        Span<char> chars = stackalloc char[len];

        for (int i = len - 1; i >= 0; i--)
        {
            chars[i] = (char)('0' + (value & 1));
            value >>= 1;
        }

        return new string(chars);
    }

    public static string ToBinary32(int value)
    {
        uint u = (uint)value;
        Span<char> chars = stackalloc char[32];

        for (int i = 31; i >= 0; i--)
        {
            chars[i] = (char)('0' + (u & 1));
            u >>= 1;
        }

        return new string(chars);
    }
}

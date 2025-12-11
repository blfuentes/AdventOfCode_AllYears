namespace AdventOfCode_2015_CSharp;

internal static class Utils
{
    public static bool DownloadContent(HttpClient httpClient, int year, int day)
    {
        var toFechDate = new DateTime(year, 12, day);

        if (toFechDate > DateTime.UtcNow)
        {
            Console.WriteLine($"Skipping day {day} year {year} input download.");
            return false;
        }

        string url = $"https://adventofcode.com/{year}/day/{day}/input";
        string sessionPath = Path.Combine(AppContext.BaseDirectory, @$"..\..\..\session.txt");
        string inputPath = Path.Combine(AppContext.BaseDirectory, @$"..\..\..\day{day:D2}/day{day:D2}.txt");
        string content = "";
        if (File.Exists(inputPath))
        {
            content = File.ReadAllText(inputPath);
        }
        else
        {
            File.Create(inputPath).Close();
        }
        if (string.IsNullOrEmpty(content) && File.Exists(sessionPath))
        {
            var sessionKey = File.ReadAllText(sessionPath);
            if (!string.IsNullOrWhiteSpace(sessionKey))
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Cookie", $"session={sessionKey}");

                content = httpClient.GetStringAsync(url).Result;
                File.WriteAllText(inputPath, content);
            }
        }

        return true;
    }

    public static string FormatTime(long ticks)
    {
        long microseconds = ticks / 10;
        long seconds = microseconds / 1_000_000;
        long minutes = seconds / 60;

        return $"{minutes:D2}:{seconds:D2}.{microseconds:D6}";
    }

    public static string Repeat(this string text, uint n)
    {
        var textAsSpan = text.AsSpan();
        var span = new Span<char>(new char[textAsSpan.Length * (int)n]);
        for (var i = 0; i < n; i++)
        {
            textAsSpan.CopyTo(span.Slice((int)i * textAsSpan.Length, textAsSpan.Length));
        }

        return span.ToString();
    }
}

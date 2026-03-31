namespace AdventOfCode_2016_CSharp.Common;

public static class DirectionExtensions
{
    public static GridPos ToOffset(this Direction d) =>
        d.Orientation switch
        {
            Dir.Up => (-1, 0),
            Dir.Down => (1, 0),
            Dir.Left => (0, -1),
            Dir.Right => (0, 1),
            _ => (0, 0)
        };
}


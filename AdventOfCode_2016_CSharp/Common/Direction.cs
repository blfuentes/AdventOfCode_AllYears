namespace AdventOfCode_2016_CSharp.Common;

public enum Dir { Up, Down, Left, Right }

public class Direction(Dir orientation)
{
    public Direction(char d) : this(d switch
    {
        'U' => Dir.Up,
        'D' => Dir.Down,
        'L' => Dir.Left,
        'R' => Dir.Right,
        _ => throw new NotImplementedException()
    }) { }

    public Dir Orientation { get; } = orientation;
}

namespace AdventOfCode_2016_CSharp.Common;

public readonly struct GridPos(int row, int col)
{
    public int Row { get; } = row;
    public int Col { get; } = col;

    public static implicit operator GridPos((int row, int col) t)
        => new(t.row, t.col);

    public static implicit operator (int row, int col)(GridPos p)
        => (p.Row, p.Col);

    public static GridPos operator +(GridPos a, GridPos b)
        => new(a.Row + b.Row, a.Col + b.Col);

    public static GridPos operator +(GridPos p, Direction d)
        => p + d.ToOffset();

    public static void Normalize(ref GridPos p)
    {
        GridPos tmp = p switch
        {
            { Row: var x, Col: var y } when x < -1 => new(-1, y),
            { Row: var x, Col: var y } when x > 1 => new(1, y),
            { Row: var x, Col: var y } when y < -1 => new(x, -1),
            { Row: var x, Col: var y } when y > 1 => new(x, 1),
            { Row: var x, Col: var y } => new(x, y)
        };
        p = tmp;
    }
}

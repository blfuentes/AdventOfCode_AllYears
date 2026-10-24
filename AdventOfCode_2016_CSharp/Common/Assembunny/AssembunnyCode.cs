namespace AdventOfCode_2016_CSharp.Common.AssembunnyCode;

public sealed class Registers
{
    public Dictionary<char, int> Register { get; set; } = [];
    public int Get(char r) => Register[r];
    public void Set(char r, int value) => _ = Register.TryAdd(r, value);
}

public record RegisterValue
{
    public RegisterValue(string value)
    {
        if (int.TryParse(value, out int v))
        {
            Value = v;
        }
        else
        {
            Registry = value[0];
        }
    }

    public int? Value;
    public char Registry;
}

public interface IOperation
{
    int Execute(Registers registers);

    IOperation Toggle();

    string Display(Registers registers);
}

public interface IToggle
{
    int Toggle { get; set; }
}

public record CopyOp(RegisterValue Source, char Target) : IOperation
{
    public int Execute(Registers registers)
    {
        registers.Register[Target] = Source.Value ?? registers.Register[Source.Registry];
        return 1;
    }

    public IOperation Toggle()
    {
        return new JnzOp(Source, new RegisterValue(Target.ToString()));
    }

    public string Display(Registers registers)
    {
        return $"cpy {Source.Value ?? registers.Register[Source.Registry]} {Target}";
    }
}

public record IncOp(char Source) : IOperation
{
    public string Display(Registers registers)
    {
        return $"inc {Source}";
    }

    public int Execute(Registers registers)
    {
        registers.Register[Source]++;
        return 1;
    }

    public IOperation Toggle()
    {
        return new DecOp(Source);
    }
}

public record DecOp(char Source) : IOperation
{
    public string Display(Registers registers)
    {
        return $"dec {Source}";
    }

    public int Execute(Registers registers)
    {
        registers.Register[Source]--;
        return 1;
    }

    public IOperation Toggle()
    {
        return new IncOp(Source);
    }
}

public record JnzOp(RegisterValue Source, RegisterValue OffSet) : IOperation
{
    public string Display(Registers registers)
    {
        return $"jnz {Source.Value ?? registers.Register[Source.Registry]} {OffSet.Value ?? registers.Register[OffSet.Registry]}";
    }

    public int Execute(Registers registers)
    {
        return (Source.Value ?? registers.Register[Source.Registry]) != 0 ?
            OffSet.Value ?? registers.Register[OffSet.Registry] :
            1;
    }

    public IOperation Toggle()
    {
        return new CopyOp(Source, OffSet.Registry);
    }
}

public record TglOp(char Source) : IOperation, IToggle
{
    public int Toggle { get; set; }

    public string Display(Registers registers)
    {
        return $"tgl {Source}";
    }

    public int Execute(Registers registers)
    {
        Toggle = registers.Register[Source];
        return 1;
    }

    IOperation IOperation.Toggle()
    {
        return new IncOp(Source);
    }
}
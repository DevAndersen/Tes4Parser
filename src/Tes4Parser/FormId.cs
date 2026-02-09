namespace Tes4Parser;

public struct FormId
{
    public uint Value { get; set; }

    public FormId(uint value)
    {
        Value = value;
    }

    public readonly override string ToString()
    {
        return Convert.ToString(Value, 16).PadLeft(8, '0');
    }
}

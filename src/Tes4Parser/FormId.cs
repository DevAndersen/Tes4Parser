namespace Tes4Parser;

public struct FormId
{
    public uint Value { get; set; }

    public FormId(uint value)
    {
        Value = value;
    }

    public static explicit operator FormId(uint value) => new FormId(value);

    public static explicit operator uint(FormId formId) => formId.Value;

    public readonly override string ToString()
    {
        return Convert.ToString(Value, 16).PadLeft(8, '0');
    }
}

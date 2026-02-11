using System.Runtime.InteropServices;

namespace Tes4Parser.Records;

[StructLayout(LayoutKind.Explicit)]
public struct ConditionDataField // Todo: Move this somewhere else?
{
    [field: FieldOffset(0)]
    public byte Operator { get; set; } // Todo: Combined normal- and flag enum.

    [field: FieldOffset(1)]
    public byte Unused1 { get; set; }

    [field: FieldOffset(2)]
    public byte Unused2 { get; set; }

    [field: FieldOffset(3)]
    public byte Unused3 { get; set; }

    [field: FieldOffset(4)]
    public float ComparisonValueFloat { get; set; }

    [field: FieldOffset(4)]
    public FormId ComparisonValueFormId { get; set; }

    [field: FieldOffset(8)]
    public ushort FunctionIndex { get; set; }

    [field: FieldOffset(10)]
    public ConditionDataFieldData Data { get; set; }

    [field: FieldOffset(10)]
    public ConditionDataFieldGetEventData GetEventData { get; set; }

    [field: FieldOffset(18)]
    public uint RunOnType { get; set; }

    [field: FieldOffset(22)]
    public FormId Reference { get; set; }

    [field: FieldOffset(26)]
    public int Index { get; set; }

    [StructLayout(LayoutKind.Explicit)]
    public struct ConditionDataFieldData
    {
        [field: FieldOffset(0)]
        public int Param1Int { get; set; }

        [field: FieldOffset(0)]
        public FormId Param1FormId { get; set; }

        [field: FieldOffset(4)]
        public int Param2Int { get; set; }

        [field: FieldOffset(4)]
        public FormId Param2FormId { get; set; }
    }

    public struct ConditionDataFieldGetEventData
    {
        public ushort EventFunction { get; set; }

        public char EventMemberChar1 { get; set; } // Todo

        public char EventMemberChar2 { get; set; } // Todo
    }
}

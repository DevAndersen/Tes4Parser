namespace Tes4Parser.Records;

public struct RecordMetadata : IReadWrite<RecordMetadata>
{
    public uint Size { get; set; }

    public uint Flags { get; set; }

    public FormId RecordFormIdentifier { get; set; }

    public ushort Timestamp { get; set; }

    public ushort VersionControlInfo { get; set; }

    public ushort InternalRecordVersion { get; set; }

    public ushort Unknown { get; set; }

    public static RecordMetadata Read(Tes4Reader reader)
    {
        return new RecordMetadata
        {
            Size = reader.ReadU32Value(),
            Flags = reader.ReadU32Value(),
            RecordFormIdentifier = reader.ReadFormIdValue(),
            Timestamp = reader.ReadU16Value(),
            VersionControlInfo = reader.ReadU16Value(),
            InternalRecordVersion = reader.ReadU16Value(),
            Unknown = reader.ReadU16Value()
        };
    }

    public readonly void Write(Tes4Writer writer)
    {
        writer.WriteU32Value(Size);
        writer.WriteU32Value(Flags);
        writer.WriteFormIdValue(RecordFormIdentifier);
        writer.WriteU16Value(Timestamp);
        writer.WriteU16Value(VersionControlInfo);
        writer.WriteU16Value(InternalRecordVersion);
        writer.WriteU16Value(Unknown);
    }
}

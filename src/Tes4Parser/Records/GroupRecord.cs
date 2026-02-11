namespace Tes4Parser.Records;

public class GroupRecord : Record, IReadWrite<GroupRecord>
{
    public const string TypeString = "GRUP";

    public required Record[] Records { get; set; }

    public static GroupRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);
        Record[] records = reader.ReadRecords(false, metadata.DataSize).ToArray();

        return new GroupRecord
        {
            Metadata = metadata,
            Records = records
        };
    }

    public override void Write(Tes4Writer writer)
    {
        writer.WriteTypeString(TypeString);
        Metadata.Write(writer);

        foreach (Record record in Records)
        {
            record.Write(writer);
        }
    }
}

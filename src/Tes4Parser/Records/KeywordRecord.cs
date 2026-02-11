namespace Tes4Parser.Records;

public class KeywordRecord : Record, IReadWrite<KeywordRecord>
{
    public const string TypeString = "KYWD";

    public required string EditorID { get; set; }

    public required uint? Color { get; set; }

    public static KeywordRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string editorId = reader.ReadUtf8NullTerminated("EDID");
        uint? color = reader.ReadU32("CNAM");

        return new KeywordRecord
        {
            Metadata = metadata,

            EditorID = editorId,
            Color = color
        };
    }

    public override void Write(Tes4Writer writer)
    {
        writer.WriteTypeString(TypeString);
        Metadata.Write(writer);

        writer.WriteUtf8NullTerminated("EDID", EditorID);
        writer.WriteU32("CNAM", Color);
    }
}

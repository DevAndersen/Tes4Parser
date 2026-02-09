namespace Tes4Parser.Records;

public abstract class Record
{
    public RecordMetadata Metadata { get; set; }
}

public class Tes4Record : Record, IReadWrite<Tes4Record>
{
    public const string TypeString = "TES4";

    public required HeaderStruct Header { get; set; }

    public required string? Author { get; set; }

    public required string? Description { get; set; }

    public required MasterStruct[] Masters { get; set; }

    public required FormId[] OverriddenFormIds { get; set; }

    public required uint NumberOfTagifiableStrings { get; set; }

    public required uint? UnknownCounter { get; set; }

    public static Tes4Record Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        throw new NotImplementedException();
    }

    public static void Write(Tes4Writer writer, Tes4Record record)
    {
        throw new NotImplementedException();
    }

    public struct HeaderStruct
    {
        public required float Version { get; set; }

        public required uint RecordAndGroupCount { get; set; }

        public required uint NextAvailableObjectId { get; set; }
    }

    public struct MasterStruct
    {
        public required string FileName { get; set; }

        public required ulong FileSize { get; set; }
    }
}

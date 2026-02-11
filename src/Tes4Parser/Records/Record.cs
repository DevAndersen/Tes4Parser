namespace Tes4Parser.Records;

public abstract class Record : IWrite
{
    public RecordMetadata Metadata { get; set; }

    public abstract void Write(Tes4Writer writer);
}

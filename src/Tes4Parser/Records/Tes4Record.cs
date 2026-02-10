namespace Tes4Parser.Records;

public class Tes4Record : Record, IReadWrite<Tes4Record>
{
    public const string TypeString = "TES4";

    public required HeaderStruct Header { get; set; }

    public required string? Author { get; set; }

    public required string? Description { get; set; }

    public required MasterStruct[] Masters { get; set; }

    public required FormId[] OverriddenFormIds { get; set; }

    public required uint? NumberOfTagifiableStrings { get; set; }

    public required uint? UnknownCounter { get; set; }

    public static Tes4Record Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);
        HeaderStruct header = reader.ReadStruct<HeaderStruct>("HEDR");

        string? author = reader.ReadUtf8NullTerminatedOptional("CNAM");
        string? description = reader.ReadUtf8NullTerminatedOptional("SNAM");

        MasterStruct[] masters = ReadMasters(reader).ToArray();

        FormId[] overriddenFormIds = reader.ReadFormListOptional("ONAM");

        uint? numberOfTagifiableStrings = reader.ReadU32Optional("INTV");
        uint? unknownCounter = reader.ReadU32Optional("INCC");

        return new Tes4Record
        {
            Metadata = metadata,

            Header = header,
            Author = author,
            Description = description,
            Masters = masters,
            OverriddenFormIds = overriddenFormIds,
            NumberOfTagifiableStrings = numberOfTagifiableStrings,
            UnknownCounter = unknownCounter,
        };
    }

    public void Write(Tes4Writer writer)
    {
        writer.WriteTypeString(TypeString);
        Metadata.Write(writer);
        writer.WriteStruct<HeaderStruct>("HEDR", Header);
        writer.WriteUtf8NullTerminated("CNAM", Author);
        writer.WriteUtf8NullTerminated("SNAM", Description);
    }

    private static IEnumerable<MasterStruct> ReadMasters(Tes4Reader reader)
    {
        while (reader.PeekTypeString("MAST"))
        {
            reader.ReadTypeString("MAST");
            string fileName = reader.ReadUtf8NullTerminatedValue();
            ulong fileSize = reader.ReadU64("DATA");

            yield return new MasterStruct
            {
                FileName = fileName,
                FileSize = fileSize
            };
        }
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

namespace Tes4Parser.Records;

public class TextureSetRecord : Record, IReadWrite<TextureSetRecord>
{
    public const string TypeString = "TXST";

    public required string EditorID { get; set; }

    public required ObjectBoundaryStruct ObjectBoundary { get; set; }

    public required string? Texture00 { get; set; }

    public required string? Texture01 { get; set; }

    public required string? Texture02 { get; set; }

    public required string? Texture03 { get; set; }

    public required string? Texture04 { get; set; }

    public required string? Texture05 { get; set; }

    public required string? Texture06 { get; set; }

    public required string? Texture07 { get; set; }

    public required TextureSetDecalData? DecalData { get; set; }

    public required TextureSetRecordFlags Flags { get; set; }

    public struct TextureSetDecalData
    {
        public float MinWidth { get; set; }

        public float MaxWidth { get; set; }

        public float MinHeight { get; set; }

        public float MaxHeight { get; set; }

        public float Depth { get; set; }

        public float Shininess { get; set; }

        public float ParallaxScale { get; set; }

        public byte ParallaxPasses { get; set; }

        public TextureSetDecalDataFlags Flags { get; set; }
    }

    [Flags]
    public enum TextureSetDecalDataFlags : byte
    {
        None = 0,
        Parallax = 0x01,
        AlphaBlending = 0x02,
        AlphaTesting = 0x04,
        Not4Subtextures = 0x08,
    }

    [Flags]
    public enum TextureSetRecordFlags : ushort
    {
        None = 0,
        NotHasSpecularMap = 0x01,
        FacegenTextures = 0x02,
        HasModelSpaceNormalMap = 0x04,
    }

    public static TextureSetRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string editorId = reader.ReadUtf8NullTerminated("EDID");

        ObjectBoundaryStruct objectBoundary = reader.ReadStruct<ObjectBoundaryStruct>("OBND");

        string? texture00 = reader.ReadUtf8NullTerminatedOptional("TX00");
        string? texture01 = reader.ReadUtf8NullTerminatedOptional("TX01");
        string? texture02 = reader.ReadUtf8NullTerminatedOptional("TX02");
        string? texture03 = reader.ReadUtf8NullTerminatedOptional("TX03");
        string? texture04 = reader.ReadUtf8NullTerminatedOptional("TX04");
        string? texture05 = reader.ReadUtf8NullTerminatedOptional("TX05");
        string? texture06 = reader.ReadUtf8NullTerminatedOptional("TX06");
        string? texture07 = reader.ReadUtf8NullTerminatedOptional("TX07");

        TextureSetDecalData? decalData = reader.ReadStructOptional<TextureSetDecalData>("DODT");

        TextureSetRecordFlags flags = (TextureSetRecordFlags)reader.ReadU16("DNAM");

        return new TextureSetRecord
        {
            Metadata = metadata,

            EditorID = editorId,
            ObjectBoundary = objectBoundary,
            Texture00 = texture00,
            Texture01 = texture01,
            Texture02 = texture02,
            Texture03 = texture03,
            Texture04 = texture04,
            Texture05 = texture05,
            Texture06 = texture06,
            Texture07 = texture07,
            DecalData = decalData,
            Flags = flags
        };
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

using System.Text;

namespace Tes4Parser.Records;

public class DialogTopicRecord : Record, IReadWrite<DialogTopicRecord>
{
    public const string TypeString = "DIAL";

    public required string? EditorID { get; set; }

    public required DialogTopicData Data { get; set; }

    public required string Subtype { get; set; }

    public required uint InfoCount { get; set; }

    public struct DialogTopicData
    {
        public bool Unknown { get; set; }

        public byte DialogueTab { get; set; }

        public byte SubtypeID { get; set; }

        public byte Unused { get; set; }
    }

    public static DialogTopicRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string? editorId = reader.ReadUtf8NullTerminatedOptional("EDID");

        string? playerDialogue = reader.ReadUtf8NullTerminatedOptional("FULL"); // Todo: Localized string, might need special handling.

        float priority = reader.ReadF32("PNAM");
        FormId? owningBranch = reader.ReadFormIdOptional("BNAM");
        FormId owningQuest = reader.ReadFormId("QNAM");

        DialogTopicData data = reader.ReadStruct<DialogTopicData>("DATA");

        reader.ReadTypeString("SNAM");
        const int subtypeLength = 4;

        ushort size = reader.ReadU16Value();
        if (size != subtypeLength)
        {
            throw new InvalidDataException("DIAL SNAM string was not four bytes in length");
        }

        string subtype = reader.ReadUtf8Value(subtypeLength);

        uint infoCount = reader.ReadU32("TIFC");

        return new DialogTopicRecord
        {
            Metadata = metadata,
            EditorID = editorId,
            Data = data,
            Subtype = subtype,
            InfoCount = infoCount
        };
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

namespace Tes4Parser.Records;

public class DialogTopicInfoRecord : Record, IReadWrite<DialogTopicInfoRecord>
{
    public const string TypeString = "INFO";

    public required string? EditorID { get; set; }

    public required DataStruct? Data { get; set; }

    public required EnamStruct? Enam { get; set; }

    public required FormId? PreviousInfo { get; set; }

    public required byte? FavorLevel { get; set; }

    public required FormId[] TopicLinks { get; set; }

    public required FormId? SharedInfo { get; set; }

    public struct DataStruct
    {
        public DialogueTab Tab { get; set; }

        public DataFlags Flags { get; set; }

        public float ResetTime { get; set; }
    }

    public struct EnamStruct
    {
        public DataFlags Flags { get; set; }

        public ushort ResetTime { get; set; }
    }

    public enum DialogueTab : ushort
    {
        PlayerDialogue = 0,
        FavorDialogue = 1,
        Scenes = 2,
        Combat = 3,
        Favors = 4,
        Detection = 5,
        Service = 6,
        Misc = 7
    }

    [Flags]
    public enum DataFlags : ushort
    {
        Goodbye = 0b_0000_0000_0000_0001,
        Random = 0b_0000_0000_0000_0010,
        SayOnce = 0b_0000_0000_0000_0100,
        OnActivation = 0b_0000_0000_0000_1000,
        RandomEnd = 0b_0000_0000_0001_0000,
        InvisibleContinue = 0b_0000_0000_0010_0000,
        WalkAway = 0b_0000_0000_0100_0000,
        WalkAwayInvisibleInMenu = 0b_0000_0000_1000_0000,
        ForceSubtitle = 0b_0000_0001_0000_0000,
        CanMoveWhileGreeting = 0b_0000_0010_0000_0000,
        HasNoLipFile = 0b_0000_0100_0000_0000,
        RequiresPostProcessing = 0b_0000_1000_0000_0000,
        HasAudioOutputOverride = 0b_0001_0000_0000_0000,
        SpendsFavorPoints = 0b_0010_0000_0000_0000,
    }

    public static DialogTopicInfoRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string? editorId = reader.ReadUtf8NullTerminatedOptional("EDID");

        // TODO: VMAD

        DataStruct? data = null;
        if (reader.PeekTypeString("DATA"))
        {
            data = reader.ReadStruct<DataStruct>("DATA");
        }

        EnamStruct? enam = null;
        if (reader.PeekTypeString("ENAM"))
        {
            enam = reader.ReadStruct<EnamStruct>("ENAM");
        }

        FormId? previousInfo = reader.ReadFormIdOptional("PNAM");
        byte? favorLevel = reader.ReadU8Optional("CNAM");

        FormId[] topicLinks = reader.ReadFormListOptional("TCLT");
        FormId? sharedInfo = reader.ReadFormIdOptional("DNAM");

        return new DialogTopicInfoRecord
        {
            Metadata = metadata,
            EditorID = editorId,
            Data = data,
            Enam = enam,
            PreviousInfo = previousInfo,
            FavorLevel = favorLevel,
            TopicLinks = topicLinks,
            SharedInfo = sharedInfo
        };
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

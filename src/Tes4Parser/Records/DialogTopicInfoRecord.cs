namespace Tes4Parser.Records;

public class DialogTopicInfoRecord : Record, IReadWrite<DialogTopicInfoRecord>
{
    public const string TypeString = "INFO";

    public required string? EditorID { get; set; }

    public required VmadStruct? ScriptInfo { get; set; }

    public required DataStruct? Data { get; set; }

    public required EnamStruct? Enam { get; set; }

    public required FormId? PreviousInfo { get; set; }

    public required byte? FavorLevel { get; set; }

    public required FormId[] TopicLinks { get; set; }

    public required FormId? SharedInfo { get; set; }

    public required DialogTopicResponse[] Responses { get; set; }

    public required ConditionDataField[] Conditions { get; set; }

    public struct DataStruct
    {
        public DialogueTab Tab { get; set; }

        public DataFlags Flags { get; set; }

        public float ResetTime { get; set; }
    }

    public struct EnamStruct // Todo: Named after TypeString, find a more meaningful name.
    {
        public DataFlags Flags { get; set; }

        public ushort ResetTime { get; set; }
    }

    public class DialogTopicResponse : IReadWrite<DialogTopicResponse>
    {
        public required EmotionStruct Emotion { get; set; }

        public required string Text { get; set; }

        public required string Notes { get; set; }

        public required string Edits { get; set; }

        public required FormId? SpeakerIdleAnims { get; set; }

        public required FormId? ListenerIdleAnims { get; set; }

        public static DialogTopicResponse Read(Tes4Reader reader)
        {
            EmotionStruct emotion = reader.ReadStruct<EmotionStruct>("TRDT");
            string text = reader.ReadUtf8NullTerminated("NAM1");
            string notes = reader.ReadUtf8NullTerminated("NAM2");
            string edits = reader.ReadUtf8NullTerminated("NAM3");
            FormId? speakerIdleAnims = reader.ReadFormIdOptional("SNAM");
            FormId? listenerIdleAnims = reader.ReadFormIdOptional("LNAM");

            return new DialogTopicResponse
            {
                Emotion = emotion,
                Text = text,
                Notes = notes,
                Edits = edits,
                SpeakerIdleAnims = speakerIdleAnims,
                ListenerIdleAnims = listenerIdleAnims
            };
        }

        public void Write(Tes4Writer writer)
        {
            throw new NotImplementedException();
        }
    }

    public struct EmotionStruct
    {
        public Emotion Emotion { get; set; }

        public uint EmotionValue { get; set; }

        public int Unknown1 { get; set; }

        public byte Id { get; set; }

        public byte Unknown2 { get; set; } // Todo: Supposedly unused, if this is just padding from 4-byte alignment it can probably be removed.

        public byte Unknown3 { get; set; } // Todo: Supposedly unused, if this is just padding from 4-byte alignment it can probably be removed.

        public byte Unknown4 { get; set; } // Todo: Supposedly unused, if this is just padding from 4-byte alignment it can probably be removed.

        public FormId SoundFile { get; set; }

        public byte UseEmotionAnimation { get; set; } // Todo: Should this be a bool?

        public byte Unknown5 { get; set; } // Todo: Supposedly unused, if this is just padding from 4-byte alignment it can probably be removed.

        public byte Unknown6 { get; set; } // Todo: Supposedly unused, if this is just padding from 4-byte alignment it can probably be removed.

        public byte Unknown7 { get; set; } // Todo: Supposedly unused, if this is just padding from 4-byte alignment it can probably be removed.
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

    public enum Emotion : uint
    {
        Neutral = 0,
        Anger = 1,
        Disgust = 2,
        Fear = 3,
        Sad = 4,
        Happy = 5,
        Surprise = 6,
        Puzzled = 7
    }

    public static DialogTopicInfoRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string? editorId = reader.ReadUtf8NullTerminatedOptional("EDID");

        VmadStruct? scriptInfo = null;
        if (reader.PeekTypeString(VmadStruct.TypeString))
        {
            reader.ReadTypeString(VmadStruct.TypeString);
            ushort size = reader.ReadU16Value();
            scriptInfo = VmadStruct.Read(reader, 1);
        }

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

        List<FormId> topicLinks = [];
        while (reader.PeekTypeString("TCLT"))
        {
            topicLinks.Add(reader.ReadFormId("TCLT"));
        }

        FormId? sharedInfo = reader.ReadFormIdOptional("DNAM");

        List<DialogTopicResponse> responses = [];
        while (reader.PeekTypeString("TRDT"))
        {
            responses.Add(DialogTopicResponse.Read(reader));
        }

        ConditionDataField[] conditions = reader.ReadMultipleFields<ConditionDataField>("CTDA").ToArray();

        return new DialogTopicInfoRecord
        {
            Metadata = metadata,
            EditorID = editorId,
            ScriptInfo = scriptInfo,
            Data = data,
            Enam = enam,
            PreviousInfo = previousInfo,
            FavorLevel = favorLevel,
            TopicLinks = topicLinks.ToArray(),
            SharedInfo = sharedInfo,
            Responses = responses.ToArray(),
            Conditions = conditions
        };
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }

    public static int MyProperty { get; set; }
}

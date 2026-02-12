namespace Tes4Parser.Records;

public class MagicEffectRecord : Record, IReadWrite<MagicEffectRecord>
{
    public const string TypeString = "MGEF";

    public required string EditorID { get; set; }

    public required VmadStruct? Vmad { get; set; }

    public required string? Name { get; set; }

    public required FormId? Static { get; set; }

    public required uint? KeywordCount { get; set; }

    public required FormId[] Keywords { get; set; }

    public required MagicEffectData Data { get; set; }

    public required FormId[] CounterEffects { get; set; }

    public required MagicEffectSoundData[] SoundData { get; set; }

    public required string Description { get; set; }

    public required ConditionDataField[] Conditions { get; set; }

    public struct MagicEffectData
    {
        public required MagicEffectDataFlags Flags { get; set; }

        public required float BaseCost { get; set; }

        public required FormId RelatedID { get; set; }

        public required int Skill { get; set; }

        public required uint ResistanceAV { get; set; }

        public required uint Unknown1 { get; set; }

        public required FormId LightID { get; set; }

        public required float TaperWeight { get; set; }

        public required FormId HitShader { get; set; }

        public required FormId EnchantShader { get; set; }

        public required uint SkillLevel { get; set; }

        public required uint Area { get; set; }

        public required float CastingTime { get; set; }

        public required float TaperCurve { get; set; }

        public required float TaperDuration { get; set; }

        public required float SecondAVWeight { get; set; }

        public required uint EffectType { get; set; }

        public required int PrimaryAV { get; set; }

        public required FormId ProjectileID { get; set; }

        public required FormId ExplosionID { get; set; }

        public required uint CastType { get; set; }

        public required uint DeliveryType { get; set; }

        public required int SecondAV { get; set; }

        public required FormId CastingArtID { get; set; }

        public required FormId HitEffectArtID { get; set; }

        public required FormId ImpactDataID { get; set; }

        public required float SkillUsageMult { get; set; }

        public required FormId DualCastID { get; set; }

        public required float DualCastScale { get; set; }

        public required FormId EnchantArtID { get; set; }

        public required uint NullData1 { get; set; }

        public required uint NullData2 { get; set; }

        public required FormId EquipAbility { get; set; }

        public required FormId ImageSpaceModID { get; set; }

        public required FormId PerkID { get; set; }

        public required uint SoundVolume { get; set; }

        public required float ScriptAIDataScore { get; set; }

        public required float ScriptAIDataDelayTime { get; set; }
    }

    [Flags]
    public enum MagicEffectDataFlags : uint
    {
        Hostile = 0x00000001,
        Recover = 0x00000002,
        Detrimental = 0x00000004,
        SnapToNavmesh = 0x00000008,
        NoHitEvent = 0x00000010,
        DispelEffects = 0x00000100,
        NoDuration = 0x00000200,
        NoMagnitude = 0x00000400,
        NoArea = 0x00000800,
        FXPersist = 0x00001000,
        GoryVisual = 0x00004000,
        HideInUI = 0x00008000,
        NoRecast = 0x00020000,
        PowerAffectsMagnitude = 0x00200000,
        PowerAffectsDuration = 0x00400000,
        Painless = 0x04000000,
        NoHitEffect = 0x08000000,
        NoDeathDispel = 0x10000000,
        Unknown1 = 0x40000000,
        Unknown2 = 0x80000000,
    }

    public struct MagicEffectSoundData
    {
        public required SoundType Type { get; set; }

        public required FormId SoundDesc { get; set; }
    }

    public enum SoundType : uint
    {
        DrawSheathe = 0,
        Charge = 1,
        Ready = 2,
        Release = 3,
        ConcentrationCastLoop = 4,
        OnHit = 5,
    }

    public static MagicEffectRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string editorId = reader.ReadUtf8NullTerminated("EDID");

        VmadStruct? vmad = null;
        if (reader.PeekdUtf8Value(Tes4Constants.TypeStringLength) == "VMAD")
        {
            reader.ReadTypeString("VMAD");
            ushort size = reader.ReadU16Value();
            vmad = VmadStruct.Read(reader, size);
        }

        string? name = reader.ReadUtf8NullTerminatedOptional("FULL");

        FormId? staticId = reader.ReadFormIdOptional("MDOB");

        uint? keywordCount = reader.ReadU32Optional("KSIZ");

        FormId[] keywords = reader.ReadFormListOptional("KWDA");

        MagicEffectData data = reader.ReadStruct<MagicEffectData>("DATA");

        FormId[] counterEffects = reader.ReadFormListOptional("ESCE");

        MagicEffectSoundData[] soundData = reader.ReadStructMultiple<MagicEffectSoundData>("SNDD");

        string description = reader.ReadUtf8NullTerminated("DNAM");

        ConditionDataField[] conditions = reader.ReadMultipleFields<ConditionDataField>("CTDA").ToArray();

        return new MagicEffectRecord
        {
            Metadata = metadata,

            EditorID = editorId,
            Vmad = vmad,
            Name = name,
            Static = staticId,
            KeywordCount = keywordCount,
            Keywords = keywords,
            Data = data,
            CounterEffects = counterEffects,
            SoundData = soundData,
            Description = description,
            Conditions = conditions
        };
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

namespace Tes4Parser.Records;

public class SpellRecord : Record, IReadWrite<SpellRecord>
{
    public const string TypeString = "SPEL";

    public required string EditorID { get; set; }

    public required ObjectBoundaryStruct ObjectBounds { get; set; }

    public required string? FullName { get; set; }

    public required FormId? MenuIcon { get; set; }

    public required FormId EquipType { get; set; }

    public required string Description { get; set; }

    public required SpellItemStruct SpellItem { get; set; }

    public required List<SpellEffectStruct> SpellEffects { get; set; }

    public static SpellRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);
        string editorId = reader.ReadUtf8NullTerminated("EDID");

        ObjectBoundaryStruct objectBound = reader.ReadStruct<ObjectBoundaryStruct>("OBND");
        string? fullName = reader.ReadUtf8NullTerminatedOptional("FULL"); // Todo: Localized string, might need special handling.
        FormId? menuIcon = reader.ReadFormIdOptional("MDOB");
        FormId equipType = reader.ReadFormId("ETYP");
        string description = reader.ReadUtf8NullTerminated("DESC"); // Todo: Localized string, might need special handling.

        SpellItemStruct spellItem = reader.ReadStruct<SpellItemStruct>("SPIT");

        List<SpellEffectStruct> spellEffects = [];

        while (reader.PeekTypeString(SpellEffectStruct.TypeString))
        {
            spellEffects.Add(SpellEffectStruct.Read(reader));
        }

        return new SpellRecord
        {
            Metadata = metadata,

            EditorID = editorId,
            ObjectBounds = objectBound,
            FullName = fullName,
            MenuIcon = menuIcon,
            EquipType = equipType,
            Description = description,
            SpellItem = spellItem,
            SpellEffects = spellEffects
        };
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }

    public struct SpellItemStruct
    {
        public uint SpellCost { get; set; }

        public SpellItemFlags Flags { get; set; }

        public SpellType Type { get; set; }

        public float ChargeTime { get; set; }

        public CastType CastType { get; set; }

        public SpellDelivery Delivery { get; set; }

        public float CastDuration { get; set; }

        public float Range { get; set; }

        public FormId HalfCostPerk { get; set; }
    }

    [Flags]
    public enum SpellItemFlags : uint
    {
        NotAutoCalculate = 0x00000001,
        Unknown1 = 0x00010000,
        PCStartSpell = 0x00020000,
        Unknown2 = 0x00040000,
        AreaEffectIgnoresLineOfSight = 0x00080000,
        IgnoreResistance = 0x00100000,
        DisallowSpellAbsorbOrReflect = 0x00200000,
        Unknown3 = 0x00400000,
        NoDualCastModifications = 0x00800000,
    }

    public enum SpellType : uint
    {
        Spell = 0x00,
        Disease = 0x01,
        Power = 0x02,
        LesserPower = 0x03,
        Ability = 0x04,
        Poison = 0x05,
        Addiction = 0x0A,
        Voice = 0x0B
    }

    public enum CastType : uint
    {
        ConstantEffect = 0x00,
        FireAndForget = 0x01,
        Concentration = 0x02
    }

    public enum SpellDelivery : uint
    {
        Self = 0x00,
        Contact = 0x01,
        Aimed = 0x02,
        TargetActor = 0x03,
        TargetLocation = 0x04
    }

    public struct SpellEffectStruct : IReadWrite<SpellEffectStruct>
    {
        public const string TypeString = "EFID";

        public required FormId EffectId { get; set; }

        public required SpellEffectItem SpellEffectItem { get; set; }

        public required ConditionDataField[] Conditions { get; set; }

        public static SpellEffectStruct Read(Tes4Reader reader)
        {
            FormId effectId = reader.ReadFormId(TypeString);
            SpellEffectItem spellEffectItem = reader.ReadStruct<SpellEffectItem>("EFIT");
            ConditionDataField[] conditions = reader.ReadMultipleFields<ConditionDataField>("CTDA").ToArray();

            return new SpellEffectStruct
            {
                EffectId = effectId,
                SpellEffectItem = spellEffectItem,
                Conditions = conditions
            };
        }

        public void Write(Tes4Writer writer)
        {
            throw new NotImplementedException();
        }
    }

    public struct SpellEffectItem
    {
        public float Magnitude { get; set; }

        public uint AreaOfEffect { get; set; }

        public uint Duration { get; set; }
    }
}

public struct ObjectBoundaryStruct // Todo: Move this somewhere else.
{
    public short X1 { get; set; }

    public short Y1 { get; set; }

    public short Z1 { get; set; }

    public short X2 { get; set; }

    public short Y2 { get; set; }

    public short Z2 { get; set; }
}

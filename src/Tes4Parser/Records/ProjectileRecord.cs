namespace Tes4Parser.Records;

public class ProjectileRecord : Record, IReadWrite<ProjectileRecord>
{
    public const string TypeString = "PROJ";

    public required ObjectBoundaryStruct ObjectBounds { get; set; }

    public required string? FullName { get; set; }

    public required ModelStruct? Model { get; set; }

    public required DestructionRecord? Destruction { get; set; }

    public required ModelStruct? EffectModel { get; set; }

    public required ProjectileDataStruct Data { get; set; }

    public required SoundDetectionLevel? DetectionLevel { get; set; }

    public struct ProjectileDataStruct
    {
        public ProjectileDataFlags Flags { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public float Gravity { get; set; }

        public float Speed { get; set; }

        public float Range { get; set; }

        public FormId Light { get; set; }

        public FormId MuzzleFlashLight { get; set; }

        public float TracerChance { get; set; }

        public float ExplosionProximity { get; set; }

        public float ExplosionTimer { get; set; }

        public FormId ExplosionType { get; set; }

        public FormId SoundRecord { get; set; }

        public float MuzzleDuration { get; set; }

        public float FadeDuration { get; set; }

        public float ImpactForce { get; set; }

        public FormId CountdownSound { get; set; }

        public uint AlwaysZero { get; set; }

        public FormId DefaultWeaponSource { get; set; }

        public float ConeSpread { get; set; }

        public float CollisionRadius { get; set; }

        public float Lifetime { get; set; }

        public float RelaunchInterval { get; set; }

        public FormId DecalData { get; set; }

        public FormId CollisionLayer { get; set; }
    }

    [Flags]
    public enum ProjectileDataFlags : ushort
    {
        Hitscan = 0x0001,
        Explosion = 0x0002,
        AltTrigger = 0x0004,
        MuzzleFlash = 0x0008,
        Unknown = 0x0010,
        CanBeDisabled = 0x0020,
        CanBePickedUp = 0x0040,
        Supersonic = 0x0080,
        CritEffectPinsLimbs = 0x0100,
        PassThroughSmallTransparent = 0x0200,
        DisableCombatAimCorrection = 0x0400
    }

    public enum ProjectileType : ushort
    {
        Missile = 0x01,
        Lobber = 0x02,
        Beam = 0x04,
        Flame = 0x08,
        Cone = 0x10,
        Barrier = 0x20,
        Arrow = 0x40
    }

    public enum SoundDetectionLevel : uint
    {
        Loud = 0x00,
        Normal = 0x01,
        Silent = 0x02,
        VeryLoud = 0x03
    }

    public static ProjectileRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string editorId = reader.ReadUtf8NullTerminated("EDID");

        ObjectBoundaryStruct objectBoundary = reader.ReadStruct<ObjectBoundaryStruct>("OBND");
        string? fullName = reader.ReadUtf8NullTerminatedOptional("FULL"); // Todo: Localized string, might need special handling.

        ModelStruct? model = null;
        if (reader.PeekTypeString("MODL"))
        {
            reader.ReadTypeString("MODL");
            model = ModelStruct.Read(reader);
        }

        DestructionRecord? destruction = null;
        if (reader.PeekTypeString("DEST"))
        {
            destruction = DestructionRecord.Read(reader);
        }

        ProjectileDataStruct data = reader.ReadStruct<ProjectileDataStruct>("DATA");

        ModelStruct? effectModel = null;
        if (reader.PeekTypeString("NAM1"))
        {
            reader.ReadTypeString("NAM1");
            effectModel = ModelStruct.Read(reader);
        }

        SoundDetectionLevel? soundDetectionLevel = reader.ReadStructOptional<SoundDetectionLevel>("VNAM");

        return new ProjectileRecord
        {
            ObjectBounds = objectBoundary,
            FullName = fullName,
            Model = model,
            Destruction = destruction,
            EffectModel = effectModel,
            Data = data,
            DetectionLevel = soundDetectionLevel,
};
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

public struct ModelStruct : IReadWrite<ModelStruct> // Todo: Move this somewhere else.
{
    public string FileName { get; set; }

    public ModelTexturesStruct? Textures { get; set; }

    public ModelAlternateTexturesStruct? AlternateTextures { get; set; }

    public static ModelStruct Read(Tes4Reader reader)
    {
        string fileName = reader.ReadUtf8NullTerminatedValue();

        ModelTexturesStruct? modelTextures = null;
        if (reader.PeekTypeString("MODT"))
        {
            modelTextures = ModelTexturesStruct.Read(reader);
        }

        ModelAlternateTexturesStruct? modelAlternateTextures = null;
        if (reader.PeekTypeString("MODS"))
        {
            throw new NotImplementedException();
            // Todo
            //modelAlternateTextures = ModelAlternateTexturesStruct.Read(reader);
        }

        return new ModelStruct
        {
            FileName = fileName,
            Textures = modelTextures,
            AlternateTextures = modelAlternateTextures
        };
    }

    public void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

public struct ModelTexturesStruct : IReadWrite<ModelTexturesStruct> // Todo: Move this somewhere else.
{
    public uint Unknown4Count { get; set; }

    public uint Unknown5Count { get; set; }

    public uint? Unknown3 { get; set; }

    public static ModelTexturesStruct Read(Tes4Reader reader)
    {
        reader.ReadTypeString("MODT");

        uint size = reader.ReadU16Value();

        uint count = reader.ReadU32Value();

        // Todo: Uncertainty, will need correction in future.

        uint unknown4Count = count >= 1
            ? reader.ReadU32Value()
            : 0;

        uint unknown5Count = count >= 2
            ? reader.ReadU32Value()
            : 0;

        uint? unknown3 = count >= 3
            ? reader.ReadU32Value()
            : null;

        return new ModelTexturesStruct
        {
            Unknown4Count = unknown4Count,
            Unknown5Count = unknown5Count,
            Unknown3 = unknown3,
        };
    }

    public void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

public struct ModelAlternateTexturesStruct // Todo: Move this somewhere else.
{

}


public class DestructionRecord : Record, IReadWrite<DestructionRecord> // Todo: Move this somewhere else.
{
    public const string TypeString = "DEST";

    public struct DestructionDataStruct
    {
        public uint Health { get; set; }

        public byte Count { get; set; }

        public bool VATsEnabled { get; set; }

        public byte Unknown1 { get; set; }

        public byte Unknown2 { get; set; }
    }

    public static DestructionRecord Read(Tes4Reader reader)
    {
        // Todo: Does this contain metadata and EDID?
        DestructionDataStruct destructionData = reader.ReadStruct<DestructionDataStruct>("DEST");

        throw new NotImplementedException();
    }

    public override void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}
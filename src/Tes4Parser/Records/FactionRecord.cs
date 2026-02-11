using System;

namespace Tes4Parser.Records;

public class FactionRecord : Record, IReadWrite<FactionRecord>
{
    public const string TypeString = "FACT";

    public required string EditorID { get; set; }

    public required string Name { get; set; }

    public required FactionFlags Flags { get; set; }

    public required FormId? PrisonMarker { get; set; }

    public required FormId? FollowerWaitMarker { get; set; }

    public required FormId? EvidenceChest { get; set; }

    public required FormId? PlayerBelongingsChest { get; set; }

    public required FormId? CrimeGroup { get; set; }

    public required FormId? JailOutfit { get; set; }

    public required uint? RankId { get; set; }

    public required string? MaleRankTitle { get; set; }

    public required string? FemaleRankTitle { get; set; }

    public required FormId? VendorList { get; set; }

    public required FormId? VendorChest { get; set; }

    public required VendorVariablesStruct? VendorVariables { get; set; }

    public struct InterfactionRelationsStruct
    {
        public required string FactionFormId { get; set; }

        public required uint Mod { get; set; }

        public required RelationsCombat Combat { get; set; }
    }

    [Flags]
    public enum FactionFlags : uint
    {
        HiddenfromPC = 0x1,
        SpecialCombat = 0x2,
        TrackCrime = 0x40,
        IgnoreMurder = 0x80,
        IgnoreAssault = 0x100,
        IgnoreStealing = 0x200,
        IgnoreTrespass = 0x400,
        DoNotReportCrimesAgainstMembers = 0x800,
        CrimeGoldUseDefaults = 0x1000,
        IgnorePickpocket = 0x2000,
        Vendor = 0x4000,
        CanBeOwner = 0x8000,
        IgnoreWerewolf = 0x10000,
    }

    public enum RelationsCombat
    {
        Neutral = 0,
        Enemy = 1,
        Ally = 2,
        Friend = 3,
    }

    public struct CrimeGoldStruct
    {
        public required bool Arrest { get; set; }

        public required bool AttackOnSight { get; set; }

        public required ushort Murder { get; set; }

        public required ushort Assault { get; set; }

        public required ushort Trespass { get; set; }

        public required ushort Pickpocket { get; set; }

        public required ushort Unused { get; set; }

        public required float StealMult { get; set; }

        public required ushort Escape { get; set; }

        public required ushort Werewolf { get; set; }
    }

    public struct VendorVariablesStruct
    {
        public required ushort StartHour { get; set; }

        public required ushort EndHour { get; set; }

        public required uint Radius { get; set; }

        public required bool BuysStolenItems { get; set; }

        public required bool NotSellBuy { get; set; }

        public required ushort Unused { get; set; }
    }

    public static FactionRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string editorId = reader.ReadUtf8NullTerminated("EDID");
        string name = reader.ReadUtf8NullTerminated("FULL");

        // Todo: XNAM

        FactionFlags flags = (FactionFlags)reader.ReadU32("DATA");

        FormId? prisonMarker = reader.ReadFormIdOptional("JAIL");
        FormId? followerWaitMarker = reader.ReadFormIdOptional("WAIT");
        FormId? evidenceChest = reader.ReadFormIdOptional("STOL");
        FormId? playerBelongingsChest = reader.ReadFormIdOptional("PLCN");
        FormId? crimeGroup = reader.ReadFormIdOptional("CRGR");
        FormId? jailOutfit = reader.ReadFormIdOptional("JOUT");

        CrimeGoldStruct? crimeGold = reader.ReadStructOptional<CrimeGoldStruct>("CRVA"); // Todo: Currently unused.

        uint? rankId = reader.ReadU32Optional("RNAM");
        string? maleRankTitle = reader.ReadUtf8NullTerminatedOptional("MNAM");
        string? femaleRankTitle = reader.ReadUtf8NullTerminatedOptional("FNAM");
        FormId? vendorList = reader.ReadFormIdOptional("VEND");
        FormId? vendorChest = reader.ReadFormIdOptional("VENC");

        VendorVariablesStruct? vendorVariables = reader.ReadStructOptional<VendorVariablesStruct>("VENV");

        return new FactionRecord
        {
            Metadata = metadata,

            EditorID = editorId,
            Name = name,
            Flags = flags,
            PrisonMarker = prisonMarker,
            FollowerWaitMarker = followerWaitMarker,
            EvidenceChest = evidenceChest,
            PlayerBelongingsChest = playerBelongingsChest,
            CrimeGroup = crimeGroup,
            JailOutfit = jailOutfit,
            RankId = rankId,
            MaleRankTitle = maleRankTitle,
            FemaleRankTitle = femaleRankTitle,
            VendorList = vendorList,
            VendorChest = vendorChest,
            VendorVariables = vendorVariables,
        };
    }

    public override void Write(Tes4Writer writer)
    {
        //throw new NotImplementedException();
    }
}

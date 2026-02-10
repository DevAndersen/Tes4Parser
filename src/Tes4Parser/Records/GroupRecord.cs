namespace Tes4Parser.Records;

public class GroupRecord : Record
{
    public const string TypeString = "GRUP";

    public required Record[] Records { get; set; }
}

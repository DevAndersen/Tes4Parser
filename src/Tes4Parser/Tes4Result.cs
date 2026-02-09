using Tes4Parser.Records;

namespace Tes4Parser;

public class Tes4Result
{
    public required Tes4Record Header { get; set; }

    public required List<GroupRecord> Groups { get; set; }
}

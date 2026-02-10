using Tes4Parser.Records;

namespace Tes4Parser;

public static class Tes4StreamParser
{
    public static Tes4Result Read(Stream stream)
    {
        using Tes4Reader reader = new Tes4Reader(stream);

        Tes4Record header = reader.ReadHeader();

        Record[] records = reader.ReadRecords(true).ToArray();
        List<GroupRecord> groups = records.OfType<GroupRecord>().ToList();

        return new Tes4Result
        {
            Header = header,
            Groups = groups
        };
    }

    public static void Write(Stream stream, Tes4Result result)
    {
        Tes4Writer writer = new Tes4Writer(stream);

        writer.Write(result);
    }
}

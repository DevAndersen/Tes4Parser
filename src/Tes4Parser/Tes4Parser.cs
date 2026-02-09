namespace Tes4Parser;

public static class Tes4StreamParser
{
    public static Tes4Result Read(Stream stream)
    {
        using Tes4Reader reader = new Tes4Reader(stream);
        reader.ReadRecords().ToArray();
        return null!;
    }

    public static void Write(Tes4Result result)
    {
        _ = result;
    }
}

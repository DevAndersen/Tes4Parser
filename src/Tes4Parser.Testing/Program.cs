using Tes4Parser;

string path = "test.esp";

using FileStream fileStream = File.Open(path, FileMode.Open);

DebugReadStream readStream = new DebugReadStream();
fileStream.CopyTo(readStream);
readStream.Position = 0;

Tes4Result result = Tes4StreamParser.Read(readStream);

using DebugWriteStream writeStream = new DebugWriteStream();

Tes4StreamParser.Write(writeStream, result);

fileStream.Position = 0;
writeStream.Position = 0;

int i;
for (i = 0; i < writeStream.Length; i++)
{
    int fileByte = fileStream.ReadByte();
    int writeByte = writeStream.ReadByte();
    if (fileByte != writeByte)
    {
        throw new InvalidDataException($"Stream difference at position {i}.");
    }
}

if (fileStream.Length != writeStream.Length)
{
    throw new InvalidDataException($"Stream lengths were not identical, position {i}.");
}

Console.WriteLine();

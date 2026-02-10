using Tes4Parser;
using Tes4Parser.Testing;

string path = "test.esp";

using FileStream fileStream = File.Open(path, FileMode.Open);
using DebugReadStream readStream = new DebugReadStream();
fileStream.CopyTo(readStream);
readStream.Position = 0;

Tes4Result result = Tes4StreamParser.Read(readStream);

using DebugWriteStream writeStream = new DebugWriteStream();

Tes4StreamParser.Write(writeStream, result);

// Validation

using DebugReadStream checkReadStream = new DebugReadStream();
fileStream.Position = 0;
fileStream.CopyTo(checkReadStream);
checkReadStream.Position = 0;

using DebugReadStream checkWriteStream = new DebugReadStream();
writeStream.Position = 0;
writeStream.CopyTo(checkWriteStream);
checkWriteStream.Position = 0;

int i;
for (i = 0; i < checkWriteStream.Length; i++)
{
    int fileByte = checkReadStream.ReadByte();
    int writeByte = checkWriteStream.ReadByte();
    if (fileByte != writeByte)
    {
        throw new InvalidDataException($"Stream difference at position {i}.");
    }
}

if (fileStream.Length != writeStream.Length)
{
    throw new InvalidDataException($"Stream lengths were not identical, position {i}.");
}

Console.WriteLine("File read and written with zero diff");

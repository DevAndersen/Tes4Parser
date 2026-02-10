using Tes4Parser;

string path = "test.esp";

using FileStream stream = File.Open(path, FileMode.Open);

DebugReadStream readStream = new DebugReadStream();
stream.CopyTo(readStream);
readStream.Position = 0;

Tes4Result result = Tes4StreamParser.Read(readStream);

using DebugWriteStream writeStream = new DebugWriteStream();

Tes4StreamParser.Write(writeStream, result);

Console.WriteLine();

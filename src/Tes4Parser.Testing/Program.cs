using Tes4Parser;

string path = "test.esp";

using FileStream stream = File.Open(path, FileMode.Open);

DebugStream debugStream = new DebugStream();
stream.CopyTo(debugStream);
debugStream.Position = 0;

Tes4StreamParser.Read(debugStream);

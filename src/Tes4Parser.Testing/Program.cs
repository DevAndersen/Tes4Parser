using Tes4Parser;

string path = "test.esp";

using FileStream stream = File.Open(path, FileMode.Open);

Tes4StreamParser.Read(stream);

using Tes4Parser;

string path = "test.esp";

using FileStream stream = File.Open(path, FileMode.Open);
using Tes4Reader reader = new Tes4Reader(stream);

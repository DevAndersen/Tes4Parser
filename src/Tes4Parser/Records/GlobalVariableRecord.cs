namespace Tes4Parser.Records;

public class GlobalVariableRecord : Record, IReadWrite<GlobalVariableRecord>
{
    public const string TypeString = "GLOB";

    public required string EditorID { get; set; }

    public required GlobalVariableValueType ValueType { get; set; }

    public required float Value { get; set; }

    public static GlobalVariableRecord Read(Tes4Reader reader)
    {
        RecordMetadata metadata = RecordMetadata.Read(reader);

        string editorId = reader.ReadUtf8NullTerminated("EDID");
        GlobalVariableValueType valueType = (GlobalVariableValueType)reader.ReadU8("FNAM");
        float value = reader. ReadF32("FLTV");

        return new GlobalVariableRecord
        {
            Metadata = metadata,

            EditorID = editorId,
            ValueType = valueType,
            Value = value
        };
    }

    public override void Write(Tes4Writer writer)
    {
        writer.WriteTypeString(TypeString);
        Metadata.Write(writer);

        writer.WriteUtf8NullTerminated("EDID", EditorID);
        writer.WriteU8("FNAM", (byte)ValueType);
        writer.WriteF32("FLTV", Value);
    }

    public enum GlobalVariableValueType : byte
    {
        Short = (byte)'s',
        Long = (byte)'l',
        Float = (byte)'f',
    }
}

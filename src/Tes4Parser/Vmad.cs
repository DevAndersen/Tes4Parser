namespace Tes4Parser;

public struct Vmad : IReadWrite<Vmad> // Todo: Find a more appropriate name for this.
{
    public const string TypeString = "VMAD";

    public short Version { get; set; }

    public short ObjectFormat { get; set; }

    public struct ScriptSection : IReadWrite<ScriptSection>
    {
        public string ScriptName { get; set; }

        public byte Status { get; set; }

        public PropertyEntry[] Properties { get; set; }

        public static ScriptSection Read(Tes4Reader reader)
        {
            string scriptName = reader.ReadWStringValue();
            byte status = reader.ReadU8Value();
            ushort propertyCount = reader.ReadU16Value();

            PropertyEntry[] properties = new PropertyEntry[propertyCount];

            for (int i = 0; i < propertyCount; i++)
            {
                properties[i] = PropertyEntry.Read(reader);
            }

            return new ScriptSection
            {
                ScriptName = scriptName,
                Status = status,
                Properties = properties
            };
        }

        public readonly void Write(Tes4Writer writer)
        {
            writer.WriteWStringValue(ScriptName);
            writer.WriteU8Value(Status);
            writer.WriteU16Value((ushort)Properties.Length);

            foreach (PropertyEntry property in Properties)
            {
                property.Write(writer);
            }
        }
    }

    public struct PropertyEntry : IReadWrite<PropertyEntry>
    {
        public string PropertyName { get; set; }

        public PropertyType PropertyType { get; set; } // Todo: Convert to enum

        public byte Status { get; set; }

        public object Value { get; set; }

        public static PropertyEntry Read(Tes4Reader reader)
        {
            string propertyName = reader.ReadWStringValue();
            PropertyType propertyType = (PropertyType)reader.ReadU8Value();
            byte status = reader.ReadU8Value();

            object value;

            if (propertyType == PropertyType.Object)
            {
                reader.ReadU16Value(); // Unused
                ushort alias = reader.ReadU16Value();
                FormId formId = reader.ReadFormIdValue();

                value = new FormIdWithAlias
                {
                    Alias = alias,
                    FormId = formId
                };
            }
            else
            {
                throw new NotImplementedException();
            }

            return new PropertyEntry
            {
                PropertyName = propertyName,
                PropertyType = propertyType,
                Status = status,
                Value = value
            };
        }

        public void Write(Tes4Writer writer)
        {
            throw new NotImplementedException();
        }
    }

    public enum PropertyType : byte
    {
        Object = 1,
        WString = 2,
        Int32 = 3,
        Float = 4,
        Bool = 5,
        ObjectArray = 11,
        WStringArray = 12,
        Int32Array = 13,
        FloatArray = 14,
        BoolArray = 15,
    }

    public static Vmad Read(Tes4Reader reader)
    {
        short version = reader.ReadI16Value();
        short objectFormat = reader.ReadI16Value();

        ushort scriptCount = reader.ReadU16Value();

        ScriptSection[] scriptSections = new ScriptSection[scriptCount];

        for (int i = 0; i < scriptCount; i++)
        {
            scriptSections[i] = ScriptSection.Read(reader);
        }

        // Todo: Fragments. Maybe peeking on the next type string is a good approach to determine if there are any fragments?

        return new Vmad
        {
            Version = version,
            ObjectFormat = objectFormat
        };
    }

    public void Write(Tes4Writer writer)
    {
        throw new NotImplementedException();
    }
}

public struct FormIdWithAlias // Todo: Find a better place for this, if it is used elsewhere.
{
    public ushort Alias { get; set; }

    public FormId FormId { get; set; }
}

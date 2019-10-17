using System;

namespace Station {
    public class PluginField {
        public string Label;
        public FieldType Type;
        public string Identifier;
        public Boolean IsRequired;

        public enum FieldType {
            TextInput,
            NumberInput,
            FileInput,
            HiddenInput,
        }
    }
}

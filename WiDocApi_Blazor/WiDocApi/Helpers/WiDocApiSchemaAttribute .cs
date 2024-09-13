
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class WiDocApiSchemaAttribute : Attribute
    {
        public string Description { get; }

        public WiDocApiSchemaAttribute(string description)
        {
            Description = description;
        }
    }





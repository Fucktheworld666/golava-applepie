using System;

namespace GoLava.ApplePie.Contracts.Attributes
{
    public class JsonDataPropertyAttribute : Attribute
    {
        public JsonDataPropertyAttribute(string name = null)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}

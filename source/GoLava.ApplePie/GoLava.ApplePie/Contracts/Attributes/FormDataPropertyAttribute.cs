using System;

namespace GoLava.ApplePie.Contracts.Attributes
{
    public class FormDataPropertyAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}
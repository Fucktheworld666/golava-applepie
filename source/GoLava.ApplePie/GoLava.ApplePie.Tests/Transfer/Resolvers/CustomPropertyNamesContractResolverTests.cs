using System.Collections.Generic;
using System.Reflection;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Transfer.Resolvers;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer.Resolvers
{
    public class CustomPropertyNamesContractResolverTests
    {
        [Theory]
        [InlineData("Foo", "foo")]
        [InlineData("foo", "foo")]
        [InlineData("FooBAR", "fooBAR")]
        public void NamesAreConvertedToCamelCase(string input, string result)
        {
            var resolver = new CustomPropertyNamesContractResolver();
            var name = resolver.GetResolvedPropertyName(input);
            Assert.Equal(result, name);
        }

        [Fact]
        public void ContractWithJsonDataClassPropertyAttributeUsesNameOfClassString()
        {
            var propertyName = this.GetPropertyNameOfResolveContract<Root<string>>();
            Assert.Equal("String", propertyName);
        }

        [Fact]
        public void ContractWithJsonDataClassPropertyAttributeUsesNameOfClassFoo()
        {
            var propertyName = this.GetPropertyNameOfResolveContract<Root<Foo>>();
            Assert.Equal("Foo", propertyName);
        }

        [Fact]
        public void ContractWithJsonDataClassPropertyAttributeUsesNameOfClassArray()
        {
            var propertyName = this.GetPropertyNameOfResolveContract<Root<int[]>>();
            Assert.Equal("Int32s", propertyName);
        }

        [Fact]
        public void ContractWithJsonDataClassPropertyAttributeUsesNameOfClassList()
        {
            var propertyName = this.GetPropertyNameOfResolveContract<Root<List<Foo>>>();
            Assert.Equal("Foos", propertyName);
        }

        [Fact]
        public void ContractWithJsonDataClassPropertyAttributeUsesNameOfJsonDataProperty()
        {
            var propertyName = this.GetPropertyNameOfResolveContract<Root<Bar>>();
            Assert.Equal("Classy", propertyName);
        }

        [Fact]
        public void ContractWithJsonDataClassPropertyAttributeUsesNameOfJsonDataPropertyPlural()
        {
            var propertyName = this.GetPropertyNameOfResolveContract<Root<List<Bar>>>();
            Assert.Equal("Classies", propertyName);
        }

        public string GetPropertyNameOfResolveContract<T>()
        {
            var resolver = new CustomPropertyNamesContractResolver();
            var jc = resolver.ResolveContract(typeof(T));
            var properties = (JsonPropertyCollection)jc
                .GetType()
                .GetProperty("Properties", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .GetValue(jc);
            return properties[0].PropertyName;
        }

        public class Root<T>
        {
            [JsonDataClassProperty]
            public T Data { get; set; }
        }

        public class Foo { }

        [JsonDataProperty("Classy")]
        public class Bar { } 
    }
}

using System;
using GoLava.ApplePie.Serializers;
using Xunit;

namespace GoLava.ApplePie.Tests.Serializers
{
    public class JsonSerializerTests
    {
        [Fact]
        public void PassingNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonSerializer(null));
        }

        [Fact]
        public void NullIsSerialized()
        {
            var serializer = new JsonSerializer();
            var json = serializer.Serialize(null);
            Assert.Equal("null", json);
        }

        [Fact]
        public void EmptyStringIsSerialized()
        {
            var serializer = new JsonSerializer();
            var json = serializer.Serialize(string.Empty);
            Assert.Equal("\"\"", json);
        }

        [Fact]
        public void StringIsSerialized()
        {
            var serializer = new JsonSerializer();
            var json = serializer.Serialize("foo");
            Assert.Equal("\"foo\"", json);
        }

        [Fact]
        public void ZeroIsSerialized()
        {
            var serializer = new JsonSerializer();
            var json = serializer.Serialize(0);
            Assert.Equal("0", json);
        }

        [Fact]
        public void ObjectIsSerialized()
        {
            var serializer = new JsonSerializer();
            var json = serializer.Serialize(new Contract { Foo = "Fish", Bar = 42 });
            Assert.Equal("{\"Foo\":\"Fish\",\"Bar\":42}", json);
        }

        [Fact]
        public void NullIsDeserialized()
        {
            var serializer = new JsonSerializer();
            var o = serializer.Deserialize<object>("null");
            Assert.Null(o);
        }

        [Fact]
        public void EmptyStringIsDeserialized()
        {
            var serializer = new JsonSerializer();
            var s = serializer.Deserialize<string>("\"\"");
            Assert.Equal(string.Empty, s);
        }

        [Fact]
        public void StringIsDeserialized()
        {
            var serializer = new JsonSerializer();
            var s = serializer.Deserialize<string>("\"foo\"");
            Assert.Equal("foo", s);
        }

        [Fact]
        public void ZeroIsDeserialized()
        {
            var serializer = new JsonSerializer();
            var i = serializer.Deserialize<int>("0");
            Assert.Equal(0, i);
        }

        [Fact]
        public void ObjectIsDeserialized()
        {
            var serializer = new JsonSerializer();
            var c = serializer.Deserialize<Contract>("{\"Foo\":\"Fish\",\"Bar\":42}");
            Assert.Equal("Fish", c.Foo);
            Assert.Equal(42, c.Bar);
        }

        public class Contract 
        {
            public string Foo { get; set; }

            public int Bar { get; set; }
        }
    }
}

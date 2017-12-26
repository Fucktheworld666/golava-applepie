using System;
using System.Collections.Generic;
using System.Linq;
using GoLava.ApplePie.Transfer;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer
{
    public class RestHeadersTests
    {
        [Fact]
        public void AddValueCorrectly()
        {
            var headers = new RestHeaders();
            headers.Add("foo", "bar");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.Equal("bar", values.Single());
        }

        [Fact]
        public void AddValueDoesNotOverwriteExistingValues()
        {
            var headers = new RestHeaders
            {
                { "foo", "bar" }
            };
            headers.Add("foo", "bla");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.True(values.Any(x => x == "bar"), "bar value not found");
            Assert.True(values.Any(x => x == "bla"), "bla value not found");
        }

        [Fact]
        public void AddValuesCorrectly()
        {
            var headers = new RestHeaders();
            headers.Add("foo", "bar", "bla");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.True(values.Any(x => x == "bar"), "bar value not found");
            Assert.True(values.Any(x => x == "bla"), "bla value not found");
        }

        [Fact]
        public void AddValuesDoesNotOverwriteExistingValues()
        {
            var headers = new RestHeaders
            {
                { "foo", "bar" }
            };
            headers.Add("foo", "bla", "baz");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.True(values.Any(x => x == "bar"), "bar value not found");
            Assert.True(values.Any(x => x == "bla"), "bla value not found");
            Assert.True(values.Any(x => x == "baz"), "baz value not found");
        }

        [Fact]
        public void SetValueCorrectly()
        {
            var headers = new RestHeaders();
            headers.Set("foo", "bar");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.Equal("bar", values.Single());
        }

        [Fact]
        public void SetValuesDoesOverwriteExistingValues()
        {
            var headers = new RestHeaders
            {
                { "foo", "bar" }
            };
            headers.Set("foo", "bla");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.Equal("bla", values.Single());
        }

        [Fact]
        public void SetValuesCorrectly()
        {
            var headers = new RestHeaders();
            headers.Set("foo", "bar", "bla");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.True(values.Any(x => x == "bar"), "bar value not found");
            Assert.True(values.Any(x => x == "bla"), "bla value not found");
        }

        [Fact]
        public void SetValueDoesOverwriteExistingValues()
        {
            var headers = new RestHeaders
            {
                { "foo", "bar" }
            };
            headers.Set("foo", "bla", "baz");

            Assert.True(headers.TryGetValue("foo", out ISet<string> values), "foo key not found");
            Assert.True(values.Any(x => x == "bla"), "bla value not found");
            Assert.True(values.Any(x => x == "baz"), "baz value not found");
        }
    }
}

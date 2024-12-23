using FluentAssertions;

using Mongo.Migration.Documents;
using Xunit;

namespace Mongo.Migration.Test.Documents
{
    
    public class DocumentVersion_When_casting
    {
        [Fact]
        public void If_implicit_string_to_version_Then_cast_should_work()
        {
            DocumentVersion version = "1.0.2";

            version.ToString().Should().Be("1.0.2");
        }

        [Fact]
        public void If_implicit_version_to_string_Then_cast_should_work()
        {
            var version = new DocumentVersion("1.0.2");

            string versionString = version;

            versionString.Should().Be("1.0.2");
        }
    }
}
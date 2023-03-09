namespace StarWars.JediArchives.Infrastructure.Tests.UnitTests.QueryParser
{
    [TestClass]
    public class QueryProcessorTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void Construct_QueryParser_WithNullPropertyCollectionParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            HashSet<string> properties = null;

            // Act
            var queryParser = () => new QueryProcessor(propertyCollection: properties);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(queryParser);
        }

        [TestMethod]
        public void Construct_QueryParser_WithNullTypeParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            Type type = null;

            // Act
            var queryParser = () => new QueryProcessor(targetType: type);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(queryParser);
        }
    }
}

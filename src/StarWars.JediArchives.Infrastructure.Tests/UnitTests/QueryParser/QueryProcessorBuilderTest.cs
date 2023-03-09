namespace StarWars.JediArchives.Infrastructure.Tests.UnitTests.QueryParser
{
    [TestClass]
    public class QueryProcessorBuilderTest
    {
        [TestMethod]
        public void Construct_QueryParser_WithNullTargetTypeParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            Type targetType = null;

            // Act
            var instantiateDelegate = () => new QueryProcessorBuilder(targetType);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => instantiateDelegate());
        }

        [TestMethod]
        public void Construct_QueryParser_TypeWithoutIntegerProperties_ShouldThrowQueryValidationException()
        {
            // Arrange
            var targetType = new { Name = "String" }.GetType();

            // Act
            var instantiateDelegate = () => new QueryProcessorBuilder(targetType);

            // Assert
            Assert.ThrowsException<QueryValidationException>(() => instantiateDelegate());
        }

        [TestMethod]
        public void Construct_QueryParser_TypeWithoutAtLeastOneIntegerProperty_ShouldNotThrowException()
        {
            // Arrange
            var targetType = new { Name = "String", Number = 1 }.GetType();

            // Act
            var queryBuilder = new QueryProcessorBuilder(targetType);

            // Assert
            Assert.IsNotNull(queryBuilder);
        }

        [TestMethod]
        public void Build_QueryParser_WithoutAnyRule_ShouldThrowQueryValidationException()
        {
            // Arrange
            var targetType = new { Number = 1 }.GetType();

            // Act
            var queryBuilder = new QueryProcessorBuilder(targetType);

            // Assert
            Assert.ThrowsException<QueryValidationException>(() => queryBuilder.Build());
        }
    }
}
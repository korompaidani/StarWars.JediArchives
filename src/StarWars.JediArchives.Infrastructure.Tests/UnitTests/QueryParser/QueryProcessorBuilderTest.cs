namespace StarWars.JediArchives.Infrastructure.Tests.UnitTests.QueryParser
{
    [TestClass]
    public class QueryProcessorBuilderTest
    {
        public record TestRecordWithStringProperty(string StringProperty);
        public record TestRecordWithStringAndIntProperties(string StringProperty, int IntegerProperty);

        [TestMethod]
        public void Construct_QueryParser_WithNullTargetTypeParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            Type targetType = null;

            // Act
            var instantiateDelegate = () => new QueryProcessorStatedBuilder<object>(targetType);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => instantiateDelegate());
        }

        [TestMethod]
        public void Construct_QueryParser_TypeWithoutIntegerProperties_ShouldThrowQueryValidationException()
        {
            // Arrange
            var targetType = new TestRecordWithStringProperty("String").GetType();

            // Act
            var instantiateDelegate = () => new QueryProcessorStatedBuilder<TestRecordWithStringProperty>(targetType);

            // Assert
            Assert.ThrowsException<QueryValidationException>(() => instantiateDelegate());
        }

        [TestMethod]
        public void Construct_QueryParser_TypeWithoutAtLeastOneIntegerProperty_ShouldNotThrowException()
        {
            // Arrange
            var targetType = new TestRecordWithStringAndIntProperties("String", 1).GetType();

            // Act
            var queryBuilder = new QueryProcessorStatedBuilder<TestRecordWithStringAndIntProperties>(targetType);

            // Assert
            Assert.IsNotNull(queryBuilder);
        }

        [TestMethod]
        public void Build_QueryParser_WithoutAnyRule_ShouldThrowQueryValidationException()
        {
            // Arrange
            var targetType = new TestRecordWithStringAndIntProperties("String", 1).GetType();

            // Act
            var queryBuilder = new QueryProcessorStatedBuilder<TestRecordWithStringAndIntProperties>(targetType);

            // Assert
            Assert.ThrowsException<QueryValidationException>(() => queryBuilder.Build());
        }
    }
}
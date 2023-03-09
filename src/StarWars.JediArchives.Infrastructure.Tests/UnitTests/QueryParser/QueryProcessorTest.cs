namespace StarWars.JediArchives.Infrastructure.Tests.UnitTests.QueryParser
{
    [TestClass]
    public class QueryProcessorTest
    {
        private const string IncomingAggregatedString = "startYear[gte]=10&Start_Year[lte]=10&startyear[qt]=10";
        public static IEnumerable<object[]> TestData
        {
            get
            {
                return new[]
                {
                    new object[] { @"(\[gte\])", "startYear[gte]=11", 12, 
                        (string queryParameter) => { var splits = queryParameter.Split('='); 
                            return new QueryOperation { Value = splits[1], PropertyName = splits[0], CompareTask = (dynamic number) => { 
                                return int.Parse(splits[1]) < (int)number; } }; }}
                };
            }
        }

        private HashSet<string> _propertyCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _propertyCollection = new HashSet<string> { "StartYear" };
        }

        [TestMethod]
        public void Construct_QueryParser_WithNullPropertyCollectionParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            HashSet<string> properties = null;
            Type sampleType = this.GetType();

            // Act
            var instantiateDelegate = () => new QueryProcessor(targetType: sampleType, propertyCollection: properties);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(instantiateDelegate);
        }

        [TestMethod]
        public void Construct_QueryParser_WithNullTargetTypeParameter_ShouldThrowArgumentNullException()
        {
            // Arrange
            HashSet<string> properties = new HashSet<string>();
            Type type = null;

            // Act
            var instantiateDelegate = () => new QueryProcessor(targetType: type, propertyCollection: properties);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(instantiateDelegate);
        }

        [TestMethod]
        public void Construct_QueryParser_WithTwoNullParamters_ShouldThrowException()
        {
            // Arrange
            HashSet<string> properties = null;
            Type type = null;

            // Act
            var instantiateDelegate = () => new QueryProcessor(targetType: type, propertyCollection: properties);

            // Assert
            Assert.ThrowsException<ArgumentNullException>(instantiateDelegate);
        }

        [TestMethod]
        [DynamicData(nameof(TestData))]
        public void AddProcessTest(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor(this.GetType(), _propertyCollection);

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            var queryOperation = enumeator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsTrue(evaluatedResult);
        }
    }
}

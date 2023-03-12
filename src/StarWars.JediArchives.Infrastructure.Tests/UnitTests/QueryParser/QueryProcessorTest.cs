namespace StarWars.JediArchives.Infrastructure.Tests.UnitTests.QueryParser
{
    [TestClass]
    public class QueryProcessorTest
    {
        private const string IncomingAggregatedString = "startYear[gte]=10&Start_Year[lte]=10&startyear[qt]=10";

        #region DynamicTestData
        public record D(int StartYear);

        private static Func<string, QueryOperation> GreaterComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) < (int)number; }
            };
        };

        private static Func<string, QueryOperation> LessComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) > (int)number; }
            };
        };

        private static Func<string, QueryOperation> EqualComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) == (int)number; }
            };
        };

        private static Func<string, QueryOperation> GreaterStartYearComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) < (int)number.StartYear; }
            };
        };

        public static IEnumerable<object[]> GreaterTestData => new[] { new object[] { @"(\[gte\])", "startYear[gte]=11", 12, GreaterComparerDelegate } };
        public static IEnumerable<object[]> LessTestData => new[] { new object[] { @"(\[lte\])", "startYear[lte]=12", 11, LessComparerDelegate }, };
        public static IEnumerable<object[]> EqualTestData => new[] { new object[] { @"(\[eq\])", "startYear[eq]=11", 11, EqualComparerDelegate }, };

        // False Intended due to values
        public static IEnumerable<object[]> EqualToGreaterTestData => new[] { new object[] { @"(\[eq\])", "startYear[eq]=11", 12, EqualComparerDelegate }, };
        public static IEnumerable<object[]> EqualToLessTestData => new[] { new object[] { @"(\[eq\])", "startYear[eq]=12", 11, EqualComparerDelegate }, };

        // False Intended due to filter (there is no result)
        public static IEnumerable<object[]> EqualButGreaterFilterTestData => new[] { new object[] { @"(\[gte\])", "startYear[eq]=11", 11, EqualComparerDelegate }, };
        public static IEnumerable<object[]> EqualButLessFilterTestData => new[] { new object[] { @"(\[lte\])", "startYear[eq]=11", 11, EqualComparerDelegate }, };

        // False Intended due to user defined data (there is no result)
        public static IEnumerable<object[]> EqualButGreaterInputTestData => new[] { new object[] { @"(\[eq\])", "startYear[gte]=11", 11, EqualComparerDelegate }, };
        public static IEnumerable<object[]> EqualButLessInputTestData => new[] { new object[] { @"(\[eq\])", "startYear[lte]=11", 11, EqualComparerDelegate }, };

        // More usere defined data
        public static IEnumerable<object[]> GreaterOnCollection => new[] { new object[] { @"(\[gte\])", "startYear[gte]=11", new List<D> { new D(9), new D(10), new D(11), new D(12), new D(13), new D(14) }, GreaterStartYearComparerDelegate, 3 } };

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        [DynamicData(nameof(GreaterTestData))]
        public void AddProcess_WithFilterGreaterThanGivenValue_ShouldBeTrue(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

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

        [TestMethod]
        [DynamicData(nameof(LessTestData))]
        public void AddProcess_WithFilterLessThanGivenValue_ShouldBeTrue(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

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

        [TestMethod]
        [DynamicData(nameof(EqualTestData))]
        public void AddProcess_WithFilterEqualToGivenValue_ShouldBeTrue(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

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

        [TestMethod]
        [DynamicData(nameof(EqualToGreaterTestData))]
        public void AddProcess_WithFilterEqualButGivenGreater_ShouldBeFalse(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            var queryOperation = enumeator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsFalse(evaluatedResult);
        }

        [TestMethod]
        [DynamicData(nameof(EqualToLessTestData))]
        public void AddProcess_WithFilterEqualButGivenLess_ShouldBeFalse(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            var queryOperation = enumeator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsFalse(evaluatedResult);
        }

        [TestMethod]
        [DynamicData(nameof(EqualButGreaterFilterTestData))]
        public void AddProcess_WithGreaterFilterEqual_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumeator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(EqualButLessFilterTestData))]
        public void AddProcess_WithLessFilterEqual_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumeator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(EqualButGreaterInputTestData))]
        public void AddProcess_EqualButInputGreater_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumeator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(EqualButLessInputTestData))]
        public void AddProcess_EqualButInputLess_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumeator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(GreaterOnCollection))]
        public void AddProcess_WithFilterGreaterBeingRanOnCollection_ShouldBeResultsMoreThanOneItem(string filter, string userdefinedCondition, List<D> compareByValues, Func<string, QueryOperation> process, int expectedCount)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<D>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumeator = queryProcessor.ExecutableEnumerator;
            enumeator.MoveNext();

            var queryOperation = enumeator.Current;
            var evaluatedResult = queryOperation.CompareTask;

            var resultList = compareByValues.Where(evaluatedResult);

            // Assert
            Assert.AreEqual(expectedCount, resultList.Count());
        }
    }
}

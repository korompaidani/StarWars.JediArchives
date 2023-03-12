namespace StarWars.JediArchives.Infrastructure.Tests.UnitTests.QueryParser
{
    [TestClass]
    public class QueryProcessorTest
    {
        private const string IncomingAggregatedString = "startYear[gte]=10&Start_Year[lte]=10&startyear[qt]=10";

        #region DynamicTestData
        public record R(int StartYear, int EndYear = 0);

        #region BasicNumberBasedComparers
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
        #endregion
        #region StarYearComparers
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

        private static Func<string, QueryOperation> EqualStartYearComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) == (int)number.StartYear; }
            };
        };

        private static Func<string, QueryOperation> LessStartYearComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) > (int)number.StartYear; }
            };
        };
        #endregion
        #region EndYearComparers
        private static Func<string, QueryOperation> EqualEndYearComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) == (int)number.EndYear; }
            };
        };

        private static Func<string, QueryOperation> LessEndYearComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) > (int)number.EndYear; }
            };
        };

        private static Func<string, QueryOperation> GreaterEndYearComparerDelegate = (string queryParameter) =>
        {
            var splits = queryParameter.Split('=');
            return new QueryOperation
            {
                Value = splits[1],
                PropertyName = splits[0],
                CompareTask = (dynamic number) => { return int.Parse(splits[1]) < (int)number.EndYear; }
            };
        };
        #endregion

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

        // Bigger collection
        public static IEnumerable<object[]> GreaterOnCollection => new[] { new object[] { @"(\[gte\])", "startYear[gte]=11", new List<R> { new R(9), new R(10), new R(11), new R(12), new R(13), new R(14) }, GreaterStartYearComparerDelegate, 3 } };

        // Two or more user defined queryparam on bigger collection
        public static IEnumerable<object[]> MoreParamOnCollection => new[] { 
            /* various filters, in various order */
            new object[] { @"(\[gte\])", @"(\[eq\])", "startYear[gte]=11&end_year[eq]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, GreaterStartYearComparerDelegate, EqualEndYearComparerDelegate, 2 },
            new object[] { @"(\[gte\])", @"(\[lte\])", "startYear[gte]=11&end_year[lte]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, GreaterStartYearComparerDelegate, LessEndYearComparerDelegate, 1 },
            new object[] { @"(\[eq\])", @"(\[lte\])", "startYear[eq]=1&end_year[lte]=10", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, EqualStartYearComparerDelegate, LessEndYearComparerDelegate, 3 },
            new object[] { @"(\[eq\])", @"(\[gte\])", "startYear[gte]=11&end_year[eq]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, EqualEndYearComparerDelegate, GreaterStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[gte\])", "startYear[gte]=11&end_year[lte]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, LessEndYearComparerDelegate, GreaterStartYearComparerDelegate, 1 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "startYear[eq]=1&end_year[lte]=10", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            /* url query parameters in different order */
            new object[] { @"(\[eq\])", @"(\[gte\])", "end_year[eq]=5&startYear[gte]=11", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, EqualEndYearComparerDelegate, GreaterStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[gte\])", "end_year[lte]=5&startYear[gte]=11", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, LessEndYearComparerDelegate, GreaterStartYearComparerDelegate, 1 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_year[lte]=10&startYear[eq]=1", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            /* unregistered filter given by query url by user */
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_year[lte]=10&startYear[eq]=1&end_year[ftr]=3", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[ftr\])", @"(\[eq\])", "end_year[lte]=10&startYear[eq]=1&end_year[ftr]=7", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 6), new R(1, 7), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 2 },
            /* & char replaced to % in url query */
            new object[] { @"(\[gte\])", @"(\[eq\])", "startYear[gte]=11%end_year[eq]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, GreaterStartYearComparerDelegate, EqualEndYearComparerDelegate, 2 },
            new object[] { @"(\[gte\])", @"(\[lte\])", "startYear[gte]=11%end_year[lte]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, GreaterStartYearComparerDelegate, LessEndYearComparerDelegate, 1 },
            new object[] { @"(\[eq\])", @"(\[lte\])", "startYear[eq]=1%end_year[lte]=10", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, EqualStartYearComparerDelegate, LessEndYearComparerDelegate, 3 },
            new object[] { @"(\[eq\])", @"(\[gte\])", "startYear[gte]=11%end_year[eq]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, EqualEndYearComparerDelegate, GreaterStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[gte\])", "startYear[gte]=11%end_year[lte]=5", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, LessEndYearComparerDelegate, GreaterStartYearComparerDelegate, 1 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "startYear[eq]=1%end_year[lte]=10", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[eq\])", @"(\[gte\])", "end_year[eq]=5%startYear[gte]=11", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, EqualEndYearComparerDelegate, GreaterStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[gte\])", "end_year[lte]=5%startYear[gte]=11", new List<R> { new R(9, 5), new R(10, 5), new R(11, 5), new R(12, 5), new R(13, 5), new R(14, 1) }, LessEndYearComparerDelegate, GreaterStartYearComparerDelegate, 1 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_year[lte]=10%startYear[eq]=1", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            /* more than two params in url query sometimes it is registered sometimes is not separator chars usage is various */
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_year[lte]=10%startYear[eq]=1%end_year[ftr]=3", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[ftr\])", @"(\[eq\])", "end_year[lte]=10%startYear[eq]=1%end_year[ftr]=7", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 6), new R(1, 7), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_year[lte]=10%startYear[eq]=1&end_year[ftr]=3", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[ftr\])", @"(\[eq\])", "end_year[lte]=10&startYear[eq]=1%end_year[ftr]=7", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 6), new R(1, 7), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_year[lte]=10&startYear[eq]=1%end_year[ftr]=3", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[ftr\])", @"(\[eq\])", "end_year[lte]=10%startYear[eq]=1&end_year[ftr]=7", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 6), new R(1, 7), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 2 },
            /* existing properties but in various case style in url query */
            new object[] { @"(\[lte\])", @"(\[eq\])", "endyear[lte]=10%Start_Year[eq]=1%endyear[ftr]=3", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[ftr\])", @"(\[eq\])", "End_year[lte]=10%start_yeAr[eq]=1%End_year[ftr]=7", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 6), new R(1, 7), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_Year[lte]=10%StartYear[eq]=1&end_Year[ftr]=3", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[ftr\])", @"(\[eq\])", "EndYear[lte]=10&start_Year[eq]=1%EndYear[ftr]=7", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 6), new R(1, 7), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 2 },
            new object[] { @"(\[lte\])", @"(\[eq\])", "end_yeAr[lte]=10&Start_year[eq]=1%end_yeAr[ftr]=3", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 5), new R(1, 5), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 3 },
            new object[] { @"(\[ftr\])", @"(\[eq\])", "End_Year[lte]=10%startYear[eq]=1&End_Year[ftr]=7", new List<R> { new R(1, 5), new R(1, 5), new R(1, 15), new R(12, 6), new R(1, 7), new R(14, 1) }, LessEndYearComparerDelegate, EqualStartYearComparerDelegate, 2 }
        };
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
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            var queryOperation = enumerator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsTrue(evaluatedResult);
        }

        [TestMethod]
        [DynamicData(nameof(LessTestData))]
        public void AddProcess_WithFilterLessThanGivenValue_ShouldBeTrue(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            var queryOperation = enumerator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsTrue(evaluatedResult);
        }

        [TestMethod]
        [DynamicData(nameof(EqualTestData))]
        public void AddProcess_WithFilterEqualToGivenValue_ShouldBeTrue(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            var queryOperation = enumerator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsTrue(evaluatedResult);
        }

        [TestMethod]
        [DynamicData(nameof(EqualToGreaterTestData))]
        public void AddProcess_WithFilterEqualButGivenGreater_ShouldBeFalse(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            var queryOperation = enumerator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsFalse(evaluatedResult);
        }

        [TestMethod]
        [DynamicData(nameof(EqualToLessTestData))]
        public void AddProcess_WithFilterEqualButGivenLess_ShouldBeFalse(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            var queryOperation = enumerator.Current;
            bool evaluatedResult = queryOperation.CompareTask(compareByValue);

            // Assert
            Assert.IsFalse(evaluatedResult);
        }

        [TestMethod]
        [DynamicData(nameof(EqualButGreaterFilterTestData))]
        public void AddProcess_WithGreaterFilterEqual_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(EqualButLessFilterTestData))]
        public void AddProcess_WithLessFilterEqual_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(EqualButGreaterInputTestData))]
        public void AddProcess_EqualButInputGreater_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(EqualButLessInputTestData))]
        public void AddProcess_EqualButInputLess_ShouldShouldThrowException(string filter, string userdefinedCondition, int compareByValue, Func<string, QueryOperation> process)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current, "Enumeration already finished.");
        }

        [TestMethod]
        [DynamicData(nameof(GreaterOnCollection))]
        public void AddProcess_WithFilterGreaterBeingRanOnCollection_ShouldBeResultsMoreThanOneItem(string filter, string userdefinedCondition, List<R> compareByValues, Func<string, QueryOperation> process, int expectedCount)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filter, process);
            queryProcessor.Run(userdefinedCondition);

            var enumerator = queryProcessor.ExecutableEnumerator;
            enumerator.MoveNext();

            var queryOperation = enumerator.Current.CompareTask;

            var resultList = compareByValues.Where(queryOperation);

            // Assert
            Assert.AreEqual(expectedCount, resultList.Count());
        }

        /// <summary>
        /// This test simulates most closely a real/production flow on a unit test level
        /// </summary>
        /// <param name="filterOne">On production: The filter 1 set by developer in startup.cs</param>
        /// <param name="filterTwo">On production: The filter 2 set by developer in startup.cs</param>
        /// <param name="userdefinedConditions">On production: This is the query string part in a GET http url set by user</param>
        /// <param name="compareByValues">On production: This represents the database entries for a given entity type</param>
        /// <param name="processOne">On production: This is the process expressed by developer which should fit to the filter 1</param>
        /// <param name="processTwo">On production: This is the process expressed by developer which should fit to the filter 2</param>
        /// <param name="expectedCount">In test context this is the expected result count of the result</param>
        [TestMethod]
        [DynamicData(nameof(MoreParamOnCollection))]
        public void AddProcess_WithMoreParamsMoreConditionsOnCollection_ShouldBeResultsMoreThanOneItem(string filterOne, string filterTwo, string userdefinedConditions, List<R> compareByValues, Func<string, QueryOperation> processOne, Func<string, QueryOperation> processTwo, int expectedCount)
        {
            // Arrange
            var queryProcessor = new QueryProcessor<R>();

            //// Act
            queryProcessor.AddProcess(filterOne, processOne);
            queryProcessor.AddProcess(filterTwo, processTwo);

            queryProcessor.Run(userdefinedConditions);

            var enumerator = queryProcessor.ExecutableEnumerator;

            IEnumerable<dynamic> resultList = compareByValues;

            while(enumerator.MoveNext())
            {
                var queryOperation = enumerator.Current.CompareTask;
                var tempResultList = resultList.Where(queryOperation);
                resultList = tempResultList;
            }

            // Assert
            Assert.AreEqual(expectedCount, resultList.Count());
        }
    }
}

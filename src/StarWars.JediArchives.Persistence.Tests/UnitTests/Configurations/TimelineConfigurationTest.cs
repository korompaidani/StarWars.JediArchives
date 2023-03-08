namespace StarWars.JediArchives.Persistence.Tests.UnitTests.Configurations
{
    [TestClass]
    public class TimelineConfigurationTest
    {
        private Mock<IInfrastructure<IConventionEntityTypeBuilder>> _entityTypeConfigurationMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _entityTypeConfigurationMock = new Mock<IInfrastructure<IConventionEntityTypeBuilder>>();
        }

        [TestMethod]
        public void Build_EntityTypeConfiguration_ShouldBeResultsIsRequiredSet()
        {
            // Arrange

            // Act

            // Assert
            Assert.Fail("NOT TESTED YET!");
        }
    }
}
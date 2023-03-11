namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    public interface IQueryProcessorBuilder
    {
        IRuleBuilder WithNewFilteredRule(string filter);
    }

    public interface IRuleBuilder
    {
        public IRuleBuilder WithValueFromCharacterUntilEndIndex(char fromCharacter, int endIndex);

        public IRuleBuilder WithPropertyFromIndexUntilEndCharacter(int fromIndex, char endCharacter);

        public IQueryProcessorBuilder WithExpectedComparer(Comparer expectedComparer);

        public IQueryProcessorBuilder WithExpectedOrderBy(OrderBy expectedDirection, string propertyName);
    }
}

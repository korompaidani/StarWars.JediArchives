namespace StarWars.JediArchives.Infrastructure.QueryParser
{
    public class QueryParserBuilder : IBuilder<QueryParser>
    {
        public class RuleBuilder
        {
            private string _filter;
            private char _valueFromCharacter;
            private int _valueUntilEndIndex;
            private int _propertyFromIndex;
            private char _propertyEndChar;

            public RuleBuilder AddRuleWithFilter(string filter)
            {
                _filter = filter;
                return this;
            }

            public RuleBuilder WithValueFromCharacterUntilEndIndex(char fromCharacter, int endIndex)
            {
                return this;
            }

            public RuleBuilder WithValueFromIndexUntilEndCharacter(int fromIndex, char endCharacter)
            {
                return this;
            }

            public RuleBuilder WithPropertyFromCharacterUntilEndIndex(char fromCharacter, int endIndex)
            {
                return this;
            }

            public RuleBuilder WithPropertyFromIndexUntilEndCharacter(int fromIndex, char endCharacter)
            {
                return this;
            }
        }

        private QueryParser _parser;

        public QueryParserBuilder(Type targetType)
        {
            _parser = new QueryParser(targetType);
        }

        public T Build<T>() where T : IQueryParser
        {
            return (T)(IQueryParser)_parser;
        }
    }
}

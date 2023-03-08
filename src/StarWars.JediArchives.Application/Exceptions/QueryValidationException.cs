namespace StarWars.JediArchives.Application.Exceptions
{
    public class QueryValidationException : ApplicationException
    {
        public IEnumerable<string> ValidationErrors { get; set; }

        public QueryValidationException(IEnumerable<string> errors)
        {
            ValidationErrors = new List<string>(errors);
        }
    }
}

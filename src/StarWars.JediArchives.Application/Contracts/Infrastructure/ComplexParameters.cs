namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    public class ComplexParameters : IParsable<ComplexParameters>
    {
        private static Type[] SupportedTypes = { typeof(int) };

        public bool IsTypeSupported(Type intendedType)
        {
            return SupportedTypes.Contains(intendedType);
        }

        public static ComplexParameters Parse(string s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out ComplexParameters result)
        {
            throw new NotImplementedException();
        }
    }
}

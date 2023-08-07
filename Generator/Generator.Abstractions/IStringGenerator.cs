
namespace Generator.Abstractions
{
    public interface IStringGenerator
    {
        public int GenerateString(ref Span<char> span);
    }
}

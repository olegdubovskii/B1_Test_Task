using Generator.Abstractions;

namespace Generator.Core
{
    public class FileGenerator : IFileGenerator
    {
        private static readonly IStringGenerator _generator = new StringGenerator();
        public void GenerateFiles(string filename)
        {
            Span<char> generationSpan = stackalloc char[61];
            int stringLenght = 0;
            using (var writer = new StreamWriter(filename))
            {
                for (int i = 0; i < 100000; i++)
                {
                    stringLenght = _generator.GenerateString(ref generationSpan);
                    writer.Write(generationSpan.Slice(0, stringLenght));
                }
            }
        }

    }
}
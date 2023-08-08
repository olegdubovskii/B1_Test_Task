using Generator.Abstractions;

namespace Generator.Core
{
    public class FileGenerator : IFileGenerator
    {
        private readonly IStringGenerator _generator = new StringGenerator();
        private string? _generationDirectoryPath;

        public void GenerateFiles(string generationDirectoryPath, int count)
        {
            _generationDirectoryPath = generationDirectoryPath;
            Parallel.For(0, count, GenerateFile);
        }
        private void GenerateFile(int index)
        {
            Span<char> generationSpan = stackalloc char[61];
            int stringLength = 0;
            using (var writer = new StreamWriter($"{_generationDirectoryPath}\\file{index}.txt"))
            {
                for (int i = 0; i < 99999; i++)
                {
                    stringLength = _generator.GenerateString(ref generationSpan);
                    generationSpan[stringLength++] = '\n';
                    writer.Write(generationSpan.Slice(0, stringLength));
                }
                stringLength = _generator.GenerateString(ref generationSpan);
                writer.Write(generationSpan.Slice(0, stringLength));
            }
        }
    }
}
using Generator.Abstractions;

namespace Generator.Core
{
    /// <summary>
    /// Class which generates files with help of string generator
    /// </summary>
    public class FileGenerator : IFileGenerator
    {
        private readonly IStringGenerator _generator = new StringGenerator();
        //path to the directory to which the file will be generated
        private string? _generationDirectoryPath;

        /// <summary>
        /// Executes parallel for loop for files generation
        /// </summary>
        /// <param name="generationDirectoryPath">Path to the directory to which the file will be generated</param>
        /// <param name="count">Count of files to generation</param>
        public void GenerateFiles(string generationDirectoryPath, int count)
        {
            _generationDirectoryPath = generationDirectoryPath;
            Parallel.For(0, count, GenerateFile);
        }
        
        /// <summary>
        /// Runs a loop which generating strings
        /// </summary>
        /// <param name="index"></param>
        private void GenerateFile(int index)
        {
            //span for string generation. Max lenght = 61 char because of our string standart
            Span<char> generationSpan = stackalloc char[61];
            int stringLength = 0;
            using (var writer = new StreamWriter($"{_generationDirectoryPath}\\file{index}.txt"))
            {
                for (int i = 0; i < 99999; i++)
                {
                    stringLength = _generator.GenerateString(ref generationSpan);
                    //adding a carriage jump to the end of a string
                    generationSpan[stringLength++] = '\n';
                    writer.Write(generationSpan.Slice(0, stringLength));
                }
                //last string in file doesnt need \n
                stringLength = _generator.GenerateString(ref generationSpan);
                writer.Write(generationSpan.Slice(0, stringLength));
            }
        }
    }
}
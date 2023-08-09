namespace Generator.Importer.DAL.Entities
{
    public class FileString
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string LatinSymbols { get; set; }
        public string RussianSymbols { get; set; }
        public int RandomInt { get; set; }
        public double RandomDouble { get; set; }

        public FileString(string date, string latinSymbols, string russianSymbols, int randomInt, double randomDouble)
        {
            Date = date;
            LatinSymbols = latinSymbols;
            RussianSymbols = russianSymbols;
            RandomInt = randomInt;
            RandomDouble = randomDouble;
        }
    }
}

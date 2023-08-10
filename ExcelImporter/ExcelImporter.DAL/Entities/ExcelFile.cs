
namespace ExcelImporter.DAL.Entities
{
    public class ExcelFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Sheet> Sheets { get; set; } = new List<Sheet>();
    }
}

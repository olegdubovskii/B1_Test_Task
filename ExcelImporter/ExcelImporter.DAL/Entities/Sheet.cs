using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelImporter.DAL.Entities
{
    /// <summary>
    /// Entity that represents excel sheet in database
    /// </summary>
    public class Sheet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public string DocumentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string MoneyCurrency { get; set; }
        public decimal IncomingBalanceActive { get; set; }
        public decimal IncomingBalancePassive { get; set; }
        public decimal DebitTurnover { get; set; }
        public decimal CreditTurnover { get; set; }
        public decimal OutgoingBalanceActive { get; set; }
        public decimal OutgoingBalancePassive { get; set; }
        [ForeignKey ("ExcelFile")]
        public int ExcelFileId { get; set; }
        public virtual ExcelFile ExcelFile { get; set; }
        public virtual List<SheetClass> Classes { get; set; } = new List<SheetClass>();
    }
}

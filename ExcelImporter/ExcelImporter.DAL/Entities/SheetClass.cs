using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelImporter.DAL.Entities
{
    public class SheetClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public decimal IncomingBalanceActive { get; set; }
        public decimal IncomingBalancePassive { get; set; }
        public decimal DebitTurnover { get; set; }
        public decimal CreditTurnover { get; set; }
        public decimal OutgoingBalanceActive { get; set; }
        public decimal OutgoingBalancePassive { get; set; }
        [ForeignKey("Sheet")]
        public int SheetId { get; set; }
        public Sheet Sheet { get; set; }
        public List<TotalBalanceAccount> TotalBalanceAccounts { get; set; } = new List<TotalBalanceAccount>();
    }
}

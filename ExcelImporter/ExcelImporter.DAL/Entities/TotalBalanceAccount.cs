using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelImporter.DAL.Entities
{
    public class TotalBalanceAccount
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public decimal IncomingBalanceActive { get; set; }
        public decimal IncomingBalancePassive { get; set; }
        public decimal DebitTurnover { get; set; }
        public decimal CreditTurnover { get; set; }
        public decimal OutgoingBalanceActive { get; set; }
        public decimal OutgoingBalancePassive { get; set; }
        [ForeignKey("SheetClass")]
        public int SheetClassId { get; set; }
        public SheetClass SheetClass { get; set; }
        public List<BalanceAccount> BalanceAccounts { get; set; } = new List<BalanceAccount>();
    }
}

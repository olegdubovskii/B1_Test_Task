using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExcelImporter.DAL.Entities;

namespace ExcelImporter.ApplicationInterface.Model
{
    public class CellModel : INotifyPropertyChanged
    {
        public string RowInfo { get; set; }
        public decimal IncomingBalanceActive { get; set; }
        public decimal IncomingBalancePassive { get; set; }
        public decimal DebitTurnover { get; set; }
        public decimal CreditTurnover { get; set; }
        public decimal OutgoingBalanceActive { get; set; }
        public decimal OutgoingBalancePassive { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public static CellModel CreateCellModel(BalanceAccount bankAccount)
        {
            CellModel cellModel = new CellModel();
            cellModel.RowInfo = bankAccount.Number.ToString();
            cellModel.IncomingBalanceActive = bankAccount.IncomingBalanceActive;
            cellModel.IncomingBalancePassive = bankAccount.IncomingBalancePassive;
            cellModel.DebitTurnover = bankAccount.DebitTurnover;
            cellModel.CreditTurnover = bankAccount.CreditTurnover;
            cellModel.OutgoingBalanceActive = bankAccount.OutgoingBalanceActive;
            cellModel.OutgoingBalancePassive = bankAccount.OutgoingBalancePassive;
            return cellModel;
        }
        public static CellModel CreateCellModel(TotalBalanceAccount totalBalanceAccount)
        {
            CellModel cellModel = new CellModel();
            cellModel.RowInfo = totalBalanceAccount.Number.ToString();
            cellModel.IncomingBalanceActive = totalBalanceAccount.IncomingBalanceActive;
            cellModel.IncomingBalancePassive = totalBalanceAccount.IncomingBalancePassive;
            cellModel.DebitTurnover = totalBalanceAccount.DebitTurnover;
            cellModel.CreditTurnover = totalBalanceAccount.CreditTurnover;
            cellModel.OutgoingBalanceActive = totalBalanceAccount.OutgoingBalanceActive;
            cellModel.OutgoingBalancePassive = totalBalanceAccount.OutgoingBalancePassive;
            return cellModel;
        }
        public static CellModel CreateCellModel(SheetClass sheetClass)
        {
            CellModel cellModel = new CellModel();
            cellModel.RowInfo = "ПО КЛАССУ";
            cellModel.IncomingBalanceActive = sheetClass.IncomingBalanceActive;
            cellModel.IncomingBalancePassive = sheetClass.IncomingBalancePassive;
            cellModel.DebitTurnover = sheetClass.DebitTurnover;
            cellModel.CreditTurnover = sheetClass.CreditTurnover;
            cellModel.OutgoingBalanceActive = sheetClass.OutgoingBalanceActive;
            cellModel.OutgoingBalancePassive = sheetClass.OutgoingBalancePassive;
            return cellModel;
        }

        public static CellModel CreateCellModel(Sheet sheet)
        {
            CellModel cellModel = new CellModel();
            cellModel.RowInfo = "БАЛАНС";
            cellModel.IncomingBalanceActive = sheet.IncomingBalanceActive;
            cellModel.IncomingBalancePassive = sheet.IncomingBalancePassive;
            cellModel.DebitTurnover = sheet.DebitTurnover;
            cellModel.CreditTurnover = sheet.CreditTurnover;
            cellModel.OutgoingBalanceActive = sheet.OutgoingBalanceActive;
            cellModel.OutgoingBalancePassive = sheet.OutgoingBalancePassive;
            return cellModel;
        }
    }
}

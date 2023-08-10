using IronXL;
using ExcelImporter.DAL.Entities;
using ExcelImporter.Parser.Abstractions;

namespace ExcelImporter.Parser.Core
{
    public class ExcelParser : IExcelParser
    {
        private int _currentRowNumber;
        private WorkSheet _workSheet;
        public ExcelFile ParseExcelFile(FileInfo fileInfo)
        {
            ExcelFile parsedFile = new ExcelFile();
            WorkBook workbook = WorkBook.Load(fileInfo.FullName);
            parsedFile.Name = fileInfo.FullName;
            foreach (var worksheet in workbook.WorkSheets)
            {
                _workSheet = worksheet;
                Sheet sheet = new Sheet();

                sheet.Name = _workSheet.Name;
                sheet.BankName = _workSheet["A1"].StringValue;
                sheet.DocumentName = _workSheet["A2"].StringValue;

                string[] dateTimeTokens = worksheet["A3"].StringValue.Split(' ');
                sheet.StartDate = DateTime.Parse(dateTimeTokens[dateTimeTokens.Length - 3]).Date;
                sheet.EndDate = DateTime.Parse(dateTimeTokens[dateTimeTokens.Length - 1]).Date;

                sheet.CreationDateTime = DateTime.Parse(_workSheet["A6"].StringValue);
                sheet.MoneyCurrency = _workSheet["G6"].StringValue;
                _currentRowNumber = 9;
                while (!_workSheet[$"A{_currentRowNumber}"].StringValue.Equals("БАЛАНС"))
                {
                    sheet.Classes.Add(GetNextClass());
                }
                sheet.IncomingBalanceActive = _workSheet[$"B{_currentRowNumber}"].DecimalValue;
                sheet.IncomingBalancePassive = _workSheet[$"C{_currentRowNumber}"].DecimalValue;
                sheet.DebitTurnover = _workSheet[$"D{_currentRowNumber}"].DecimalValue;
                sheet.CreditTurnover = _workSheet[$"E{_currentRowNumber}"].DecimalValue;
                sheet.OutgoingBalanceActive = _workSheet[$"F{_currentRowNumber}"].DecimalValue;
                sheet.OutgoingBalancePassive = _workSheet[$"G{_currentRowNumber}"].DecimalValue;

                parsedFile.Sheets.Add(sheet);
            }
            return parsedFile;
        }

        private SheetClass GetNextClass()
        {
            SheetClass sheetClass = new SheetClass();
            sheetClass.Name = _workSheet[$"A{_currentRowNumber}"].StringValue;
            string[] classTokens = _workSheet[$"A{_currentRowNumber}"].StringValue.Split(" ");
            sheetClass.Number = int.Parse(classTokens[2]);
            _currentRowNumber++;
            while (!_workSheet[$"A{_currentRowNumber}"].StringValue.Equals("ПО КЛАССУ"))
            {
                sheetClass.TotalBalanceAccounts.Add(GetNextTotalBalanceAccount());
            }
            sheetClass.IncomingBalanceActive = _workSheet[$"B{_currentRowNumber}"].DecimalValue;
            sheetClass.IncomingBalancePassive = _workSheet[$"C{_currentRowNumber}"].DecimalValue;
            sheetClass.DebitTurnover = _workSheet[$"D{_currentRowNumber}"].DecimalValue;
            sheetClass.CreditTurnover = _workSheet[$"E{_currentRowNumber}"].DecimalValue;
            sheetClass.OutgoingBalanceActive = _workSheet[$"F{_currentRowNumber}"].DecimalValue;
            sheetClass.OutgoingBalancePassive = _workSheet[$"G{_currentRowNumber}"].DecimalValue;
            _currentRowNumber++;
            return sheetClass;
        }

        private TotalBalanceAccount GetNextTotalBalanceAccount()
        {
            TotalBalanceAccount totalBalanceAccount = new TotalBalanceAccount();
            while (!_workSheet[$"A{_currentRowNumber}"].Style.Font.Bold)
            {
                totalBalanceAccount.BalanceAccounts.Add(GetNextBalanceAccount());
            }
            totalBalanceAccount.Number = _workSheet[$"A{_currentRowNumber}"].IntValue;
            totalBalanceAccount.IncomingBalanceActive = _workSheet[$"B{_currentRowNumber}"].DecimalValue;
            totalBalanceAccount.IncomingBalancePassive = _workSheet[$"C{_currentRowNumber}"].DecimalValue;
            totalBalanceAccount.DebitTurnover = _workSheet[$"D{_currentRowNumber}"].DecimalValue;
            totalBalanceAccount.CreditTurnover = _workSheet[$"E{_currentRowNumber}"].DecimalValue;
            totalBalanceAccount.OutgoingBalanceActive = _workSheet[$"F{_currentRowNumber}"].DecimalValue;
            totalBalanceAccount.OutgoingBalancePassive = _workSheet[$"G{_currentRowNumber}"].DecimalValue;
            _currentRowNumber++;
            return totalBalanceAccount;
        }

        private BalanceAccount GetNextBalanceAccount()
        {
            BalanceAccount balanceAccount = new BalanceAccount();
            balanceAccount.Number = int.Parse(_workSheet[$"A{_currentRowNumber}"].StringValue);
            balanceAccount.IncomingBalanceActive = _workSheet[$"B{_currentRowNumber}"].DecimalValue;
            balanceAccount.IncomingBalancePassive = _workSheet[$"C{_currentRowNumber}"].DecimalValue;
            balanceAccount.DebitTurnover = _workSheet[$"D{_currentRowNumber}"].DecimalValue;
            balanceAccount.CreditTurnover = _workSheet[$"E{_currentRowNumber}"].DecimalValue;
            balanceAccount.OutgoingBalanceActive = _workSheet[$"F{_currentRowNumber}"].DecimalValue;
            balanceAccount.OutgoingBalancePassive = _workSheet[$"G{_currentRowNumber}"].DecimalValue;
            _currentRowNumber++;
            return balanceAccount;
        }
    }
}
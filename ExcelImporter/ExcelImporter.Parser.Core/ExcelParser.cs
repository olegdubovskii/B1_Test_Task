using IronXL;
using ExcelImporter.DAL.Entities;
using ExcelImporter.Parser.Abstractions;

namespace ExcelImporter.Parser.Core
{
    /// <summary>
    /// Parses excel document and in final creates ExcelFile entity for database
    /// </summary>
    public class ExcelParser : IExcelParser
    {
        /// <summary>
        /// Parameter that indicates current string in excel doc
        /// </summary>
        private int _currentRowNumber;
        /// <summary>
        /// Presents an excel sheet in program with help of IronXL library
        /// </summary>
        private WorkSheet _workSheet;

        /// <summary>
        /// Main method that starts parsing the document and parses it sheets
        /// </summary>
        /// <param name="fileInfo">File info which contains file path for excel doc</param>
        /// <returns></returns>
        public ExcelFile ParseExcelFile(FileInfo fileInfo)
        {
            ExcelFile parsedFile = new ExcelFile();
            WorkBook workbook = WorkBook.Load(fileInfo.FullName);
            parsedFile.Name = fileInfo.FullName;
            //loop for excel sheets
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
                //loop which parses all classes and nested elements
                while (!_workSheet[$"A{_currentRowNumber}"].StringValue.Equals("БАЛАНС"))
                {
                    sheet.Classes.Add(GetNextClass());
                }
                //in this block we initialize the sheet entity with values
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

        /// <summary>
        /// Parses one sheet class from document
        /// </summary>
        /// <returns>SheetClass entity</returns>
        private SheetClass GetNextClass()
        {
            SheetClass sheetClass = new SheetClass();
            sheetClass.Name = _workSheet[$"A{_currentRowNumber}"].StringValue;
            string[] classTokens = _workSheet[$"A{_currentRowNumber}"].StringValue.Split(" ");
            sheetClass.Number = int.Parse(classTokens[2]);
            _currentRowNumber++;
            //loop which parses all subclasses and nested elements
            while (!_workSheet[$"A{_currentRowNumber}"].StringValue.Equals("ПО КЛАССУ"))
            {
                sheetClass.TotalBalanceAccounts.Add(GetNextTotalBalanceAccount());
            }
            //in this block we initialize the SheetClass entity with values
            sheetClass.IncomingBalanceActive = _workSheet[$"B{_currentRowNumber}"].DecimalValue;
            sheetClass.IncomingBalancePassive = _workSheet[$"C{_currentRowNumber}"].DecimalValue;
            sheetClass.DebitTurnover = _workSheet[$"D{_currentRowNumber}"].DecimalValue;
            sheetClass.CreditTurnover = _workSheet[$"E{_currentRowNumber}"].DecimalValue;
            sheetClass.OutgoingBalanceActive = _workSheet[$"F{_currentRowNumber}"].DecimalValue;
            sheetClass.OutgoingBalancePassive = _workSheet[$"G{_currentRowNumber}"].DecimalValue;
            _currentRowNumber++;
            return sheetClass;
        }

        /// <summary>
        /// Parses one total balance account(subclass) from excel doc
        /// </summary>
        /// <returns>TotalBalanceAccount entity</returns>
        private TotalBalanceAccount GetNextTotalBalanceAccount()
        {
            TotalBalanceAccount totalBalanceAccount = new TotalBalanceAccount();
            //loop which parses all balance accounts
            while (!_workSheet[$"A{_currentRowNumber}"].Style.Font.Bold)
            {
                totalBalanceAccount.BalanceAccounts.Add(GetNextBalanceAccount());
            }
            //in this block we initialize the TotalBalanceAccount entity with values
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

        /// <summary>
        /// Parses one balance account from excel doc
        /// </summary>
        /// <returns>Balance account entity</returns>
        private BalanceAccount GetNextBalanceAccount()
        {
            BalanceAccount balanceAccount = new BalanceAccount();
            //in this block we initialize the BalanceAccount entity with values
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
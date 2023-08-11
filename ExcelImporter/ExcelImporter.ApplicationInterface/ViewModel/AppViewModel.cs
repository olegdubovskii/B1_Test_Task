using ExcelImporter.ApplicationInterface.Model;
using ExcelImporter.DAL;
using ExcelImporter.DAL.Entities;
using ExcelImporter.Parser.Core;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ExcelImporter.ApplicationInterface.ViewModel
{
    /// <summary>
    /// Connects model and view through several collections and variables
    /// </summary>
    public class AppViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// UOW variable for database import
        /// </summary>
        private UnitOfWork _unitOfWork;
        /// <summary>
        /// Stores all files from database and files opened in the application
        /// </summary>
        private ObservableCollection<ExcelFile> _openedFiles;
        public ObservableCollection<ExcelFile> OpenedFiles
        {
            get => _openedFiles;
        }

        /// <summary>
        /// Stores all CellModel objects which represent rows from excel document
        /// </summary>
        private ObservableCollection<CellModel> _cells;
        public ObservableCollection<CellModel> ExcelCells
        {
            get => _cells;
        }

        /// <summary>
        /// Stores selected sheet to display all cells from this sheet
        /// </summary>
        private Sheet _selectedSheet;
        public Sheet SelectedSheet
        {
            get => _selectedSheet;
            set
            {
                _selectedSheet = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command for open file button
        /// </summary>
        private ButtonCommand openCommand;
        public ButtonCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new ButtonCommand(OpenFileButtonClick));
            }
        }

        public AppViewModel()
        {
            _unitOfWork = new UnitOfWork();
            _cells = new ObservableCollection<CellModel>();
            //initializes collection of files by GetItems method which takes all files from database
            _openedFiles = new ObservableCollection<ExcelFile>(_unitOfWork.ExcelFileRepository.GetItems());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// TreeView changes handler. Replaces the current sheet with a new one from View.
        /// </summary>
        /// <param name="e">Item from TreeView</param>
        public void HandleTreeViewItemSelected(RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Sheet)
            {
                _selectedSheet = (Sheet)e.NewValue;
                DisplaySheet(_selectedSheet);
            }
        }

        /// <summary>
        /// Open file event handler
        /// </summary>
        private void OpenFileButtonClick(object sender)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel file|*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog() == true)
            {
                ExcelParser excelParser = new ExcelParser();
                ExcelFile excelFile = excelParser.ParseExcelFile(new FileInfo(openFileDialog.FileName));
                //insert new file into the database
                _unitOfWork.ExcelFileRepository.InsertItem(excelFile);
                _unitOfWork.Save();
                _openedFiles.Add(excelFile);
            }
        }

        /// <summary>
        /// Displays selected sheet in View
        /// </summary>
        /// <param name="sheet">Selected sheet</param>
        private void DisplaySheet(Sheet sheet)
        {
            _cells.Clear();
            foreach (SheetClass sheetClass in sheet.Classes)
            {
                foreach(TotalBalanceAccount totalBalanceAccount in sheetClass.TotalBalanceAccounts)
                {
                    foreach(BalanceAccount balanceAccount in totalBalanceAccount.BalanceAccounts)
                    {
                        _cells.Add(CellModel.CreateCellModel(balanceAccount));
                    }
                    _cells.Add(CellModel.CreateCellModel(totalBalanceAccount));
                }
                _cells.Add(CellModel.CreateCellModel(sheetClass));
            }
            _cells.Add(CellModel.CreateCellModel(sheet));
        }
    }
}

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
    public class AppViewModel : INotifyPropertyChanged
    {
        private UnitOfWork _unitOfWork;
        private ObservableCollection<ExcelFile> _openedFiles;
        public ObservableCollection<ExcelFile> OpenedFiles
        {
            get => _openedFiles;
        }

        private ObservableCollection<CellModel> _cells;
        public ObservableCollection<CellModel> ExcelCells
        {
            get => _cells;
        }

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
            _openedFiles = new ObservableCollection<ExcelFile>(_unitOfWork.ExcelFileRepository.GetItems());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void HandleTreeViewItemSelected(RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Sheet)
            {
                _selectedSheet = (Sheet)e.NewValue;
                DisplaySheet(_selectedSheet);
            }
        }

        private void OpenFileButtonClick(object sender)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel file|*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog() == true)
            {
                ExcelParser excelParser = new ExcelParser();
                ExcelFile excelFile = excelParser.ParseExcelFile(new FileInfo(openFileDialog.FileName));
                _unitOfWork.ExcelFileRepository.InsertItem(excelFile);
                _unitOfWork.Save();
                _openedFiles.Add(excelFile);
            }
        }

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

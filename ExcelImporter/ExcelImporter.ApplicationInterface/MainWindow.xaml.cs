﻿using ExcelImporter.DAL;
using ExcelImporter.Parser.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelImporter.ApplicationInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelParser excelParser = new ExcelParser();
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                databaseContext.Add(excelParser.ParseExcelFile(new FileInfo("C:\\Users\\Oleg\\Downloads\\ОСВ для тренинга.xls")));
                databaseContext.SaveChanges();
            }
        }
    }
}

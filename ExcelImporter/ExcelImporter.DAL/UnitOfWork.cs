﻿using ExcelImporter.DAL.Abstractions;
using ExcelImporter.DAL.Repositories;

namespace ExcelImporter.DAL
{
    /// <summary>
    /// Implementation of Unit Of Work pattern for easy work with several repositories and to be sure that we use one database context
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        private DatabaseContext _databaseContext;
        private ExcelFileRepository _excelFileRepository;
        private SheetRepository _sheetRepository;
        private SheetClassRepository _sheetClassRepository;
        private TotalBalanceAccountRepository _totalBalanceAccountRepository;
        private BalanceAccountRepository _balanceAccountRepository;

        public UnitOfWork()
        {
            _databaseContext = new DatabaseContext();
        }

        public ExcelFileRepository ExcelFileRepository
        {
            get
            {
                if (_excelFileRepository == null)
                {
                    _excelFileRepository = new ExcelFileRepository(_databaseContext);
                }
                return _excelFileRepository;
            }
        }

        public SheetRepository SheetRepository
        {
            get
            {
                if (_sheetRepository == null)
                {
                    _sheetRepository = new SheetRepository(_databaseContext);
                }
                return _sheetRepository;
            }
        }

        public SheetClassRepository SheetClassRepository
        {
            get
            {
                if (_sheetClassRepository == null)
                {
                    _sheetClassRepository = new SheetClassRepository(_databaseContext);
                }
                return _sheetClassRepository;
            }
        }

        public TotalBalanceAccountRepository TotalBalanceAccountRepository
        {
            get
            {
                if (_totalBalanceAccountRepository == null)
                {
                    _totalBalanceAccountRepository = new TotalBalanceAccountRepository(_databaseContext);
                }
                return _totalBalanceAccountRepository;
            }
        }

        public BalanceAccountRepository BalanceAccountRepository
        {
            get
            {
                if(_balanceAccountRepository== null)
                {
                    _balanceAccountRepository = new BalanceAccountRepository(_databaseContext);
                }
                return _balanceAccountRepository;
            }
        }

        /// <summary>
        /// Disposing context
        /// </summary>
        public void Dispose()
        {
            _databaseContext.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Saving all commited changes in the database
        /// </summary>
        public void Save()
        {
            _databaseContext.SaveChanges();
        }
    }
}

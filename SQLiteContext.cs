using BookingServices.Interfaces;
using CommonServices.Extensions;
using SQLite.Net;
using SQLite.Net.Platform.SQLCipher.WinRT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServices.SQLite
{
    public class SQLiteContext : IDbContext
    {
        private readonly object _lockObject = new object();
        private SQLiteConnectionWithLock _dbConnection;
        private const string PragmaCipherLicense = "PRAGMA cipher_license = {0};";

        private readonly string _connectionString;
        private readonly string _password;
        private readonly string _license;

        public SQLiteContext(
            string connectionString,
            string password,
            string license)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(connectionString);
            _password = password ?? throw new ArgumentNullException(password);
            _license = license ?? throw new ArgumentNullException(license);
        }

        public Task CreateTables(IEnumerable<Type> tables)
        {
            try
            {
                return
                    Task.Run(
                        () =>
                        {
                            OpenConnection();
                            using (_dbConnection.Lock())
                            {
                                _dbConnection.RunInTransaction(
                                    () =>
                                    {
                                        foreach (var table in tables)
                                            _dbConnection.CreateTable(table);
                                    });
                            }
                        });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<int> ExecuteAsync(string sql, params object[] parameters)
        {
            try
            {
                return
                    Task.Run<int>(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                            return _dbConnection.Execute(sql, parameters);
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters)
        {
            try
            {
                return
                    Task.Run<T>(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                            return _dbConnection.ExecuteScalar<T>(sql, parameters);

                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<T> SelectFirstOrDefaultAsync<T>(string sql, params object[] parameters) where T : class
        {
            try
            {
                return
                    Task.Run<T>(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                        {
                            var result = _dbConnection.Query<T>(sql, parameters);

                            if (result != null)
                                return result.FirstOrDefault();

                            return default(T);
                        }
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<IList<T>> SelectAsync<T>(string sql, params object[] parameters) where T : class
        {
            try
            {
                return
                    Task.Run<IList<T>>(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                        {
                            var result = _dbConnection.Query<T>(sql, parameters);

                            if (result != null)
                                return result.ToList();

                            return default(IList<T>);
                        }
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<T> SelectSingleOrDefaultAsync<T>(string sql, params object[] parameters) where T : class
        {
            try
            {
                return
                    Task.Run<T>(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                        {
                            var result = _dbConnection.Query<T>(sql, parameters);

                            if (result != null)
                                return result.SingleOrDefault();

                            return default(T);
                        }
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<int> InsertAsync(object value)
        {
            try
            {
                return
                    Task.Run<int>(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                            return _dbConnection.Insert(value);
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<int> InsertAllAsync(IEnumerable value)
        {
            try
            {
                return
                    Task.Run<int>(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                            return _dbConnection.InsertAll(value);
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task UpdateAsync(object value)
        {
            try
            {
                return
                    Task.Run(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                            _dbConnection.Update(value);
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task DeleteAsync(object value)
        {
            try
            {
                return
                    Task.Run(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                            _dbConnection.Delete(value);
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Task<List<SQLiteConnection.ColumnInfo>> GetColumnDetailsAsync(string tableName)
        {
            try
            {
                return
                    Task.Run(() =>
                    {
                        OpenConnection();
                        using (_dbConnection.Lock())
                            return _dbConnection.GetTableInfo(tableName);
                    });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_dbConnection")]
        protected virtual void Dispose(bool disposing)
        {

        }

        private void OpenConnection()
        {
            lock (_lockObject)
            {
                if (_dbConnection == null || _dbConnection.Handle == null)
                {
                    try
                    {
                        _dbConnection =
                            new SQLiteConnectionWithLock(
                                new SQLitePlatformWinRT(_password),
                                new SQLiteConnectionString(_connectionString, false));

                        _dbConnection.ExecuteScalar<int>(PragmaCipherLicense.FormatString(_license));
                    }
                    catch (SQLiteException)
                    {
                        throw;
                    }
                }
            }
        }

        public void CloseConnection()
        {
            if (_dbConnection != null && _dbConnection.Handle != null)
                _dbConnection.Close();
        }

        public Task DropTables(IEnumerable<Type> tables)
        {
            try
            {
                return
                    Task.Run(
                        () =>
                        {
                            OpenConnection();
                            using (_dbConnection.Lock())
                            {
                                _dbConnection.RunInTransaction(
                                    () =>
                                    {
                                        foreach (var table in tables)
                                            _dbConnection.DropTable(table);
                                    });
                            }
                        });
            }
            catch (SQLiteException)
            {
                throw;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace BookingServices.Interfaces
{
    public interface IDbContext : IDisposable
    {
        Task<int> ExecuteAsync(string sql, params object[] parameters);
        Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters);
        Task<T> SelectFirstOrDefaultAsync<T>(string sql, params object[] parameters) where T : class, new();
        Task<T> SelectSingleOrDefaultAsync<T>(string sql, params object[] parameters) where T : class, new();
        Task<IList<T>> SelectAsync<T>(string sql, params object[] parameters) where T : class, new();
        Task<int> InsertAsync(object value);
        Task<int> InsertAllAsync(IEnumerable value);
        Task UpdateAsync(object value);
        Task DeleteAsync(object value);
        Task CreateTables(IEnumerable<Type> tables);
        void CloseConnection();
        Task<List<SQLiteConnection.ColumnInfo>> GetColumnDetailsAsync(string tableName);
        Task DropTables(IEnumerable<Type> tables);
    }
}

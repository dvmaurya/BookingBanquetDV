using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite.Net;
//using BookingServices.SQLite.DataAccessLibrary;

namespace BookingServices.SQLite
{
    public sealed class SqliteConnection 
    {
        public SqliteConnection(string v)
        {
            //string connection = @"Data Source=C:\Folder\SampleDB.db;Version=3;New=False;Compress=True;";
            //SQLiteConnection sqlite_conn = new SQLiteConnection(connection);
            //string stringQuery = "INSERT INTO _StudyInfo" + "(Param,Val)" + "Values('Name','" + snbox.Text + "')";//insert the studyinfo into Db
            ////sqlite_conn.Open();//Open the SqliteConnection
            ////var SqliteCmd = new SQLiteCommand();//Initialize the SqliteCommand
            ////SqliteCmd = sqlite_conn.CreateCommand();//Create the SqliteCommand
            //SqliteCmd.CommandText = stringQuery;//Assigning the query to CommandText
            //SqliteCmd.ExecuteNonQuery();//Execute the SqliteCommand
            //sqlite_conn.Close();//Close the SqliteConnection
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public Task CreateTables(IEnumerable<Type> tables)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task DropTables(IEnumerable<Type> tables)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteAsync(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<List<SQLiteConnection.ColumnInfo>> GetColumnDetailsAsync(string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAllAsync(IEnumerable value)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(object value)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> SelectAsync<T>(string sql, params object[] parameters) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> SelectFirstOrDefaultAsync<T>(string sql, params object[] parameters) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> SelectSingleOrDefaultAsync<T>(string sql, params object[] parameters) where T : class
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(object value)
        {
            throw new NotImplementedException();
        }
    }
}

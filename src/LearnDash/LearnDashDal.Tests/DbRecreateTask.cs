using System;
using System.Data;
using System.Data.SqlClient;
using LearnDash.Dal.NHibernate;
using NUnit.Framework;

namespace LearnDashDal.Tests
{
    public static class DbHelper
    {
        private const string ConnString = "Data Source=localhost\\SQLEXPRESS;Integrated Security=SSPI";
        private const string DbName = "LearnDash";

        public static void CreateDatabase()
        {
            var connection = new SqlConnection(ConnString);
            var commandText = string.Format("CREATE DATABASE {0}", DbName);
            var createDbCommand = new SqlCommand(commandText, connection);

            try
            {
                connection.Open();
                createDbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static void DropDatabase()
        {
            var connection = new SqlConnection(ConnString);
            var commandText = string.Format("alter database {0} set single_user with rollback immediate \n" +
                                            "DROP DATABASE {0}", DbName);
            var createDbCommand = new SqlCommand(commandText, connection);

            try
            {
                connection.Open();
                createDbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public static bool IfDbExists()
        {
            var connection = new SqlConnection(ConnString);
            var commandText = string.Format("select * from master.dbo.sysdatabases where name=\'{0}\'", DbName);
            var createDbCommand = new SqlCommand(commandText, connection);

            try
            {
                connection.Open();
                var reader = createDbCommand.ExecuteReader();
                return reader.HasRows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }

    [TestFixture]
    class DbRecreateTask
    {
        [Test]
        public void Reset_and_create_new_database_with_new_tables()
        {
            if (DbHelper.IfDbExists())
            {
                DbHelper.DropDatabase();
            }

            DbHelper.CreateDatabase();

            DataAccess.ResetDb();
        }
    }
}

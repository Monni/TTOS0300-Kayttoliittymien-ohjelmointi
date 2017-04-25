using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace WpfApp2
{
    public class MemberDatabaseConnection
    {

        //public MySqlConnection connect;
        public SqlConnection connection;

        private bool connectionUP;

        public bool CheckConnection()
        {
            if (connectionUP)
                return true;
            else
                return false;
        }

        public MemberDatabaseConnection()
        {
            connectionUP = false;
        }

        public void ConnectDatabase()
        {
            try
            {
                var entityConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TietokantaEntityContainer"].ConnectionString;
                var ConnectionBuilder = new EntityConnectionStringBuilder(entityConnection);
                connection = new SqlConnection(ConnectionBuilder.ProviderConnectionString);

                connection.Open();
                connectionUP = true;
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

        }
    }
}

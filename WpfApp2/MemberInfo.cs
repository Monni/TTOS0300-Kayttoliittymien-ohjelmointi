using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Windows;
using System.Diagnostics;

namespace WpfApp2
{
    public class MemberInfo
    {
        private string entityConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TietokantaEntityContainer"].ConnectionString;
        private EntityConnectionStringBuilder ConnectionBuilder;
        private SqlConnection connection;
        private Boolean isConnected = false;


    public MemberInfo()
        {
            this.entityConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TietokantaEntityContainer"].ConnectionString;
            this.ConnectionBuilder = new EntityConnectionStringBuilder(entityConnection);
            this.connection = new SqlConnection(ConnectionBuilder.ProviderConnectionString);
    }


    static string updateQuery;
        
        public MemberInfo(SqlConnection connection, Boolean isConnected)
        {
            this.connection = connection;
            this.isConnected = isConnected;
            updateQuery = "Update " + "jasenetSet" + " SET hetu=@hetu, snimi=@snimi, enimet=@enimet, osoite=@osoite, postinumero=@postinumero, postitoimipaikka=@postitoimipaikka, puhelinnumero=@puhelinnumero, email=@email, liittymispv=@liittymis_pvm, eroamispv=@eroamis_pvm, tilinumero=@tilinumero";

        }


        public bool UpdateMemberInfo(string[] info, int avain)
        {
            bool success = false;

            var ConnectionBuilder = new EntityConnectionStringBuilder(this.entityConnection);
            SqlConnection connection = new SqlConnection(ConnectionBuilder.ProviderConnectionString);
            connection.Open();

            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "UPDATE jasenetSet SET hetu=@hetu, snimi=@snimi, enimet=@enimet, osoite=@osoite, postinumero=@postinumero, postitoimipaikka=@postitoimipaikka, puhelinnumero=@puhelinnumero, email=@email, liittymispv=@liittymispv, maksettu=@maksettu, lisatietoa=@lisatietoa WHERE avain=@avain";
                    command.Parameters.AddWithValue("@hetu", info[0]);
                    command.Parameters.AddWithValue("@snimi", info[1]);
                    command.Parameters.AddWithValue("@enimet", info[2]);
                    command.Parameters.AddWithValue("@osoite", info[3]);
                    command.Parameters.AddWithValue("@postinumero", int.Parse(info[4]));
                    command.Parameters.AddWithValue("@postitoimipaikka", info[5]);
                    command.Parameters.AddWithValue("@puhelinnumero", info[6]);
                    command.Parameters.AddWithValue("@email", info[7]);
                    DateTime now = DateTime.Now.Date;
                    command.Parameters.AddWithValue("@liittymispv", now);
                    command.Parameters.AddWithValue("@maksettu", info[9]);
                    command.Parameters.AddWithValue("@lisatietoa", info[10]);
                    // Insert ID which is used to update values
                    command.Parameters.AddWithValue("@avain", avain);

                    try
                    {
                        command.ExecuteNonQuery();
                    } catch (SqlException)
                    {
                        throw;
                    } finally
                    {
                        success = true;
                        command.Dispose();                  
                    }
                }
            }
            return success;
        }


        public Boolean createNewMember(string[] info, Boolean isConnected)
        {
            Boolean success = false;
            this.isConnected = isConnected;

            var ConnectionBuilder = new EntityConnectionStringBuilder(this.entityConnection);
            SqlConnection connection = new SqlConnection(ConnectionBuilder.ProviderConnectionString);
            connection.Open();

            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "INSERT INTO jasenetSet (hetu, snimi, enimet, osoite, postinumero, postitoimipaikka, puhelinnumero, email, liittymispv, maksettu, lisatietoa) VALUES (@hetu, @snimi, @enimet, @osoite, @postinumero, @postitoimipaikka, @puhelinnumero, @email, @liittymispv, @maksettu, @lisatietoa)";
                    command.Parameters.AddWithValue("@hetu", info[0]);
                    command.Parameters.AddWithValue("@snimi", info[1]);
                    command.Parameters.AddWithValue("@enimet", info[2]);
                    command.Parameters.AddWithValue("@osoite", info[3]);
                    command.Parameters.AddWithValue("@postinumero", int.Parse(info[4]));
                    command.Parameters.AddWithValue("@postitoimipaikka", info[5]);
                    command.Parameters.AddWithValue("@puhelinnumero", info[6]);
                    command.Parameters.AddWithValue("@email", info[7]);
                    DateTime now = DateTime.Now.Date;
                    command.Parameters.AddWithValue("@liittymispv", now);
                    command.Parameters.AddWithValue("@maksettu", info[9]);
                    command.Parameters.AddWithValue("@lisatietoa", info[10]);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        success = true;
                        command.Dispose();
                    }
                }
            }
            return success;
        }


        public void removeMember(int avain)
        {
            var ConnectionBuilder = new EntityConnectionStringBuilder(this.entityConnection);
            SqlConnection connection = new SqlConnection(ConnectionBuilder.ProviderConnectionString);
            connection.Open();

            int recordsAffected = 0;
            //this.isConnected = isConnected;

            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "DELETE FROM jasenetSet WHERE avain=@avain";
                    command.Parameters.AddWithValue("@avain", avain);

                    try
                    {
                        if (!this.isConnected)
                        {
                            connection.Open();
                        }
                        recordsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        command.Dispose();
                        //connection.Close();
                    }
                }
            }
        }


        public List<jasenet> GetMembersByPostalcode(int postalcode)
        {
            List<jasenet> jasenList = new List<jasenet>();

            var ConnectionBuilder = new EntityConnectionStringBuilder(this.entityConnection);
            SqlConnection connection = new SqlConnection(ConnectionBuilder.ProviderConnectionString);
            connection.Open();

            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM jasenetSet WHERE postinumero=@postinumero";
                    command.Parameters.AddWithValue("@postinumero", postalcode);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int index = 0;
                            jasenet jasen = new jasenet();
                            jasen.avain = reader.GetInt32(index++);
                            jasen.hetu = reader.GetString(index++);
                            jasen.snimi = reader.GetString(index++);
                            jasen.enimet = reader.GetString(index++);
                            jasen.osoite = reader.GetString(index++);
                            jasen.postinumero = reader.GetInt32(index++);
                            jasen.postitoimipaikka = reader.GetString(index++);
                            jasen.puhelinnumero = reader.GetString(index++);
                            jasen.email = reader.GetString(index++);
                            jasen.liittymispv = reader.GetDateTime(index++);
                            jasen.maksettu= (reader[index++] as int?) ?? 0;
                            jasen.lisatietoa = reader.GetString(index++);

                            jasenList.Add(jasen);

                        }
                    }
                        command.Dispose();
                }
            }
            return jasenList;
        }


        public List<jasenet> getUnpaidMembers(int vuosimaksu)
        {
            List<jasenet> jasenList = new List<jasenet>();

            var ConnectionBuilder = new EntityConnectionStringBuilder(this.entityConnection);
            SqlConnection connection = new SqlConnection(ConnectionBuilder.ProviderConnectionString);
            connection.Open();

            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM jasenetSet WHERE maksettu!=@vuosimaksu";
                    command.Parameters.AddWithValue("@vuosimaksu", vuosimaksu);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int index = 0;
                            jasenet jasen = new jasenet();
                            jasen.avain = reader.GetInt32(index++);
                            jasen.hetu = reader.GetString(index++);
                            jasen.snimi = reader.GetString(index++);
                            jasen.enimet = reader.GetString(index++);
                            jasen.osoite = reader.GetString(index++);
                            jasen.postinumero = reader.GetInt32(index++);
                            jasen.postitoimipaikka = reader.GetString(index++);
                            jasen.puhelinnumero = reader.GetString(index++);
                            jasen.email = reader.GetString(index++);
                            jasen.liittymispv = reader.GetDateTime(index++);
                            jasen.maksettu = (reader[index++] as int?) ?? 0;
                            jasen.lisatietoa = reader.GetString(index++);

                            jasenList.Add(jasen);

                        }
                    }
                    command.Dispose();
                    //connection.Close();
                }
            }


            return jasenList;
        }


    }
}

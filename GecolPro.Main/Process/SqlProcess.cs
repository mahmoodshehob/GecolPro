using Microsoft.Data.SqlClient;
using static GecolPro.Main.Process.TcpSocketClient;

namespace GecolPro.Main.Process
{

    public class SqlProcess
    {
        private static string connectionString = @"Data Source=WIN-UVCBHDHF1CT\SQLSHEHOB;Initial Catalog=QRDB;Integrated Security=True;";


        public static async void CheckValied(string Msisdn, string QrCode, string ChannanName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await Task.Run(() =>
                {
                    CheckSub(Msisdn, QrCode, ChannanName);
                });
            }
        }

        private static async void CheckSub(string Msisdn, string QrCode, string ChannanName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await Task.Run(() =>
                {
                    connection.Open();

                    string querySubscriber =
                       "SELECT COUNT(Id) FROM [QRDB].[dbo].[SubscriptionReports]  where [Msisdn]='" + Msisdn + "'";

                    using (SqlCommand command = new SqlCommand(querySubscriber, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int qrid = reader.GetInt32(0);
                                //var qrid = reader.GetString(0).ToString();
                                // Your code to do something with the qrid value goes here.
                                if (qrid == 0)
                                {
                                    CheckQr(Msisdn, QrCode, ChannanName);
                                }
                                else
                                {
                                    CountFlag(Msisdn);
                                }
                            }
                        }
                    }
                });
            }
        }

        private static async void CheckQr(string Msisdn, string QrCode, string ChannanName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await Task.Run(() =>
                {
                    connection.Open();

                    string queryQr =
                     "SELECT TOP 1 [QRCODE]   FROM [QRDB].[dbo].[TBL_QR]   where [QRCODE]='" + QrCode + "' and [QRISACTIVE]='1'";

                    using (SqlCommand command = new SqlCommand(queryQr, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var qrid = reader.HasRows;
                                //var qrid = reader.GetString(0).ToString();
                                // Your code to do something with the qrid value goes here.
                                if (qrid == true)
                                {
                                    TcpSucket.CreateSocket("000000000007",Msisdn);
                                }
                            }
                        }
                    }
                });
            }
        }

        public static async void InsertSub(string Msisdn, string QrCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await Task.Run(() =>
                {
                    connection.Open();

                    string InsertSql =
                    "INSERT INTO [QRDB].[dbo].[SubscriptionReports] ([Msisdn],[QrCode],[FirstOrderTime],[Flag]) " +
                    " VALUES" +
                    "('" + Msisdn + "','" + QrCode + "','" + DateTime.Now.ToString() + "',0)";

                    using (SqlCommand command = new SqlCommand(InsertSql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            var grid = reader.RecordsAffected;
                        }
                    }
                });

            }
        }

        public static async void InsertBraif(string Msisdn, string ChannanName, string QrCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await Task.Run(() =>
                {
                    connection.Open();

                    string InsertSql =
                    "INSERT INTO [QRDB].[dbo].[Briaf]  ([Msisdn],[Channel],[DelivaryTime],[Message])" +
                    " VALUES" +
                    "('" + Msisdn + "','" + ChannanName + "','" + DateTime.Now.ToString() + "','" + QrCode + "')";

                    using (SqlCommand command = new SqlCommand(InsertSql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            var grid = reader.RecordsAffected;
                        }
                    }
                });

            }
        }

        private static async void CountFlag(string Msisdn)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await Task.Run(() =>
                {
                    connection.Open();

                    string UpdateFlag = "UPDATE [QRDB].[dbo].[SubscriptionReports] SET[Flag] = [Flag] + 1 WHERE[Msisdn] = '" + Msisdn + "'";

                    using (SqlCommand command = new SqlCommand(UpdateFlag, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            var grid = reader.RecordsAffected;
                        }
                    }
                });
            }
        }

    }
}



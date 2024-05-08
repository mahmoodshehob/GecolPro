using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibrary.Services.TcpSocketClient
{
    
        //static IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("host.contoso.com");
        //IPAddress ipAddress = ipHostInfo.AddressList[0];
        public class TcpSucket
    {
        // Set the IP address and port number of the server to connect to
        private static string ipAddress = "172.16.198.11";
        private static int port = 7901;


        public static async Task Interface()
        {

        }





        private static string StateOfReponce(string Rsp)
        {
            int startIndex = Rsp.IndexOf('=') + 1;
            int endIndex = Rsp.IndexOf('}');
            string result = Rsp.Substring(startIndex, endIndex - startIndex);

            if (result == "0")
            {
                return result;
            }
            else
            {
                return "F";
            }
        }



        public static async Task Create()
        {
            await CreateSocket("000000000007", "218947776156");
        }

        public static async Task Create(string SN, string Msisdn)
        {
            //await Task.Run(async () =>
            //{
            await CreateSocket(SN, Msisdn);
            //});
        }

        public static async Task CreateSocket(string SN, string Msisdn)
        {
            if (SN.Length == 12)
            {
                await Task.Run(async () =>
                {

                    try
                    {

                        Msisdn = Regex.Replace(Msisdn, @"^.{3}", "");

                        string message = "{6001" + SN + "0000003840=" + Msisdn + "}";

                        // Create a TCP socket and connect to the server
                        TcpClient client = new TcpClient(ipAddress, port);

                        // Get the network stream used for sending and receiving data
                        NetworkStream stream = client.GetStream();

                        // Convert the message string to a byte array
                        byte[] data = Encoding.ASCII.GetBytes(message);

                        // Send the message to the server
                        stream.Write(data, 0, data.Length);

                        // Receive the response from the server
                        data = new byte[256];
                        int bytes = stream.Read(data, 0, data.Length);
                        string response = Encoding.ASCII.GetString(data, 0, bytes);

                        // Display the response from the server
                        Console.WriteLine(bytes);
                        Console.WriteLine("Server response: {0}", response);

                        // Close the connection
                        stream.Close();
                        client.Close();

                        if (StateOfReponce(response) == "0")
                        {

                            // await Logger.Logger.TcpLog("218" + Msisdn + ",Success recive Socket");

                            //Console.WriteLine("Success");
                            //SmppTest.SubmitSMS("218" + Msisdn, "Success recive Socket");

                        }
                        else
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e);
                        Console.ReadLine();
                    }
                });
            }
        }
    }
}
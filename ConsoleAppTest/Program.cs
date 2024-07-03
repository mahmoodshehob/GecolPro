
using ClassLibrary.GecolSystem;
using ClassLibrary.GecolSystem.Models;
using ClassLibrary.Services;
using System;

namespace ConsoleAppTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GecolTest();
        }

        static void GecolTest()
        {
            Loggers loggers = new Loggers();

            var a = new CommonParameters();

            Console.WriteLine(a.Username);
            Console.WriteLine(a.Password);
            Console.WriteLine(a.Url);
            Console.WriteLine(a.EANDeviceID);
            Console.WriteLine(a.GenericDeviceID);
            Console.WriteLine(a.UniqueNumber);
            Console.WriteLine(a.DateTimeReq);



            var jjjjj = new ClassLibrary.GecolSystem.Models.AuthCred();
            var kkkkk = new ClassLibrary.GecolSystem.Models.CommonParameters();

            Console.ReadLine();
        }

        static void DCBTest()
        {


            var a = new CommonParameters();

            Console.WriteLine(a.Username);
            Console.WriteLine(a.Password);
            Console.WriteLine(a.Url);
            Console.WriteLine(a.EANDeviceID);
            Console.WriteLine(a.GenericDeviceID);
            Console.WriteLine(a.UniqueNumber);
            Console.WriteLine(a.DateTimeReq);



            var jjjjj = new ClassLibrary.GecolSystem.Models.AuthCred();
            var kkkkk = new ClassLibrary.GecolSystem.Models.CommonParameters();

            Console.ReadLine();
        }

    }
}
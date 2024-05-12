// See https://aka.ms/new-console-template for more information


using ClassLibrary.GecolSystem_Update;
using ClassLibrary.GecolSystem_Update.Models;

IGecolServices services = new GecolServices();

var login = services.LoginReqOp();

Console.ReadLine();
Console.ReadLine();

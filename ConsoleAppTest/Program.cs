
using ClassLibrary.GecolSystem_Update;
using ClassLibrary.GecolSystem_Update.Models;
using ClassLibrary.Services;


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




//loggers.LogInfoAsync();



Console.ReadLine();

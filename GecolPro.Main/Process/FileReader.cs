using IniParser.Model;
using IniParser;
using System.Drawing;
using System;

namespace GecolPro.Main.Process
{
    public class FileReader
    {

        private static string folderName = "Configuration_Sho";
        private static string ProjDirectory = Directory.GetCurrentDirectory();
        private static string currentDirectory = Path.Combine(ProjDirectory, folderName);

        private enum Language
        {
            English,
            Arabic
        }





        //public static string Read(string fileName)
        //{
        //    fileName = fileName+".txt";
        //    string content = string.Empty;
        //    string filePath = Path.Combine(currentDirectory, fileName);

        //    try
        //    {
        //        content = File.ReadAllText(filePath);
        //    }
        //    catch (IOException e)
        //    {
        //        // Handle any exceptions that occur during file reading
        //        Console.WriteLine($"An error occurred while reading the file: {e.Message}");
        //    }

        //    return content;
        //}



        //public static List<string> ReadIniPhoneList()
        //{
        //    List<string> myList = new List<string>();
        //    string iniFileName = "Configration.ini";
        //    string iniFilePath = Path.Combine(currentDirectory, iniFileName);

        //    if (!File.Exists(iniFilePath))
        //    {
        //        CreateFiles.CreateIniConfigration();
        //    }
        //    else
        //    {
        //        FileIniDataParser parser = new FileIniDataParser();
        //        IniData data = parser.ReadFile(iniFilePath);

        //        string sectionName = "SectionName";
        //        string keyName = "PhoneList";
        //        string value = data[sectionName][keyName];
        //        myList = value.Split(',').ToList();
        //    }

        //    return myList;
        //}



        //public static (int,int, int, int) ReadIniTimeOfTask()
        //{
        //    int FixedHourMax = 23;
        //    int FixedHourMin = 0;
        //    int DayMax = 6;
        //    int DayMin = 1;

        //    string iniFileName = "Configration.ini";
        //    string iniFilePath = Path.Combine(currentDirectory, iniFileName);

        //    if (!File.Exists(iniFilePath))
        //    {
        //        CreateFiles.CreateIniConfigration();
        //    }
        //    else
        //    {
        //        FileIniDataParser parser = new FileIniDataParser();
        //        IniData data = parser.ReadFile(iniFilePath);

        //        string sectionName = "SectionName";

        //        FixedHourMax = int.Parse(data[sectionName]["FixedHourMax"]);
        //        FixedHourMin = int.Parse(data[sectionName]["FixedHourMin"]);
        //        DayMax = int.Parse(data[sectionName]["DayMax"]);
        //        DayMin = int.Parse(data[sectionName]["DayMin"]);

        //    }

        //    return (FixedHourMax, FixedHourMin, DayMax, DayMin);
        //}


        public static string MenuCont_En(string UssdKey ,string MenuID )
        {

            //string currentDirectory = "~/UssdMenu/";
            currentDirectory = ProjDirectory;



            string iniFileName = "UssdMenu\\"+ UssdKey + "\\" + Language.English.ToString() + ".ini";
            string iniFilePath = Path.Combine(currentDirectory, iniFileName);

            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFilePath);

            string sectionName = MenuID;

            string MenuContent = data[Language.English.ToString()][MenuID];

            return MenuContent;
        }

        public static string MenuCont_Ar(string UssdKey, string MenuID)
        {

            //string currentDirectory = "~/UssdMenu/";
            currentDirectory = ProjDirectory;



            string iniFileName = UssdKey + "/" + Language.Arabic.ToString() + ".ini";
            string iniFilePath = Path.Combine(currentDirectory, iniFileName);

            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFilePath);

            string sectionName = MenuID;

            string MenuContent = data[Language.English.ToString()][MenuID];

            return MenuContent;
        }
    }
}
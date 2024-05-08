using static System.Net.Mime.MediaTypeNames;
using System;
using System.IO;
using IniParser;
using IniParser.Model;


namespace GecolPro.Main.Process
{
    public class CreateFiles
    {

        private static string LoggerFolder = "Configuration_Sho";
        private static string folderName = "Configuration_Sho";
        private static string ProjDirectory = Directory.GetCurrentDirectory();
        private static string currentDirectory = Path.Combine(ProjDirectory, folderName);

        private enum Language
        {
            English,
            Arabic 
        }

        class MessageFileModel
        {
            public string FileMessage { get; set; }
            public string Message { get; set; }
        }
        /// <summary>
        /// Message1
        /// </summary>
        private static string FileMessage1 = "Message1" +".txt";
        private static string Message1 = @"
أربح يوم مجاني في خدمة ستاديو ثم 0.5د.ل لكل 3 ايام
للاشتراك أرسل 1 الى 18100 أو من www.stadio.ly
 وتابع اخبار الكورة الليبية والعالمية ⚽
أرسل 1
";
        /// <summary>
        /// Message2
        /// </summary>
        private static string FileMessage2 = "Message2" + ".txt";
        private static string Message2 = @"
أربح يوم مجاني في خدمة ستاديو ثم 0.5د.ل لكل 3 ايام
للاشتراك أرسل 1 الى 18100 أو من www.stadio.ly
 وتابع اخبار الكورة الليبية والعالمية ⚽
أرسل 1
";
        /// <summary>
        /// Message3
        /// </summary>
        private static string FileMessage3 = "Message3" + ".txt";
        private static string Message3 = @"
أربح يوم مجاني في خدمة ستاديو ثم 0.5د.ل لكل 3 ايام
للاشتراك أرسل 1 الى 18100 أو من www.stadio.ly
 وتابع اخبار الكورة الليبية والعالمية ⚽
أرسل 1
";
        private static string FileMessage4 = "ContralFdaMt" + ".txt";
        private static string Message4 = @"0";


        private static List<MessageFileModel> messageFileModel = new List<MessageFileModel>()
        {
            new MessageFileModel {
                FileMessage = FileMessage1,
                Message = Message1,
            },
            new MessageFileModel {
                FileMessage = FileMessage2,
                Message = Message2,
            },
            new MessageFileModel {
                FileMessage = FileMessage3,
                Message = Message3,
            },
             new MessageFileModel {
                FileMessage = FileMessage4,
                Message = Message4,
            }

        };


        public static async Task Create()
        {
            await Task.Run(() => {

                try
                {
                    // Check if the directory already exists
                    if (!Directory.Exists(currentDirectory))
                    {
                        // Create the directory
                        Directory.CreateDirectory(currentDirectory);
                    }

                }
                catch (Exception ex)
                {
                    
                }






                try
                {
                    foreach (var messageFileModel in messageFileModel)
                    {
                        string FileMessage = messageFileModel.FileMessage;
                        FileMessage = Path.Combine(currentDirectory, FileMessage);
                        if (!File.Exists(FileMessage))
                        {
                            // Create a FileStream object to append to the file.
                            using (FileStream fs = new FileStream(FileMessage, FileMode.Append))
                            {
                                // Create a StreamWriter object to write to the file.
                                using (StreamWriter sw = new StreamWriter(fs))
                                {
                                    // Write the string to the file.
                                    sw.WriteLine(messageFileModel.Message);
                                }
                            }
                        }
                        else
                        {
                            //// Create a FileStream object to create the file.
                            //using (FileStream fs = new FileStream(FileMessage, FileMode.Create))
                            //{
                            //    // Create a StreamWriter object to write to the file.
                            //    using (StreamWriter sw = new StreamWriter(fs))
                            //    {
                            //        // Write the string to the file.
                            //        sw.WriteLine(messageFileModel.Message);
                            //    }
                            //}
                        }
                    }
                }
                catch (Exception ex) 
                { 
                
                }
            });
        }

        public static void CreateIniConfigration()
        {
            //currentDirectory = "~/UssdMenu/";
            currentDirectory = ProjDirectory;
            string KeysDir = currentDirectory + "\\UssdMenu\\";

            string UssdKey = "558";
            string iniFileName_En = UssdKey + "\\" + Language.English.ToString()   + ".ini";
            string iniFileName_Ar = UssdKey + "\\" + Language.Arabic.ToString()    + ".ini";


            //string iniFilePath_En = Path.Combine(currentDirectory, iniFileName_En);
            //string iniFilePath_Ar = Path.Combine(currentDirectory, iniFileName_Ar);


            string iniFilePath_En = Path.Combine(KeysDir, iniFileName_En);
            string iniFilePath_Ar = Path.Combine(KeysDir, iniFileName_Ar);

        
            if (!Directory.Exists(KeysDir))
            {
                // Create the directory
                Directory.CreateDirectory(KeysDir);
            }

            if (!Directory.Exists(KeysDir+ UssdKey))
            {
                // Create the directory
                Directory.CreateDirectory(KeysDir+ UssdKey);
            }


            if (!File.Exists(iniFilePath_En))
            {
                try
                {
                    string Lang = Language.English.ToString();

                    FileIniDataParser parser = new FileIniDataParser();

                    IniData data = new IniData();

                    // Add sections and keys
                    data.Sections.AddSection(Lang);

                    //List<string> phoneList = new List<string> { "218947776156", "218928525002"};
                    //data["SectionName"].AddKey("PhoneList", string.Join(",", phoneList));


                    data[Lang].AddKey("*558", "1.option1\n2.option2\n3.option3");
                    
                    data[Lang].AddKey("*1", "1.option11\n2.option12\n3.option13");
                    data[Lang].AddKey("*2", "1.option21\n2.option22\n3.option23");
                    data[Lang].AddKey("*3", "1.option31\n2.option32\n3.option33");

                    data[Lang].AddKey("*1*1", "1.option111\n2.option112\n3.option113");
                    data[Lang].AddKey("*1*2", "1.option121\n2.option122\n3.option123");
                    data[Lang].AddKey("*1*3", "1.option131\n2.option132\n3.option133");
                    
                    data[Lang].AddKey("*2*1", "1.option211\n2.option212\n3.option213");
                    data[Lang].AddKey("*2*2", "1.option221\n2.option222\n3.option223");
                    data[Lang].AddKey("*2*3", "1.option231\n2.option232\n3.option233");

                    data[Lang].AddKey("*3*1", "1.option311\n2.option312\n3.option313");
                    data[Lang].AddKey("*3*2", "1.option321\n2.option322\n3.option323");
                    data[Lang].AddKey("*3*3", "1.option331\n2.option332\n3.option333");

                    parser.WriteFile(iniFilePath_En, data);
                }
                catch (Exception ex) 
                {
                    string exMessage = ex.Message;
                }
            }

            if (!File.Exists(iniFilePath_Ar))
            {
                try
                {

                    string Lang = Language.Arabic.ToString();

                    FileIniDataParser parser = new FileIniDataParser();

                    IniData data = new IniData();

                    // Add sections and keys
                    data.Sections.AddSection(Lang);

                    //List<string> phoneList = new List<string> { "218947776156", "218928525002"};
                    //data["SectionName"].AddKey("PhoneList", string.Join(",", phoneList));


                    data[Lang].AddKey("*558", "1.option1\n2.option2\n3.option3");

                    data[Lang].AddKey("*1", "1.option11\n2.option12\n3.option13");
                    data[Lang].AddKey("*2", "1.option21\n2.option22\n3.option23");
                    data[Lang].AddKey("*3", "1.option31\n2.option32\n3.option33");

                    data[Lang].AddKey("*1*1", "1.option111\n2.option112\n3.option113");
                    data[Lang].AddKey("*1*2", "1.option121\n2.option122\n3.option123");
                    data[Lang].AddKey("*1*3", "1.option131\n2.option132\n3.option133");

                    data[Lang].AddKey("*2*1", "1.option211\n2.option212\n3.option213");
                    data[Lang].AddKey("*2*2", "1.option221\n2.option222\n3.option223");
                    data[Lang].AddKey("*2*3", "1.option231\n2.option232\n3.option233");

                    data[Lang].AddKey("*3*1", "1.option311\n2.option312\n3.option313");
                    data[Lang].AddKey("*3*2", "1.option321\n2.option322\n3.option323");
                    data[Lang].AddKey("*3*3", "1.option331\n2.option332\n3.option333");

                    parser.WriteFile(iniFilePath_Ar, data);
                }
                catch (Exception ex)
                {
                    string exMessage = ex.Message;
                }
            }

        }

            

            

    }
}

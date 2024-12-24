using System.Runtime.InteropServices;


using IniParser.Model;
using IniParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

namespace GecolPro.Services
{
    partial class IniFiles
    {
        private string baseDirectory;
        private string iniFileName = "Configration.ini";

        public IniFiles()
        {
            CreateDirectoryBasedOnSystem();
        }

        private void CreateDirectoryBasedOnSystem()
        {
            // Check operating system
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //@"C:\Configrations";
                baseDirectory = Path.Combine(desktop, "Configrations");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                baseDirectory = $"/home/{Environment.UserName}/Configrations";
            }
            else
            {
                throw new NotSupportedException("OS not supported.");
            }

            // Create the base directory if it doesn't exist
            CreateDirectoryIfNotExists(baseDirectory);


            CreateFileIfNotExists(baseDirectory);

        }

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void CreateFileIfNotExists(string DirectoryPath)
        {
            string iniFilePath = Path.Combine(baseDirectory, iniFileName);

            if (!File.Exists(iniFilePath))
            {
                try
                {

                    FileIniDataParser parser = new FileIniDataParser();

                    IniData data = new IniData();

                    // Add sections SMPP_Section
                    data.Sections.AddSection("SMPP_Section");

                    data["SMPP_Section"].AddKey("Protocol", "http");
                    data["SMPP_Section"].AddKey("IpAddress", "172.29.54.125");
                    data["SMPP_Section"].AddKey("Port", "13013");
                    data["SMPP_Section"].AddKey("Username", "");
                    data["SMPP_Section"].AddKey("Password", "");


                    // Add sections DCB_Section

                    data.Sections.AddSection("DCB_Section");

                    data["DCB_Section"].AddKey("Protocol", "http");
                    data["DCB_Section"].AddKey("IpAddress", "192.168.120.25");
                    data["DCB_Section"].AddKey("Port", "8223");
                    data["DCB_Section"].AddKey("Username", "");
                    data["DCB_Section"].AddKey("Password", "");
                    // Add sections GECOL_Section

                    data.Sections.AddSection("GECOL_Section");

                    data["GECOL_Section"].AddKey("Protocol", "http");
                    data["GECOL_Section"].AddKey("IpAddress", "160.19.103.138");
                    data["GECOL_Section"].AddKey("Port", "40808");
                    data["GECOL_Section"].AddKey("Username", "");
                    data["GECOL_Section"].AddKey("Password", "");


                    parser.WriteFile(iniFilePath, data);
                }
                catch (Exception ex)
                {
                    string exMessage = ex.Message;
                }
            }
        }

        public Dictionary<string, string> Read(string SectionName)
        {
            string iniFilePath = Path.Combine(baseDirectory, iniFileName);


            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFilePath);

            Dictionary<string, string> Keys = new Dictionary<string, string>();


            switch (SectionName)
            {
                case "SMPP_Section":

                    //Dictionary<string, string> Keys = new Dictionary<string, string>();


                    foreach (var element in data.Sections[SectionName])
                    {
                        Keys.Add(element.KeyName, data[SectionName][element.KeyName]);
                    }

                    break;


                case "DCB_Section":
                    foreach (var element in data.Sections[SectionName])
                    {
                        Keys.Add(element.KeyName, data[SectionName][element.KeyName]);
                    }
                    break;


                case "GECOL_Section":
                    foreach (var element in data.Sections[SectionName])
                    {
                        Keys.Add(element.KeyName, data[SectionName][element.KeyName]);
                    }
                    break;
            }
            return Keys;
        }
    }
}

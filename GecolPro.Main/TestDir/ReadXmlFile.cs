using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_GecolSystem
{
    public class ReadXmlFile
    {

        public string Read()
        {
            string fileName = "resp_3_token.xml";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string ProjectPath = "D:\\OneDrive\\Projects(Asus)\\GecolPro\\GecolPro.Main\\TestDir\\";

            string xmlFilePath = Path.Combine(ProjectPath, fileName);

            string xml = File.ReadAllText(xmlFilePath);

            return xml;

        }
    }
}




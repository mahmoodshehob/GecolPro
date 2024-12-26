using System.Runtime.InteropServices;
using ZGecolPro.SmppClient.Services.IServices;

namespace ZGecolPro.SmppClient.Services
{
    public class Loggers : ILoggers
    {
        private string baseDirectory;
        private string logDirectory = "logs";


        public Loggers() 
        {
            // Check operating system
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                baseDirectory = Path.Combine(desktopPath, logDirectory);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                baseDirectory = $"/home/{Environment.UserName}/{logDirectory}";
            }
            else
            {
                throw new NotSupportedException("OS not supported.");
            }

            // Create the base directory if it doesn't exist
            CreateDirectoryIfNotExists(baseDirectory);

            // Create sub-directories for different types of logs
            CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "Info"));
            CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "Error"));
            CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "Debug"));
            //CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "UssdTrans"));
            //CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "DcbTrans"));
        } 

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private async Task WriteLogAsync(string message, string category)
        {
            try
            {
                string filePath = Path.Combine(baseDirectory, category, $"{category.ToLower()}_sms_" + $"{DateTime.Now:yyyyMMdd}.log");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    await writer.WriteLineAsync($"{DateTime.Now.ToString("yyyyMMddHHmmss")}|{DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss")}|{message}");
                }
            }
            catch (Exception ex) { }
        }

        public async Task LogInfoAsync(string message)
         {
            await WriteLogAsync(message, "Info");
        }

        public async Task LogErrorAsync(string message)
        {
            await WriteLogAsync(message, "Error");
        }

        public async Task LogDebugAsync(string message)
        {
            await WriteLogAsync(message, "Debug");
        }


    }
}

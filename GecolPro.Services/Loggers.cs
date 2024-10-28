using System.IO;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using GecolPro.Services.IServices;


namespace GecolPro.Services
{
    public class Loggers : ILoggers
    {
        private string baseDirectory;

        public Loggers()
        {
            // Check operating system
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                baseDirectory = Path.Combine(desktopPath, "Logs");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                baseDirectory = $"/home/{Environment.UserName}/logs";
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
            CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "UssdTrans"));
            CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "DcbTrans"));
            CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "GecolTrans"));
            CreateDirectoryIfNotExists(Path.Combine(baseDirectory, "IssuedTrans"));

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
            string filePath = Path.Combine(baseDirectory, category, $"{category.ToLower()}_{DateTime.UtcNow:yyyyMMdd}.log");
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                await writer.WriteLineAsync($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}");
            }
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

        public async Task LogUssdTransAsync(string message)
        {
            await WriteLogAsync(message, "UssdTrans");
        }
        
        public async Task LogDcbTransAsync(string message)
        {
            await WriteLogAsync(message, "DcbTrans");
        }
        
        public async Task LogGecolTransAsync(string message)
        {
            await WriteLogAsync(message, "GecolTrans");
        }

        public async Task LogIssuedTokenAsync(string message)
        {
            await WriteLogAsync(message, "IssuedToken");
        }
    }
}

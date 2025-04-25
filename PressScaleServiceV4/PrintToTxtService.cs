using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PressScaleServiceV4.Services
{
    public class PrintToTxtService
    {
        private readonly string filePath;
        private readonly object fileLock = new object();


        public PrintToTxtService(string path = "PlcDataLog.txt")
        {
            filePath = path;
        }

        public async Task WriteDataAsync(float gewicht, int materiaalID, int aantalBalen, bool magVerwerken)
        {
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}; Gewicht={gewicht}; MateriaalID={materiaalID}; AantalBalen={aantalBalen}; MagVerwerken={magVerwerken}";
            try
            {
                lock (fileLock)
                {
                    File.AppendAllText(filePath, line + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij schrijven naar bestand: {ex.Message}");
            }
        }
    }
}

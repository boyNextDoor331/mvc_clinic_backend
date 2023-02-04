using Clinic.Models;
using Newtonsoft.Json;

namespace Clinic.Engine
{
    public class DataManager
    {
        public static void Write(List<Client> data, string fileName)
        {
            using (var fileStream = new StreamWriter($"{fileName}.bin", true))
            {
                foreach (var client in data)
                {
                    fileStream.WriteLineAsync(JsonConvert.SerializeObject(client));
                }
            }
        }

        public static void Write(object data, string fileName)
        {
            using (var fileStream = new StreamWriter($"{fileName}.bin", true))
            {
                fileStream.WriteLineAsync(JsonConvert.SerializeObject(data));
            }
        }

        public static void Read(List<Client> list, string fileName)
        {
            using (var fileStream = new StreamReader($"{fileName}.bin"))
            {
                while (!fileStream.EndOfStream)
                {
                    var stream = fileStream.ReadLine();
                    if (!string.IsNullOrEmpty(stream))
                    {
                        list.Add(JsonConvert.DeserializeObject<Client>(stream));
                    }
                }
            }
        }

        public static void Truncate(string fileName)
        {
            using (var fileStreamClear = new FileStream($"{fileName}.bin", FileMode.Truncate)) { }
        }



    }
}

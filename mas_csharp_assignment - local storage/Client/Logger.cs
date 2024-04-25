using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Client
{
    internal class Logger
    {
        public static string read_file(string sub_folder_name, string file_name)
        {
            string content = string.Empty;

            string file_path = Path.Combine(Directory.GetCurrentDirectory(), sub_folder_name, file_name);

            if (File.Exists(file_path))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), sub_folder_name, file_name)))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return content;
        }

        public static void log_object(string sub_folder_name, string file_name, object content)
        {
            DirectoryInfo directory_info = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), sub_folder_name));

            string file_path = Path.Combine(directory_info.FullName, file_name);

            try
            {
                using (StreamWriter writer = new StreamWriter(file_path, false, Encoding.UTF8))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(content));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void delete_file(string sub_folder_name, string file_name)
        {
            string file_path = Path.Combine(Directory.GetCurrentDirectory(), sub_folder_name, file_name);

            if (File.Exists(file_path))
            {
                File.Delete(file_path);
            }
        }
    }
}

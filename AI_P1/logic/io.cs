using System.IO;
namespace AI_P1
{
    namespace io
    {
        public static class IOHandler
        {
            public static string LoadInitialState(string path)
            {
                string initialState = null;
                using (StreamReader reader = new StreamReader(path))
                {
                    initialState = reader.ReadToEnd();   
                }

                return initialState;
            }

            public static void SaveInitialState(string path, string initialState)
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(initialState);
                }
            }
           
        }
    }
}
using System;
using System.IO;
using System.Data.Odbc;

namespace DBFileExtractor
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Enter connection string:");
            string connectionString = Console.ReadLine();

            object data;

            try
            {
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    Console.WriteLine("Connection Successful.");
                    Console.Write("Enter table name: ");
                    string tableName = Console.ReadLine();
                    Console.Write("Enter field name: ");
                    string fieldName = Console.ReadLine();
                    Console.Write("Enter where clause: ");
                    string whereClause = Console.ReadLine();
                    string queryString = string.Format("SELECT {0} FROM {1} WHERE {2}", fieldName, tableName, whereClause);
                    OdbcCommand cmd = new OdbcCommand(queryString, conn);
                    conn.Open();
                    data = cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press Enter to Exit.");
                Console.ReadLine();
                return 1;
            }
            Console.Write("Enter output file name: ");
            string fileName = Console.ReadLine();

            string outputFile = Path.Combine(Environment.CurrentDirectory, fileName);

            Console.Write("Text or Binary: ");
            switch (Console.ReadLine().ToLower().Substring(0, 1))
            {
                case "b":
                    using (FileStream file = File.Create(outputFile))
                    {
                        file.Write(data as byte[]);
                    }
                    break;
                case "t":
                default:
                    File.WriteAllText(outputFile, data as string);
                    break;
            }

            Console.WriteLine(string.Format("File written to: {0}", outputFile));
            Console.WriteLine("Press Enter to Exit.");
            Console.ReadLine();
            return 0;
        }
    }
}

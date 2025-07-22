namespace JsonConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string json = File.ReadAllText("data.txt");

            var formattedJson = JsonFormatter.PrettyPrintJson(json);

            Console.WriteLine(formattedJson);
        }
    }
}

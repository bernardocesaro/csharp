global using MySql.Data.MySqlClient;
global using System.Text.RegularExpressions;
global using System.Reflection;
global using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Clear();
        string stringConexao = PegarConexao.Executar();

        try
        {
            Menu menu = new Menu(stringConexao);

            menu.Executar();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
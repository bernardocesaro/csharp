using System;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        string stringConexao = GerenciarConexao.Executar();
        Console.Clear();

        using (var connection = new MySqlConnection(stringConexao))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Conectado ao MySQL!");

                Sql sql = new Sql(stringConexao);
                Menu menu = new Menu(sql);

                menu.Executar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
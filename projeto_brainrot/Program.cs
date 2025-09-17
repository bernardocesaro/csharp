using System;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "server=localhost;user=root;password=mysql_local1510;database=projeto_brainrot;";

        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Conectado ao MySQL!");
                Sql sql = new Sql();

                string[] menuOptions =
                {
                    "1 - Insert dados",
                    "2 - Select dados",
                    "3 - Update dados",
                    "4 - Delete dados",
                    "5 - Sair"
                };
                int option = 0;
                do
                {
                    foreach (var menuOption in menuOptions)
                    {
                        Console.WriteLine(menuOption);
                    }
                    Console.Write("Escolha uma opção: ");
                    string? optionInput = Console.ReadLine();

                    Console.Clear();

                    // Validar entrada
                    if (!int.TryParse(optionInput, out option) || option < 1 || option > 5)
                    {
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        continue;
                    }

                    if (option == 5)
                    {
                        Console.WriteLine("Saindo...");
                        Thread.Sleep(750);
                        break;
                    }

                    sql.DecidirOpcaoTabela(option, connection);
                    
                } while (option != 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
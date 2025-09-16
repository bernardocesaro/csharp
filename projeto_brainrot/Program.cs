using System;
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

                // 1. INSERIR DADOS
                string insertSql = "INSERT INTO conta (nome) VALUES (@nome)";
                
                using (var insertCommand = new MySqlCommand(insertSql, connection))
                {
                    insertCommand.Parameters.AddWithValue("@nome", Console.ReadLine());
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} linha(s) inserida(s)!");
                }

                // 2. LER DADOS
                string selectSql = "SELECT id, nome FROM conta";
                
                using (var selectCommand = new MySqlCommand(selectSql, connection))
                using (var reader = selectCommand.ExecuteReader())
                {
                    Console.WriteLine("\nDados na tabela:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Nome: {reader["nome"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
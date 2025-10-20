public static class TestarConexao
{
    public static MySqlConnection ConectarBanco(string stringConexao)
    {
        var connection = new MySqlConnection(stringConexao);
        connection.Open();

        Console.WriteLine("Conectado ao MySQL com sucesso!");
        Thread.Sleep(800);
        Console.Clear();

        return connection;
    }
    public static bool AbrirFecharConexao(string stringConexao)
    {
        try
        {
            using (var connection = new MySqlConnection(stringConexao))
            {
                connection.Open();
                Console.WriteLine("Testando conexão...");
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
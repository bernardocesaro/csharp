
class Program
{
    static void Main()
    {
        Console.Clear();
        string stringConexao = GerenciarConexao.Executar();

        try
        {
            MiSql miSql = new MiSql(stringConexao);
            Menu menu = new Menu(miSql);

            menu.Executar();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
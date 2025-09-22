
class Program
{
    static void Main()
    {
        Console.Clear();
        string stringConexao = GerenciarConexao.Executar();

        try
        {
            Console.WriteLine("Conectado ao MySQL!");

            MiSql mySql = new MiSql(stringConexao);
            Menu menu = new Menu(mySql);

            menu.Executar();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
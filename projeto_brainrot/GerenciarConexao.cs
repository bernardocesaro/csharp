
public static class GerenciarConexao
{
    public static string Executar()
    {
        while (true)
        {
            Console.Clear();
            string? tempOpcaoConexaoBanco = EscolherConexao();
            string? stringConexao = ValidarEscolherConexao(tempOpcaoConexaoBanco);

            if (stringConexao != null)
            {
                return stringConexao;
            }
        }
    }
    private static string? EscolherConexao()
    {
        Console.WriteLine("Deseja usar o banco Real (1) ou o banco Teste (2)?");
        return Console.ReadLine();
    }
    private static string? ValidarEscolherConexao(string? tempOpcaoConexaoBanco)
    {
        tempOpcaoConexaoBanco = tempOpcaoConexaoBanco == null ? null : tempOpcaoConexaoBanco.Trim();

        if (tempOpcaoConexaoBanco == null || tempOpcaoConexaoBanco != "1" && tempOpcaoConexaoBanco != "2")
        {
            Utilidades.MostrarErro();
            return null;
        }
        else
        {
            return AtribuirConexao(tempOpcaoConexaoBanco);
        }
    }
    private static string AtribuirConexao(string tempOpcaoConexaoBanco)
    {
        string stringConexao = tempOpcaoConexaoBanco == "1"
            ? "server=localhost;user=root;password=mysql_local1510;database=projeto_brainrot;"
            : "server=localhost;user=root;password=mysql_local1510;database=projeto_brainrot_teste;";
        return stringConexao;
    }
}
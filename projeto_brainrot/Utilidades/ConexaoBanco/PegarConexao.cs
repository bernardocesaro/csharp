public enum TipoConexao { Real = 1, Teste = 2 }

public static class PegarConexao
{
    public static string Executar()
    {
        while (true)
        {
            Console.Clear();
            string? input = EscolherConexao();
            TipoConexao? opcaoConexaoBancoValidada = ValidarConexao(input);

            if (opcaoConexaoBancoValidada.HasValue)
            {
                return AtribuirConexao(opcaoConexaoBancoValidada.Value);
            }
            else
            {
                Utilidades.MostrarErro();
            }
        }
    }
    private static string? EscolherConexao()
    {
        Console.WriteLine("Deseja usar o banco Real (1) ou o banco Teste (2)?");
        return Console.ReadLine();
    }

    private static TipoConexao? ValidarConexao(string? input)
    {
        input = input == null ? null : input.Trim();

        if (int.TryParse(input, out int opcaoConexaoBancoValidada) && Enum.IsDefined(typeof(TipoConexao), opcaoConexaoBancoValidada))
        {
            return (TipoConexao)opcaoConexaoBancoValidada;
        }

        return null;
    }

    private static string AtribuirConexao(TipoConexao opcaoConexaoBancoValidada)
    {
        const string STRING_TIPO_CONEXAO_REAL = "server=localhost;user=root;password=mysql_local1510;database=projeto_brainrot;";
        const string STRING_TIPO_CONEXAO_TESTE = "server=localhost;user=root;password=mysql_local1510;database=projeto_brainrot_teste;";

        string stringConexao = STRING_TIPO_CONEXAO_TESTE;

        Console.WriteLine($"Opção Selecionada: {opcaoConexaoBancoValidada}");
        if (opcaoConexaoBancoValidada == TipoConexao.Real)
        {
            stringConexao = STRING_TIPO_CONEXAO_REAL;
        }
        else if (opcaoConexaoBancoValidada == TipoConexao.Teste)
        {
            stringConexao = STRING_TIPO_CONEXAO_TESTE;
        }

        return stringConexao;
    }
}
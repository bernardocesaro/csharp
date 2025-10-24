public class ComandoSelect : BaseSql
{
    public ComandoSelect(string stringConexao, OpcaoTabela opcaoTabela, List<string> camposTabelaSnakeCase) : base(stringConexao, opcaoTabela, camposTabelaSnakeCase) { }

    public override void Executar()
    {
        using (var conexao = TestarConexao.ConectarBanco(_stringConexao))
        {
            string stringComando = CriarComando();

            if (!Utilidades.Confirmar(_opcaoTabela))
            {
                return;
            }

            using (var comandoSelect = new MySqlCommand(stringComando, conexao))
            using (var leitor = comandoSelect.ExecuteReader())
            {
                Console.WriteLine("Trazendo dados");
                Thread.Sleep(800);
                Console.Clear();

                MostrarSelect(leitor);
            }
        }
    }
    protected override string CriarComando()
    {
        string stringComando = $"SELECT {ConverterUmParaDois.ConverterListToStringParametro(_camposTabelaSnakeCase, false)} " + 
                               $"FROM {_opcaoTabela.ToString().ToLower()}";
        // Console.WriteLine($"\nO comando gerado foi esse: {stringComandoSelect}\n");
        return stringComando;
    }
    private void MostrarSelect(MySqlDataReader leitor)
    {
        Console.WriteLine($"Dados da tabela '{_opcaoTabela.ToString().ToLower()}'");

        MostrarCamposSelect();
        MostrarLinhaSeparadoraSelect();
        MostrarDadosSelect(leitor);

        Console.WriteLine("\nMais nenhum resultado encontrado!");
        Utilidades.Retornar();
    }
    private void MostrarCamposSelect()
    {
        for (int i = 0; i < _camposTabelaSnakeCase.Count; i++)
        {
            Console.Write("|" + $" {_camposTabelaSnakeCase[i],-15} ");
        }
        Console.Write("|\n");
    }
    private void MostrarLinhaSeparadoraSelect()
    {
        for (int i = 0; i < _camposTabelaSnakeCase.Count; i++)
        {
            Console.Write("|" + $"=================");
        }
        Console.Write("|\n");
    }
    private void MostrarDadosSelect(MySqlDataReader leitor)
    {
        while (leitor.Read())
        {
            for (int i = 0; i < _camposTabelaSnakeCase.Count; i++)
            {
                Console.Write($"| {leitor[$"{_camposTabelaSnakeCase[i]}"],-15} ");
            }
            Console.Write("|\n");
        }
    }

}
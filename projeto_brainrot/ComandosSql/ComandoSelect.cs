public class ComandoSelect : ComandoBaseSql
{
    protected readonly string _stringConexao;
    public ComandoSelect(string stringConexao) : base(stringConexao) { }

    public override void Executar()
    {
        using (var conexao = TestarConexao.ConectarBanco(_stringConexao))
        {
            Console.Clear();
            OpcaoTabela tabelaValidada = PegarEValidarTabela();

            if (tabelaValidada == OpcaoTabela.Sair)
            {
                return;
            }
            
            List<string> camposTabelaSelecionada = PegarCamposTabelaSelecionada((tabelaValidada).ToString());

            string stringComandoSelect = CriarComandoSelect(tabelaValidada.ToString(), camposTabelaSelecionada);

            using (var comandoSelect = new MySqlCommand(stringComandoSelect, conexao))
            using (var leitor = comandoSelect.ExecuteReader())
            {
                Console.WriteLine("Trazendo dados");
                Thread.Sleep(800);
                Console.Clear();

                MostrarSelect(leitor, tabelaValidada.ToString(), camposTabelaSelecionada);
            }
        }
    }
    private string CriarComandoSelect(string tabelaSelecionadaValidada, List<string> camposTabelaSelecionada)
    {
        string stringComandoSelect = $"SELECT {GerarCamposComandoSql(camposTabelaSelecionada)} FROM {tabelaSelecionadaValidada}";
        // Console.WriteLine($"\nO comando gerado foi esse: {stringComandoSelect}\n");

        return stringComandoSelect;
    }
    private void MostrarSelect(MySqlDataReader leitor, string tabelaSelecionadaValidada, List<string> camposTabelaSelecionada)
    {
        Console.WriteLine($"Dados da tabela '{tabelaSelecionadaValidada}'");

        MostrarCamposSelect(camposTabelaSelecionada);
        MostrarLinhaSeparadoraSelect(camposTabelaSelecionada);
        MostrarDadosSelect(leitor, camposTabelaSelecionada);

        Console.WriteLine("\nMais nenhum resultado encontrado!");
        Utilidades.Retornar();
    }
    private void MostrarCamposSelect(List<string> camposTabelaSelecionada)
    {
        for (int i = 0; i < camposTabelaSelecionada.Count; i++)
        {
            Console.Write("|" + $" {camposTabelaSelecionada[i],-15} ");
        }
        Console.Write("|\n");
    }
    private void MostrarLinhaSeparadoraSelect(List<string> camposTabelaSelecionada)
    {
        for (int i = 0; i < camposTabelaSelecionada.Count; i++)
        {
            Console.Write("|" + $"=================");
        }
        Console.Write("|\n");
    }
    private void MostrarDadosSelect(MySqlDataReader leitor, List<string> camposTabelaSelecionada)
    {
        while (leitor.Read())
        {
            for (int i = 0; i < camposTabelaSelecionada.Count; i++)
            {
                Console.Write($"| {leitor[$"{camposTabelaSelecionada[i]}"],-15} ");
            }
            Console.Write("|\n");
        }
    }

}
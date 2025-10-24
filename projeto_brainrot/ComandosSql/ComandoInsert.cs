public class ComandoInsert : BaseSql
{
    public ComandoInsert(string stringConexao, OpcaoTabela opcaoTabela, List<string> camposTabelaSnakeCase) : base(stringConexao, opcaoTabela, camposTabelaSnakeCase) { }

    public override void Executar()
    {
        using (var conexao = TestarConexao.ConectarBanco(_stringConexao))
        {
            string stringComando = CriarComando();

            AtribuirValoresCampos();

            if (!Utilidades.Confirmar(_opcaoTabela))
            {
                return;
            }

            using (var comandoSelect = new MySqlCommand(stringComando, conexao))
            using (var leitor = comandoSelect.ExecuteReader())
            {
                Console.WriteLine("Registrando dados");
                Thread.Sleep(800);
                Console.Clear();
                Console.WriteLine("Dados Registrados!");
                Utilidades.Retornar();
            }
        }
    }
    protected override string CriarComando()
    {
        string stringComando = $"INSERT INTO {_opcaoTabela.ToString().ToLower()} ({ConverterUmParaDois.ConverterListToStringParametro(_camposTabelaSnakeCase, false)}) " +
                               $"VALUES({ConverterUmParaDois.ConverterListToStringParametro(_camposTabelaSnakeCase, true)})";
        // Console.WriteLine($"\nO comando gerado foi esse: {stringComandoSelect}\n");
        return stringComando;
    }
    private void AtribuirValoresCampos()
    {
        foreach (var campo in _camposTabelaSnakeCase)
        {
            while (true)
            {
                if (campo == "Id")
                {
                    break;
                }

                string? valorCampoEscrito = PegarValorCampo(campo);
                if (string.IsNullOrEmpty(valorCampoEscrito))
                {

                }    
            }
        }
    }
    private string? PegarValorCampo(string campo)
    {
        Console.Write($"Digite qual valor colocar no campo {campo}: ");
        return Console.ReadLine();
    }
}
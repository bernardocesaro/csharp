public enum TipoOpcao { Insert = 1, Select = 2, Update = 3, Delete = 4, Sair = 5 }

public class Menu
{
    private readonly string _stringConexao;
    private readonly Dictionary<TipoOpcao, string> opcoesMenu = new Dictionary<TipoOpcao, string>()
    {
        { TipoOpcao.Insert, "1 - Insert dados" },
        { TipoOpcao.Select, "2 - Select dados" },
        { TipoOpcao.Update, "3 - Update dados" },
        { TipoOpcao.Delete, "4 - Delete dados" },
        { TipoOpcao.Sair  , "5 - Sair"         }
    };
    public Menu(string stringConexao)
    {
        _stringConexao = stringConexao;
    }

    public void Executar()
    {
        bool conexaoEstabelecida = TestarConexao.AbrirFecharConexao(_stringConexao);

        if (!conexaoEstabelecida)
        {
            return;
        }

        while (true)
        {
            Console.Clear();
            MostrarMenu();
            string? input = PegarOpcao();
            TipoOpcao? opcaoValidada = ValidarOpcao(input);

            if (opcaoValidada.HasValue)
            {
                if (!ProcessarExecutarOpcao(opcaoValidada.Value))
                {
                    break;
                }
            }
            else
            {
                Utilidades.MostrarErro();
            }
        }
    }
    private void MostrarMenu()
    {
        Console.WriteLine("Menu SQL - Comandos");

        foreach (var opcaoMenu in opcoesMenu)
        {
            Console.WriteLine(opcaoMenu.Value);
        }
    }
    private string? PegarOpcao()
    {
        Console.Write("Escolha uma opção: ");
        return Console.ReadLine();
    }
    private TipoOpcao? ValidarOpcao(string? input)
    {
        input = input == null ? null : input.Trim();

        if (int.TryParse(input, out int opcaoValidada) && opcoesMenu.ContainsKey((TipoOpcao)opcaoValidada))
        {
            return (TipoOpcao)opcaoValidada;
        }

        return null;
    }
    private bool ProcessarExecutarOpcao(TipoOpcao? opcaoValidada)
    {
        Console.Clear();

        BaseSql comando;
        OpcaoTabela opcaoTabela = ConsultaTabelaECampos.PegarEValidarTabela();
        List<string> camposTabelaSnakeCase = ConsultaTabelaECampos.PegarCamposEConverterToList(opcaoTabela);

        switch (opcaoValidada)
        {
            case TipoOpcao.Insert:
                Console.WriteLine("Executando INSERT\n");
                comando = new ComandoSelect(_stringConexao, opcaoTabela, camposTabelaSnakeCase);
                comando.Executar();
                return true;
            case TipoOpcao.Select:
                Console.WriteLine("Executando SELECT\n");
                return true;
            case TipoOpcao.Update:
                Console.WriteLine("Executando UPDATE\n");
                return true;
            case TipoOpcao.Delete:
                Console.WriteLine("Executando DELETE\n");
                return true;
            case TipoOpcao.Sair:
                Console.WriteLine("Saindo...");
                return false;
            default:
                return false;
        }
    }
}
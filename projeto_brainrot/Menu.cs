
public class Menu
{
    private readonly MiSql _miSql;
    public Menu(MiSql miSql)
    {
        _miSql = miSql;
    }

    public void Executar()
    {
        while (true)
        {
            Console.Clear();
            MostrarMenu();
            int opcaoSelecionadaValidada = PegarEValidarOpcao();

            if (!ProcessarOpcaoComando(opcaoSelecionadaValidada))
            {
                break;
            }
        }
    }
    private void MostrarMenu()
    {
        string[] opcoesMenu =
        {
            "Menu MySql - Comandos",
            "1 - Insert dados",
            "2 - Select dados",
            "3 - Update dados",
            "4 - Delete dados",
            "5 - Sair"
        };
        foreach (var opcaoMenu in opcoesMenu)
        {
            Console.WriteLine(opcaoMenu);
        }
    }
    private int PegarEValidarOpcao()
    {
        string? opcaoEscrita = PegarOpcao();
        int? opcaoEscritaValidada = ValidarPegarOpcao(opcaoEscrita);
        if (opcaoEscritaValidada != null)
        {
            return opcaoEscritaValidada.Value;
        }
        return 0;
    }
    private string? PegarOpcao()
    {
        Console.Write("Escolha uma opção: ");
        return Console.ReadLine();
    }
    private int? ValidarPegarOpcao(string? opcaoEscrita)
    {
        opcaoEscrita = opcaoEscrita == null ? null : opcaoEscrita.Trim();

        if (!int.TryParse(opcaoEscrita, out int opcaoSelecionada) || opcaoSelecionada < 1 || opcaoSelecionada > 5)
        {
            Utilidades.MostrarErro();
            return null;
        }
        return opcaoSelecionada;
    }
    private bool ProcessarOpcaoComando(int opcaoSelecionadaValidada)
    {
        Console.Clear();
        switch (opcaoSelecionadaValidada)
        {
            case 0:
                return true;
            case 1:
                Console.WriteLine("Executando INSERT\n");
                _miSql.ExecutarComandoInsert();
                return true;
            case 2:
                Console.WriteLine("Executando SELECT\n");
                _miSql.ExecutarComandoSelect();
                return true;
            case 3:
                Console.WriteLine("Executando UPDATE\n");
                return true;
            case 4:
                Console.WriteLine("Executando DELETE\n");
                return true;
            case 5:
                Console.WriteLine("Saindo...");
                return false;
            default:
                throw new InvalidOperationException ($"Opção inesperada: {opcaoSelecionadaValidada}");
        }
    }
}
public class Menu
{
    private readonly Sql _sql;
    public Menu(Sql sql)
    {
        _sql = sql;
    }

    public void Executar()
    {
        while (true)
        {
            Console.Clear();
            MostrarOpcoes();
            int opcaoSelecionadaValidada = PegarEValidarOpcao();

            if (!ProcessarOpcao(opcaoSelecionadaValidada))
            {
                break;
            }
        }
    }
    private void MostrarOpcoes()
    {
        string[] opcoesMenu =
            {
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
        while (true)
        {
            string? opcaoEscrita = PegarOpcao();
            int? opcaoSelecionadaValidada = ValidarPegarOpcao(opcaoEscrita);
            if (opcaoSelecionadaValidada != null)
            {
                return opcaoSelecionadaValidada.Value;
            }
        }
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
            Console.WriteLine("Erro ao digitar uma Opção\nOpção inválida!");
            Console.WriteLine("Digite qualquer tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
            return null;
        }
        return opcaoSelecionada;
    }
    private bool ProcessarOpcao(int opcaoSelecionadaValidada)
    {
        switch (opcaoSelecionadaValidada)
        {
            case 1:
                Console.WriteLine("Executando INSERT");
                return true;
            case 2:
                Console.WriteLine("Executando SELECT");
                return true;
            case 3:
                Console.WriteLine("Executando UPDATE");
                return true;
            case 4:
                Console.WriteLine("Executando DELETE");
                return true;
            case 5:
                Console.WriteLine("Saindo...");
                return false;
            default:
                throw new InvalidOperationException($"Opção inesperada: {opcaoSelecionadaValidada}");
        }
    }
}
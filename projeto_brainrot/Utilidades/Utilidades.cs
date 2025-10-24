public static class Utilidades
{
    public static void MostrarErro()
    {
        Console.WriteLine("\nErro...");
        Retornar();
    }
    public static void Retornar()
    {
        Console.WriteLine("Digite qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
    public static bool Confirmar(OpcaoTabela opcaoTabela)
    {
        while (true)
        {
            if (opcaoTabela == OpcaoTabela.Sair)
            {
                return false;
            }

            string? inputConfirmacao = PegarConfirmacao();
            bool? confirmacaoValidada = ValidarConfirmacao(inputConfirmacao);

            if (confirmacaoValidada.HasValue)
            {
                return confirmacaoValidada.Value;
            }
            MostrarErro();
        }
    }
    private static string? PegarConfirmacao()
    {
        Console.WriteLine("Continuar? (Sim = 1; Não = 2)");
        return Console.ReadLine();
    }
    private static bool? ValidarConfirmacao(string? inputConfirmacao)
    {
        const bool CONTINUAR_SIM = true;
        const bool CONTINUAR_NAO = false;

        inputConfirmacao = inputConfirmacao == null ? null : inputConfirmacao.Trim();

        if (int.TryParse(inputConfirmacao, out int confirmacaoValidada))
        {
            if (confirmacaoValidada == 2)
            {
                return CONTINUAR_NAO;
            }
            return CONTINUAR_SIM;
        }
        return null;
    }
}
public static class CapturarTabelaECampos
{
    public static OpcaoTabela PegarEValidarTabela()
    {
        while (true)
        {
            string? inputTabela = PegarTabela();
            OpcaoTabela? tabelaValidada = ValidarTabela(inputTabela);
            if (tabelaValidada.HasValue)
            {
                Console.WriteLine("\nTabela validada com sucesso!\n");
                Thread.Sleep(800);
                Console.Clear();
                return tabelaValidada.Value;
            }
            Utilidades.MostrarErro();
        }
    }
    private static string? PegarTabela()
    {
        Console.WriteLine("Digite qual tabela deseja selecionar (Digite 'sair' para voltar ao menu): ");
        return Console.ReadLine();
    }
    private static OpcaoTabela? ValidarTabela(string? input)
    {
        input = input == null ? null : input.Trim();

        if (Enum.TryParse<OpcaoTabela>(input, true, out OpcaoTabela tabelaValidada) && MapeamentoTabelas.TabelasValidas.ContainsKey(tabelaValidada))
        {
            return tabelaValidada;
        }
        return null;
    }
    public static List<string> PegarCamposEConverterToList(OpcaoTabela opcaoTabela)
    {
        PropertyInfo[] camposTabela = PegarCampos(opcaoTabela);
        return ConverterToList(camposTabela);
    }
    private static PropertyInfo[] PegarCampos(OpcaoTabela opcaoTabela)
    {
        PropertyInfo[] camposTabela;

        if (MapeamentoTabelas.TabelasValidas.ContainsKey(opcaoTabela))
        {
            camposTabela = MapeamentoTabelas.TabelasValidas[opcaoTabela].GetProperties();
            return camposTabela;
        }
        throw new InvalidOperationException($"Tabela inesperada: {opcaoTabela}");
    }
    private static List<string> ConverterToList(PropertyInfo[] camposTabela)
    {
        List<string> camposSnakeCase = new List<string>();

        foreach (PropertyInfo propInfo in camposTabela)
        {
            camposSnakeCase.Add(ConverterUmParaDois.ConverterPascalToSnakeCase(propInfo.Name));
        }
        return camposSnakeCase;
    }
}
    
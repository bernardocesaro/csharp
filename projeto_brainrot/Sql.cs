using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Reflection;

public class MiSql
{
    private readonly string _stringConexao;
    protected Conta conta { get; set; } = new Conta();
    protected Brainrot brainrot { get; set; } = new Brainrot();

    public MiSql(string stringConexao)
    {
        _stringConexao = stringConexao;
    }
    public void ExecutarComandoInsert()
    {
        using (var connection = new MySqlConnection(_stringConexao))
        {
            bool estaConectado = TestandoConexao();

            if (!estaConectado)
            {
                Utilidades.MostrarErro();
            }
            else
            {
                Console.WriteLine("Conectado ao MySQL com sucesso!\n");

                connection.Open();
                CriarComandoInsert(connection);
            }
        }
    }
    public bool TestandoConexao()
    {
        try
        {
            using (var connection = new MySqlConnection(_stringConexao))
            {
                connection.Open();
                Console.WriteLine("Testando conexão...");
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
    public void CriarComandoInsert(MySqlConnection connection)
    {
        // Fazer um mini debug, tipo:

        // Console.WriteLine($"=== DEBUG SQL ===");
        // Console.WriteLine($"SQL: {sql}");
        // Console.WriteLine($"Tabela: {tabela}");
        // Console.WriteLine($"Campos: {string.Join(", ", campos)}");
        // Console.WriteLine($"Instância: {instancia.GetType().Name}");

        // Ai teria que trocar o nome das variáveis e de outras coisas

        string tabelaSelecionadaValidada = PegarEValidarTabela();
        List<string> camposTabelaSelecionada = PegarCamposTabelaSelecionada(tabelaSelecionadaValidada);
        PegarEValidarEAtribuirValoresCampos(tabelaSelecionadaValidada, camposTabelaSelecionada);

        string stringComandoInsert = $"INSERT INTO {tabelaSelecionadaValidada} ({GerarCamposComandoSql(camposTabelaSelecionada)}) VALUES ({GerarParametrosComandoSql(camposTabelaSelecionada)})";
        Console.WriteLine($"O comando gerado foi esse: {stringComandoInsert}");

        using (var comandoInsert = new MySqlCommand(stringComandoInsert, connection))
        {
            PropertyInfo[] propriedades = null;
            object instancia = null;

            switch (tabelaSelecionadaValidada)
            {
                case "conta":
                    propriedades = conta.GetType().GetProperties();
                    instancia = conta;
                    break;
                case "brainrot":
                    propriedades = brainrot.GetType().GetProperties();
                    instancia = brainrot;
                    break;
                default:
                    Utilidades.MostrarErro();
                    break;
            }

            if (propriedades != null)
            {
                for (int i = 0; i < camposTabelaSelecionada.Count; i++)
                {
                    // Ordenar as propriedades, id = 0; nome = 1
                    // Pelo que parece a ordem não necessáriamente é a que está na classe, depende de algumas coisas
                    // A ideia é ordenar manualmente para ver se funciona
                    comandoInsert.Parameters.AddWithValue($"@{camposTabelaSelecionada[i]}", propriedades[i + 1].GetValue(instancia));
                }
                comandoInsert.ExecuteNonQuery();
            }
        }
    }
    private string PegarEValidarTabela()
    {
        while (true)
        {
            string? tabelaEscrita = PegarTabela();
            string? tabelaEscritaValidada = ValidarTabela(tabelaEscrita);
            if (tabelaEscritaValidada != null)
            {
                Console.WriteLine("\nTabela validada com sucesso!\n");
                return tabelaEscritaValidada;
            }
            else
            {
                Utilidades.MostrarErro();
            }
        }
    }
    private string? PegarTabela()
    {
        Console.WriteLine($"Digite em qual tabela deseja fazer um INSERT: ");
        Console.Write("Tabelas disponíveis: 'conta', 'brainrot' --> ");
        return Console.ReadLine();
    }
    private string? ValidarTabela(string? tabelaEscrita)
    {
        tabelaEscrita = tabelaEscrita == null ? null : tabelaEscrita.Trim();

        if (tabelaEscrita == null || !(tabelaEscrita == "conta") && !(tabelaEscrita == "brainrot"))
        {
            return null;
        }
        return tabelaEscrita;
    }
    private List<string> PegarCamposTabelaSelecionada(string tabelaSelecionadaValidada)
    {
        List<string> camposTabelaSelecionada = new List<string>(); 

        if (tabelaSelecionadaValidada == "conta")
        {
            camposTabelaSelecionada =
            [
                "Nome",
                "EspacosTotais",
                "EspacosOcupados"
            ];
        }
        else if (tabelaSelecionadaValidada == "brainrot")
        {
            camposTabelaSelecionada =
            [
                "IdConta",
                "Nome",
                "Raridade",
                "Efeito"
            ];
        }
        else
        {
            throw new InvalidOperationException
                ($"Tabela inesperada: {tabelaSelecionadaValidada}");
        }
        return camposTabelaSelecionada;
    }
    private void PegarEValidarEAtribuirValoresCampos(string tabelaSelecionadaValidada, List<string> camposTabelaSelecionada)
    {
        foreach (string campo in camposTabelaSelecionada)
        {
            while (true)
            {
                string? valorCampoEscrito = PegarValorCampo(camposTabelaSelecionada, campo);
                string? valorCampoEscritoValidado = ValidarValorCampo(valorCampoEscrito);

                if (valorCampoEscritoValidado != null)
                {
                    AtribuirValorCampo(tabelaSelecionadaValidada, campo, valorCampoEscritoValidado);
                    break;
                }
                else
                {
                    Utilidades.MostrarErro();
                }
            }
        }
    }
    private string? PegarValorCampo(List<string> camposTabelaSelecionada, string campo)
    {
        Console.WriteLine($"Digite qual valor colocar no campo {campo}: ");
        return Console.ReadLine();
    }
    private string? ValidarValorCampo(string? valorCampoEscrito)
    {
        string patternVerificador = @"[a-zA-Z0-9]";
        string patternCerto = @"[^a-zA-Z0-9]";
        if (valorCampoEscrito == null || Regex.Replace(valorCampoEscrito, patternVerificador, "").Length > 0)
        {
            return null;
        }
        return Regex.Replace(valorCampoEscrito, patternCerto, "");
    }
    private void AtribuirValorCampo(string tabelaSelecionadaValidada, string campo, string valorCampoEscritoValidado)
    {
        PropertyInfo propriedade = PegarPropriedadeTabela(tabelaSelecionadaValidada, campo);

        if (propriedade != null)
        {
            if (int.TryParse(valorCampoEscritoValidado, out int valorIntCampoEscritoValidado))
            {
                switch (tabelaSelecionadaValidada)
                {
                    case "conta":
                        propriedade.SetValue(conta, valorIntCampoEscritoValidado);
                        break;
                    case "brainrot":
                        propriedade.SetValue(brainrot, valorIntCampoEscritoValidado);
                        break;
                    default:
                        throw new InvalidOperationException
                            ($"Tabela inesperada: {tabelaSelecionadaValidada}");
                }
            }
            else
            {
                switch (tabelaSelecionadaValidada)
                {
                    case "conta":
                        propriedade.SetValue(conta, valorCampoEscritoValidado);
                        break;
                    case "brainrot":
                        propriedade.SetValue(brainrot, valorCampoEscritoValidado);
                        break;
                    default:
                        Utilidades.MostrarErro();
                        break;
                }
            }
        }
    }
    private PropertyInfo PegarPropriedadeTabela(string tabelaSelecionadaValidada, string campo)
    {
        switch (tabelaSelecionadaValidada)
        {
            case "conta":
                Type tipoConta = typeof(Conta);
                return tipoConta.GetProperty(campo);
            case "brainrot":
                Type tipoBrainrot = typeof(Brainrot);
                return tipoBrainrot.GetProperty(campo);
            default:
                Utilidades.MostrarErro();
                return null;
        }
    }
    private string GerarCamposComandoSql(List<string> camposTabelaSelecionada)
    {
        string camposSql = string.Join(", ", camposTabelaSelecionada);
        return camposSql;
    }
    private string GerarParametrosComandoSql(List<string> camposTabelaSelecionada)
    {
        string camposSql = "@" + string.Join(", @", camposTabelaSelecionada);
        return camposSql;
    }
    public void ComandoSelect()
    {

    }
    public void ComandoUpdate()
    {

    }
    public void ComandoDelete()
    {
        
    }
    /*
    public bool VerificarStringInList(List<string> tabelas, string tabelaSelecionada)
    {
        foreach (var tabela in tabelas)
        {
            if (tabelaSelecionada == tabela)
                return true;
        }
        return false;
    }
    public string GerarCamposComandoSql(List<string> campos, bool parametro)
    {
        string camposSql = "";
        string arroba = "";
        if (parametro)
        {
            arroba = "@";
        }

        foreach (var campo in campos)
        {
            if (camposSql == "")
                camposSql = $"{arroba}{campo}";
            else
            {
                camposSql += $",{arroba}{campo}";
            }
        }
        return camposSql;
    }
    public void DecidirOpcaoTabela(int option, MySqlConnection connection)
    {
        string[] significadoOpcao =
        {
            "inserir",
            "ler",
            "atualizar"
        };
        if (option == 1)
        {
            // 1. INSERIR DADOS
            Console.WriteLine($"\nDigite em qual tabela deseja {significadoOpcao[0]}: ");
            Console.WriteLine("Tabelas disponíveis: 'conta', 'brainrot'");
            string? input = Console.ReadLine();
            string? tabelaSelecionada = (input == null ? null : input.Trim());

            if (tabelaSelecionada == null || !VerificarStringInList(tabelas, tabelaSelecionada))
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }
            else
            {
                DefinirCampos(tabelaSelecionada);
                InsertCampos(tabelaSelecionada, connection);
            }
        }
        else if (option == 2)
        {
            // 2. LER DADOS
            Console.WriteLine($"\nDigite em qual tabela deseja {significadoOpcao[1]}: ");
            Console.WriteLine("Tabelas disponíveis: 'conta', 'brainrot'");
            string? input = Console.ReadLine();
            string? tabelaSelecionada = input == null ? null : input.Trim();

            if (tabelaSelecionada == null || !VerificarStringInList(tabelas, tabelaSelecionada))
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }
            else
            {
                DefinirCampos(tabelaSelecionada);
                SelectCampos(tabelaSelecionada, connection);
            }
        }
        
        else if (option == 3)
        {
            // 3. ATUALIZAR DADOS
            Console.WriteLine($"\nDigite em qual tabela deseja {significadoOpcao[2]}: ");
            Console.WriteLine("Tabelas disponíveis: 'conta', 'brainrot'");
            string? input = Console.ReadLine();
            string? tabelaSelecionada = (input == null ? null : input.Trim());

            if (tabelaSelecionada == null || !VerificarStringInList(tabelas, tabelaSelecionada))
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }
            else
            {
                DefinirCampos(tabelaSelecionada);
                UpdateCampos(tabelaSelecionada, connection);
            }
        }
        
    }
    public void DefinirCampos(string tabelaSelecionada)
    {
        if (tabelaSelecionada == tabelas[0])
        {
            campos =
            [
                "nome",
                "espaco_total",
                "espaco_usado"
            ];
        }
        else
        {
            campos =
            [
                "nome",
                "raridade",
                "efeito"
            ];
        }
    }
    public void InsertCampos(string tabelaSelecionada, MySqlConnection connection)
    {
        string insertSql = $"INSERT INTO `{tabelaSelecionada}` ({GerarCamposSql(campos, false)}) VALUES ({GerarCamposSql(campos, true)})";
        using (var insertCommand = new MySqlCommand(insertSql, connection))
        {
            int totalInseridos = 0;
            char continuarInserindo = 'n';
            do
            {
                insertCommand.Parameters.Clear();

                foreach (var campo in campos)
                {
                    Console.Write($"Digite um valor para inserir no campo {campo}, tabela {tabelaSelecionada}: ");
                    var valorCampo = Console.ReadLine();

                    insertCommand.Parameters.AddWithValue($"@{campo}", valorCampo);
                }

                try
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    totalInseridos += rowsAffected;
                    Console.WriteLine($"{totalInseridos} linha(s) inserida(s)!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }

                Console.Write("\nDeseja continuar inserindo? (S/n): ");
                string? resposta = Console.ReadLine();
                continuarInserindo = string.IsNullOrEmpty(resposta) ? 'n' : resposta[0];

            } while (continuarInserindo == 's' || continuarInserindo == 'S');
        }
    }
    public void SelectCampos(string tabelaSelecionada, MySqlConnection connection)
    {
        string selectSql = $"SELECT `{GerarCamposSql(campos, false)}` FROM `{tabelaSelecionada}`";
        using (var selectCommand = new MySqlCommand(selectSql, connection))
        using (var reader = selectCommand.ExecuteReader())
        {
            Console.WriteLine($"\nDados na tabela '{tabelaSelecionada}':");
            while (reader.Read())
            {
                foreach (var campo in campos)
                {
                    Console.Write($"| {campo}: {reader[$"{campo}"]} |\n");
                }
            }
        }
    }
    public void UpdateCampos(string tabelaSelecionada, MySqlConnection connection)
    {
        string updateSql = $"UPDATE `{GerarCamposSql(campos, false)}` FROM `{tabelaSelecionada}`";
        using (var selectCommand = new MySqlCommand(updateSql, connection))
        {

        }
    }
    */
}
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
        using (var conexao = ConexaoBanco())
        {
            Console.Clear();

            string tabelaSelecionadaValidada = PegarEValidarTabela();
            List<string> camposTabelaSelecionada = PegarCamposTabelaSelecionada(tabelaSelecionadaValidada);
            
            PegarEValidarEAtribuirValoresCampos(tabelaSelecionadaValidada, camposTabelaSelecionada);

            string stringComandoInsert = CriarComandoInsert(tabelaSelecionadaValidada, camposTabelaSelecionada);

            using (var comandoInsert = new MySqlCommand(stringComandoInsert, conexao))
            {
                PropertyInfo[] propriedades;
                object instancia;
                Console.WriteLine("debug");
                switch (tabelaSelecionadaValidada)
                {
                    case "conta":
                        propriedades = conta.GetType().GetProperties();
                        /*
                        propriedades = propriedades.OrderBy(p => p.Name switch
                        {
                            "Id" => 0,
                            "Nome" => 1, 
                            "EspacosTotais" => 2,
                            "EspacosOcupados" => 3,
                            _ => 4
                        }).ToArray();
                        */
                        instancia = conta;
                        break;
                    case "brainrot":
                        propriedades = brainrot.GetType().GetProperties();
                        /*
                        propriedades = propriedades.OrderBy(p => p.Name switch
                        {
                            "Id" => 0,
                            "IdConta" => 1,
                            "Nome" => 2,
                            "Raridade" => 3,
                            "Efeito" => 4,
                            _ => 5
                        }).ToArray();
                        */
                        instancia = brainrot;
                        break;
                    default:
                        propriedades = null;
                        instancia = null;
                        Utilidades.MostrarErro();
                        break;
                }

                if (propriedades != null)
                {
                    try
                    {
                        for (int i = 0; i < camposTabelaSelecionada.Count; i++)
                        {
                            // Console.WriteLine(camposTabelaSelecionada[i]);
                            // Console.WriteLine(propriedades[i + 1]);
                            object valor = propriedades[i + 1].GetValue(instancia);
                            comandoInsert.Parameters.AddWithValue($"@{camposTabelaSelecionada[i + 1]}", valor);
                        }
                        comandoInsert.ExecuteNonQuery();

                        Console.WriteLine("Registrando dados");
                        Thread.Sleep(800);
                        Console.Clear();
                        Console.WriteLine("Dados Registrados!");
                        Utilidades.Retornar();
                    }
                    catch
                    {
                        Console.WriteLine("Problema");
                        Utilidades.MostrarErro();
                    }
                }
            }
        }
    }
    public MySqlConnection ConexaoBanco()
    {
        var connection = new MySqlConnection(_stringConexao);

        bool estaConectado = TestarConexao();

        if (!estaConectado)
        {
            Utilidades.MostrarErro();
            throw new Exception("Falha na conexão com o banco");
        }

        connection.Open();
        Console.WriteLine("Conectado ao MySQL com sucesso!");
        Thread.Sleep(800);
        Console.Clear();

        return connection;
    }
    public bool TestarConexao()
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
    private string CriarComandoInsert(string tabelaSelecionadaValidada, List<string> camposTabelaSelecionada)
    {
        string stringComandoInsert = $"INSERT INTO {tabelaSelecionadaValidada} ({GerarCamposComandoSql(camposTabelaSelecionada)}) VALUES ({GerarParametrosComandoSql(camposTabelaSelecionada)})";
        // Console.WriteLine($"\nO comando gerado foi esse: {stringComandoInsert}\n");

        return stringComandoInsert;
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
                Thread.Sleep(800);
                Console.Clear();
                return tabelaEscritaValidada;
            }
            Utilidades.MostrarErro();
        }
    }
    private string? PegarTabela()
    {
        Console.WriteLine($"Digite qual tabela deseja selecionar: ");
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
                "Id",
                "Nome",
                "EspacosTotais",
                "EspacosOcupados"
            ];
        }
        else if (tabelaSelecionadaValidada == "brainrot")
        {
            camposTabelaSelecionada =
            [
                "Id",
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
                if (campo == "Id")
                {
                    break;
                }

                string? valorCampoEscrito = PegarValorCampo(campo);

                string? valorCampoEscritoValidado = ValidarValorCampo(tabelaSelecionadaValidada, campo, valorCampoEscrito);

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
    private string? PegarValorCampo(string campo)
    {
        Console.Write($"Digite qual valor colocar no campo {campo}: ");
        return Console.ReadLine();
    }
    private string? ValidarValorCampo(string tabelaSelecionadaValidada, string campo, string? valorCampoEscrito)
    {
        Type tipoPropriedade;
        switch (tabelaSelecionadaValidada)
        {
            case "conta":
                tipoPropriedade = conta.GetType().GetProperty(campo).PropertyType;
                break;
            case "brainrot":
                tipoPropriedade = brainrot.GetType().GetProperty(campo).PropertyType;
                break;
            default:
                tipoPropriedade = null;
                Utilidades.MostrarErro();
                break;
        }
        
        string patternVerificador = @"[a-zA-Z0-9]";
        string patternCerto = @"[^a-zA-Z0-9]";

        if (tipoPropriedade != valorCampoEscrito.GetType() || Regex.Replace(valorCampoEscrito, patternVerificador, "").Length > 0)
        {
            Console.WriteLine("n passou do regex");
            return null;
        }
        Console.WriteLine("passou do regex");
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
                        throw new InvalidOperationException
                            ($"Tabela inesperada: {tabelaSelecionadaValidada}");
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

    // Select
    public void ExecutarComandoSelect()
    {
        using (var conexao = ConexaoBanco())
        {
            Console.Clear();

            string tabelaSelecionadaValidada = PegarEValidarTabela();
            List<string> camposTabelaSelecionada = PegarCamposTabelaSelecionada(tabelaSelecionadaValidada);

            string stringComandoSelect = CriarComandoSelect(tabelaSelecionadaValidada, camposTabelaSelecionada);

            using (var comandoSelect = new MySqlCommand(stringComandoSelect, conexao))
            using (var leitor = comandoSelect.ExecuteReader())
            {
                Console.WriteLine("Trazendo dados");
                Thread.Sleep(800);
                Console.Clear();

                MostrarSelect(leitor, tabelaSelecionadaValidada, camposTabelaSelecionada);
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
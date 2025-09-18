using MySql.Data.MySqlClient;

public class Sql
{
    private readonly string _stringConexao;
    public Sql(string stringConexao)
    {
        _stringConexao = stringConexao;
    }

    public void ComandoInsert()
    {
        PegarEValidarTabela();
    }
    private string PegarEValidarTabela()
    {
        while (true)
        {
            string? tabelaEscrita = PegarTabela();
            string? tabelaEscritaValidada = ValidarPegarTabela(tabelaEscrita);
            if (tabelaEscritaValidada != null)
            {
                return tabelaEscritaValidada;
            }
        }
    }
    private string? PegarTabela()
    {
        Console.WriteLine($"\nDigite em qual tabela deseja fazer um INSERT: ");
        Console.WriteLine("Tabelas disponíveis: 'conta', 'brainrot'");
        return Console.ReadLine();
    }
    private string? ValidarPegarTabela(string? tabelaEscrita)
    {
        tabelaEscrita = tabelaEscrita == null ? null : tabelaEscrita.Trim();

        if (tabelaEscrita == null || !(tabelaEscrita == "conta") || !(tabelaEscrita == "brainrot"))
        {
            Console.WriteLine("Erro ao digitar uma Tabela\nTabela inválida!");
            Console.WriteLine("Digite qualquer tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
            return null;
        }
        return tabelaEscrita;
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
    public string GerarCamposSql(List<string> campos, bool parametro)
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
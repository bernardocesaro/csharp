public static class ConverterUmParaDois
{
    public static string ConverterPascalToSnakeCase(string nomePropriedade)
    {
        var sb = new StringBuilder(nomePropriedade.Length + 5);
        sb.Append(char.ToLower(nomePropriedade[0]));

        for (int i = 1; i < nomePropriedade.Length; i++)
        {
            char caractereAtual = nomePropriedade[i];
            char caractereAnterior = nomePropriedade[i - 1];

            if (char.IsUpper(caractereAtual))
            {
                if (char.IsLower(caractereAnterior) ||
                   (i + 1 < nomePropriedade.Length && char.IsLower(nomePropriedade[i + 1])))
                {
                    sb.Append('_');
                }
                sb.Append(char.ToLower(caractereAtual));
            }
            else
            {
                sb.Append(caractereAtual);
            }
        }
        return sb.ToString();
    }
    public static string ConverterListToStringParametro(List<string> _camposTabelaSnakeCase, bool comParametro)
    {
        string _camposTabelaSnakeCaseParametro;
        if (comParametro)
        {
            _camposTabelaSnakeCaseParametro = "@" + string.Join(",@", _camposTabelaSnakeCase);
        }
        else
        {
            _camposTabelaSnakeCaseParametro = string.Join(",", _camposTabelaSnakeCase);
        }
        return _camposTabelaSnakeCaseParametro;
    }
}
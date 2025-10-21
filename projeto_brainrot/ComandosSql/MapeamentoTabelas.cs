public enum OpcaoTabela { Conta = 1, Brainrot = 2, Sair = 99 }

public static class MapeamentoTabelas
{
    public static readonly Dictionary<OpcaoTabela, Type> TabelasValidas = new Dictionary<OpcaoTabela, Type>()
    {
        { OpcaoTabela.Conta, typeof(Conta) },
        { OpcaoTabela.Brainrot, typeof(Brainrot) },
        { OpcaoTabela.Sair, typeof(string) }
    };
}
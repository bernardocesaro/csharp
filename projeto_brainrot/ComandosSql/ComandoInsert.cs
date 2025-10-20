public class ComandoInsert : ComandoBaseSql
{
    protected readonly string _stringConexao;
    public ComandoInsert(string stringConexao) : base(stringConexao) { }

    public override void Executar() {}
}
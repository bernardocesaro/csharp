public static class Utilidades
{
    public static void MostrarErro()
    {
        Console.WriteLine("Erro...");
        Retornar();
    }
    public static void Retornar()
    {
        Console.WriteLine("Digite qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
}
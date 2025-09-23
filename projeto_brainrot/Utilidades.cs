
public static class Utilidades
{
    public static void MostrarErro()
    {
        Console.WriteLine("\nErro!");
        Retornar();
    }
    public static void Retornar()
    {
        Console.WriteLine("Digite qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
}
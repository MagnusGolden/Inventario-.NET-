// Serializacion de Datos en XML, Binario, Json y SQLite

using AppInventario.DLL;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Iniciando sistema...\n");
        
        try
        {
            
            Sistema.IniciarSistema();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fatal al iniciar el sistema: {ex.Message}");
        }

        Console.WriteLine("\nPrograma finalizado.");
    }
}
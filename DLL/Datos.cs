// Aqui se llama a los metodos de guardado de los repositorios,
// utilizando la ruta cargada en el método Inicializar() de cada repositorio.

using AppInventario.DAL;

namespace AppInventario.DLL
{
    public class Datos
    {
        public static bool GuardarTodo() // A pesar de llamarse guardar o cargar todo, solo maneja archivos y no opciones.
        {
            bool inventarioGuardado = RepoInventario.GuardarInventario();
            bool ordenesGuardadas = RepoOrdenes.GuardarOrdenes();
            
            if (inventarioGuardado && ordenesGuardadas)
            {
                Console.WriteLine("Todos los datos fueron guardados exitosamente.");
                return true;
            }
            else
            {
                if (Opciones.DebugMode)
                {
                    if (!inventarioGuardado)
                        Console.WriteLine("Error al guardar el inventario.");
                    if (!ordenesGuardadas)
                        Console.WriteLine("Error al guardar las órdenes.");
                }
                Console.WriteLine("Error al guardar algunos datos.");
                return false;
            }
        }
        public static bool CargarTodo()
        {
                bool inventarioCargado = RepoInventario.CargarInventario();
                bool ordenesCargadas = RepoOrdenes.CargarOrdenes();
                
                if (inventarioCargado && ordenesCargadas)
                {
                    Console.WriteLine("Todos los datos fueron cargados exitosamente.");
                    return true;
                }
                else
                {
                    if (Opciones.DebugMode)
                    {
                        if (!inventarioCargado)
                            Console.WriteLine("Error al cargar el inventario.");
                        if (!ordenesCargadas)
                            Console.WriteLine("Error al cargar las órdenes.");
                    }
                    Console.WriteLine("Error al cargar algunos datos.");
                    return false;
                }
        }
    }
}
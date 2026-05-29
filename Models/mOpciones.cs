// Archivo para definir las opciones del sistema y las rutas.

namespace AppInventario.DLL
{
    public static class Opciones
    {
        // Rutas de los archivos de datos
        public static string RutaProductos { get; set; } = string.Empty;
        public static string RutaOrdenes { get; set; } = string.Empty;

        // Opciones de configuración
        // public static bool AutoOrdenes { get; set; } = false;     // Generación automática de órdenes
        // public static bool rawBackup { get; set; } = false;  // Copia de seguridad en formato war
        public static char FileFormat { get; set; } = 'j';         // J = Json, X = XML, B = Binary
        public static bool DebugMode { get; set; } = false;        // Modo de depuración
    }
}
/* 
    * En este archivo se encuentra la función "IniciarSistema()"
    * Lo primero es la lectura del archivo config.json que contiene las rutas de los archivos
    * Tras eso inicializa los repositorios en la app
    * Lo siguiente es la carga de datos desde el método "CargarDatos()"
    * Tras eso se llama al menú principal (ubicado en UI_Menu.cs)
*/

using System.Text.Json;
using System.Globalization;
using AppInventario.DAL;
using AppInventario.UI;
using AppInventario.Models;

namespace AppInventario.DLL
{
    public class Sistema
    {
        public static void IniciarSistema()
        {
            try
            {
                // Intento de lectura del archivo config.json, ubicado en la raiz.
                string pathConfig = "config.json";
                if (!File.Exists(pathConfig))
                {
                    // Crea un archivo de configuración que por defecto es json y usa las rutas base.
                    var defaultConfig = new Dictionary<string, object>
                    {
                        { "RutaProductos", "data/productos.json" },
                        { "RutaOrdenes", "data/ordenes.json" },
                        { "FileFormat", "j" }, // json
                        //{ "AutoOrdenes", false }, 
                        //{ "rawBackup", false } // Removidos por desuso
                        { "DebugMode", false } // Mensajes de depuración
                    };
                    File.WriteAllText(pathConfig, JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));

                    Console.WriteLine("Opciones no detectadas. Usando valores por defecto.");
                }
                
                // Carga de las opciones al sistema.
                string opcionesJson = File.ReadAllText(pathConfig);
                var configData = JsonSerializer.Deserialize<Dictionary<string, object>>(opcionesJson) ?? new Dictionary<string, object>();

                // Cargar opciones en la clase estática
                if (configData.TryGetValue("RutaProductos", out var rutaProductos))
                        Opciones.RutaProductos = rutaProductos?.ToString() ?? string.Empty;
                    
                if (configData.TryGetValue("RutaOrdenes", out var rutaOrdenes))
                        Opciones.RutaOrdenes = rutaOrdenes?.ToString() ?? string.Empty;
                    
                if (configData.TryGetValue("FileFormat", out var fileFormat))
                        Opciones.FileFormat = (fileFormat?.ToString() ?? "j")[0];

                if (configData.TryGetValue("DebugMode", out var debugMode))
                        Opciones.DebugMode = bool.Parse(debugMode?.ToString() ?? "false");

                // Carga inicial de los repositorios
                RepoInventario.Inicializar(Opciones.RutaProductos);
                RepoOrdenes.Inicializar(Opciones.RutaOrdenes);

                Console.WriteLine("Configuración cargada correctamente.");
                Console.WriteLine($"Formato: {Opciones.FileFormat} | Ruta Productos: {Opciones.RutaProductos} | Ruta Órdenes: {Opciones.RutaOrdenes}\n");

                // Primero se verifica la existencia de los datos
                bool recienCreados = false;
                try 
                {
                    if (!File.Exists(Opciones.RutaProductos))
                    {
                        Console.WriteLine($"Archivo de productos no encontrado en {Opciones.RutaProductos}. Creando archivo vacío.");
                        File.WriteAllText(Opciones.RutaProductos, "[]"); // Crear un archivo JSON vacío
                        recienCreados = true;
                    }
                    else if (Opciones.DebugMode)
                    {
                        Console.WriteLine("Archivo de productos detectado.");
                    }

                    if (!File.Exists(Opciones.RutaOrdenes))
                    {
                        Console.WriteLine($"Archivo de órdenes no encontrado en {Opciones.RutaOrdenes}. Creando archivo vacío.");
                        File.WriteAllText(Opciones.RutaOrdenes, "[]"); // Crear un archivo JSON vacío
                        recienCreados = true;
                    }
                    else if (Opciones.DebugMode)
                    {
                        Console.WriteLine("Archivo de órdenes detectado.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al verificar/crear archivos: {ex.Message}");
                }
                bool inventarioVacio = new FileInfo(Opciones.RutaProductos).Length == 0;
                bool ordenesVacio = new FileInfo(Opciones.RutaOrdenes).Length == 0;
                // Ahora se intentan cargar
                if (inventarioVacio || ordenesVacio) // No hay nada que cargar.
                {
                    if (Opciones.DebugMode){
                        Console.WriteLine("Archivos detectados pero vacios");
                    }
                }
                else if (recienCreados) // Se acaban de crear, no hay nada que cargar.
                {
                    if (Opciones.DebugMode){
                        Console.WriteLine("Archivos recién creados, sin datos para cargar");
                    }
                }
                else
                {
                    Console.WriteLine("Cargando datos...");
                    Datos.CargarTodo();
                }

                // Mostrar menú principal
                Console.WriteLine("\nSistema iniciado\n");
                Menu.MostrarMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar el sistema: {ex.Message}");
            }
        }
        public static void GuardarSistema()
        {
            try
            {
                File.WriteAllText("config.json", JsonSerializer.Serialize(new Dictionary<string, object>
                {
                    { "RutaProductos", Opciones.RutaProductos },
                    { "RutaOrdenes", Opciones.RutaOrdenes },
                    { "FileFormat", Opciones.FileFormat },
                    { "DebugMode", Opciones.DebugMode }
                }, new JsonSerializerOptions { WriteIndented = true }));
                if (Opciones.DebugMode)
                {
                    Console.WriteLine("Opciones guardadas en config.json");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el sistema: {ex.Message}");
            }
        }
        public static void debug()
        {
            // Se lee el archivo template.txt
            // Que contendra 100 o asi productos
            // ID,Nombre,StockActual,StockMaximo,StockMinimo,Precio

            if (!File.Exists("template.txt"))
            {
                Console.WriteLine("template.txt no encontrado.");
                return;
            }

            var lines = File.ReadAllLines("template.txt");
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var partes = line.Split(',');
                if (partes.Length < 6)
                {
                    Console.WriteLine($"Línea inválida (campos insuficientes): {line}");
                    continue;
                }

                try
                {
                    string id = partes[0].Trim();
                    string nombre = partes[1].Trim();

                    // Cree la lista usando puntos pra lso decilames, pero el programa no los detectaba
                    // Asi que se agregó el CultureInfo.InvariantCulture para asegurar que se usen puntos como separador de decimales
                    if (!decimal.TryParse(partes[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var stockActualDecimal))
                    {
                        Console.WriteLine($"No se pudo parsear StockActual en la línea: {line}");
                        continue;
                    }

                    int stockMaximoInt;
                    if (!int.TryParse(partes[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out stockMaximoInt))
                    {
                        // Soporta valores como "100.0" convirtiéndolos desde decimal
                        if (decimal.TryParse(partes[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var tmpMax))
                            stockMaximoInt = (int)tmpMax;
                        else
                        {
                            Console.WriteLine($"No se pudo parsear StockMaximo en la línea: {line}");
                            continue;
                        }
                    }

                    int stockMinimoInt;
                    if (!int.TryParse(partes[4].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out stockMinimoInt))
                    {
                        if (decimal.TryParse(partes[4].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var tmpMin))
                            stockMinimoInt = (int)tmpMin;
                        else
                        {
                            Console.WriteLine($"No se pudo parsear StockMinimo en la línea: {line}");
                            continue;
                        }
                    }

                    if (!decimal.TryParse(partes[5].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
                    {
                        Console.WriteLine($"No se pudo parsear Precio en la línea: {line}");
                        continue;
                    }

                    var producto = new mProducto
                    {
                        ID = id,
                        Nombre = nombre,
                        StockActual = stockActualDecimal,
                        StockMaximo = stockMaximoInt,
                        StockMinimo = stockMinimoInt,
                        Precio = precio
                    };

                    RepoInventario.AgregarProducto(producto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar la entrada: {ex.Message}");
                }
                Console.WriteLine("Datos de prueba cargados desde template.txt");
            }
        }
    }
}
// Repositorio utilizado para la carga y generación de productos por el programa.

using System.Text.Json;
using AppInventario.DLL;
using AppInventario.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
#pragma warning disable CS8600

namespace AppInventario.DAL
{
    public class RepoInventario
    {
        private static List<mProducto> inventario = new List<mProducto>();
        public static string rutaData = string.Empty;
        // Crea el repositorio de inventario con la ruta del archivo de datos
        public static void Inicializar(string ruta)
        {
            rutaData = ruta;
            Opciones.RutaProductos = ruta;

        }

        public static bool CargarInventario()
        {
            try
            {
                // La existencia del archivo ya debio ser verificada de antemano

                string contenido = File.ReadAllText(rutaData);
                switch (Opciones.FileFormat)
                {
                    case 'j':
                        var productosJson = JsonSerializer.Deserialize<List<mProducto>>(contenido);
                        if (productosJson != null)
                        {
                            inventario = productosJson;
                            Console.WriteLine($"Inventario cargado desde JSON. Total de productos: {inventario.Count}");
                            return true;
                        }
                        return false;
                    case 'x':
                        var xmlSerializer = new XmlSerializer(typeof(List<mProducto>));
                        using (var reader = new StreamReader(rutaData))
                        {
                            var productosXml = (List<mProducto>)xmlSerializer.Deserialize(reader);
                            if (productosXml != null)
                            {
                                inventario = productosXml;
                                Console.WriteLine($"Inventario cargado desde XML. Total de productos: {inventario.Count}");
                                return true;
                            }
                            return false;
                        }
                    case 'b':
                        Console.WriteLine("En versiones anteriores de .NET, esta funcion tendria uso, pero tras ser deshabilitada por Microsoft en versiones modernas debido a la alta cantidad de problemas de seguridad que representaba, ya no puede ser utilizada en dia de hoy sin recurrir a metodos(apps, .dll, traductores) de terceros.");
						/*
						using (var fs = new FileStream(rutaData, FileMode.Open))
                        {
                            #pragma warning disable SYSLIB0011 // Esto mas que quitar el error, le permite a la aplicacion ser compilada
                            var formatter = new BinaryFormatter();
                            inventario = (List<mProducto>)formatter.Deserialize(fs);
                            #pragma warning restore SYSLIB0011 // Pues (nuevamente) fue eliminada por Microsoft en VSCODE.
                            Console.WriteLine($"Inventario cargado desde Binario. Total de productos: {inventario.Count}");
                            
                        }*/
						return false;
                    default:
                        Console.WriteLine("Formato de archivo no válido para cargar.");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el inventario: {ex.Message}");
                return false;
            }
        }

        public static bool GuardarInventario()
        {
            // Aqui lo escribo en crudo para ver los valores que deben recibir los archivos 
            File.WriteAllText("data/dataInventario.raw", string.Join(Environment.NewLine, inventario.Select(p => $"{p.ID},{p.Nombre},{p.StockActual},{p.Precio}")));
            try
            {
                switch (Opciones.FileFormat)
                {
                    case 'j': // JSON
                        string inventariojson = JsonSerializer.Serialize(inventario, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(rutaData, inventariojson);
                        return true;

                    case 'x': // XML
                        var xmlSerializer = new XmlSerializer(typeof(List<mProducto>));
                        using (var writer = new StreamWriter(rutaData))
                        {
                            xmlSerializer.Serialize(writer, inventario);
                        }
                        return true;

                    
					case 'b': // Binario 
                        Console.WriteLine("En versiones anteriores de .NET, esta funcion tendria uso, pero tras ser deshabilitada por Microsoft en versiones modernas debido a la alta cantidad de problemas de seguridad que representaba, ya no puede ser utilizada en dia de hoy sin recurrir a metodos(apps, .dll, traductores) de terceros.");
						/*
						using (var fs = new FileStream(rutaData, FileMode.Create))
                        {
                            #pragma warning disable SYSLIB0011 // Desactiva la alerta de obsolescencia
                            var formatter = new BinaryFormatter();
                            formatter.Serialize(fs, inventario);
                            #pragma warning restore SYSLIB0011
                        } 
						*/
                        return false;
					
                    default:
                        Console.WriteLine("Formato de archivo no válido.");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el inventario: {ex.Message}");
                return false;
            }
        }
        public static void AgregarProducto(mProducto producto)
        {
            if (producto is not null && !inventario.Any(p => p.ID == producto.ID))
            {
                inventario.Add(producto);
            }
        }

        public static void EliminarProducto(mProducto producto)
        {
            if (producto is not null)
            {
                inventario.Remove(producto);
            }
        }
        public static List<mProducto> ObtenerInventario()
        {
            return [.. inventario];
        }
        public static mProducto? BuscarPorID(string id)
        {
            return inventario.FirstOrDefault(p => p.ID == id);
        }
        public static bool ActualizarStock(string id, int nuevaCantidad)
        {
            try
            {
                var producto = BuscarPorID(id);
                if (producto is not null)
                {
                    producto.StockActual = nuevaCantidad;
                    return true;
                }
                Console.WriteLine($"Producto con ID {id} no encontrado para actualizar stock.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el stock: {ex.Message}");
                return false;
            }
    
        }
    }
}
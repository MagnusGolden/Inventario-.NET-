// Repositorio utilizado para la carga y generación de órdenes por el programa.
using AppInventario.Models;
using AppInventario.DLL;
using System.Text.Json;
using System.Xml.Serialization;
#pragma warning disable CS8600

namespace AppInventario.DAL
{
    public class RepoOrdenes
    {
        private static List<mOrdenes> ordenes = new List<mOrdenes>();

        public static string rutaData = string.Empty;

        public static void Inicializar(string ruta)
        {
            rutaData = ruta;
            Opciones.RutaOrdenes = ruta;
        }

        public static bool CargarOrdenes()
        {
            try
            {
                if (!File.Exists(rutaData))
                {
                    Console.WriteLine($"Archivo no encontrado: {rutaData}");
                    return false;
                }

                string contenido = File.ReadAllText(rutaData);
                switch (Opciones.FileFormat)
                {
                    case 'j':
                        var ordenesJson = JsonSerializer.Deserialize<List<mOrdenes>>(contenido);
                        if (ordenesJson != null)
                        {
                            ordenes = ordenesJson;
                            Console.WriteLine($"Órdenes cargadas desde JSON. Total de órdenes: {ordenes.Count}");
                            return true;
                        }
                        return false;
                    case 'x':
                        var xmlSerializer = new XmlSerializer(typeof(List<mOrdenes>));
                        using (var reader = new StreamReader(rutaData))
                        {
                            var ordenesXml = (List<mOrdenes>)xmlSerializer.Deserialize(reader);
                            if (ordenesXml != null)
                            {
                                ordenes = ordenesXml;
                                Console.WriteLine($"Órdenes cargadas desde XML. Total de órdenes: {ordenes.Count}");
                                return true;
                            }
                            return false;
                        }
                    case 'b':
                        using (var fs = new FileStream(rutaData, FileMode.Open))
                        {
                            #pragma warning disable SYSLIB0011 // Desactiva la alerta de obsolescencia
                            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                            ordenes = (List<mOrdenes>)formatter.Deserialize(fs);
                            #pragma warning restore SYSLIB0011
                            Console.WriteLine($"Órdenes cargadas desde Binario. Total de órdenes: {ordenes.Count}");
                            return true;
                        }
                    default:
                        Console.WriteLine("Formato de archivo no válido para cargar órdenes.");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar órdenes: {ex.Message}");
                return false;
            }
        }

        public static bool GuardarOrdenes()
        {
            // escribir primero en raw
            File.WriteAllText("data/dataOrdenes.raw", string.Join(Environment.NewLine, ordenes.Select(o => $"{o.OrdenID},{o.ID},{o.Nombre},{o.Cantidad},{o.Precio}")));
            try
            {
                switch (Opciones.FileFormat)
                {
                    case 'j': // JSON
                        string ordenesJson = JsonSerializer.Serialize(ordenes, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(rutaData, ordenesJson);
                        return true;

                    case 'x': // XML
                        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<mOrdenes>));
                        using (var writer = new StreamWriter(rutaData))
                        {
                            xmlSerializer.Serialize(writer, ordenes);
                        }
                        return true;

                    case 'b': // Binario 
                        using (var fs = new FileStream(rutaData, FileMode.Create))
                        {
                            #pragma warning disable SYSLIB0011 // Debido a ser obsoleto, tengo que deshabilitar la alerta de obsolescencia
                            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                            formatter.Serialize(fs, ordenes);
                            #pragma warning restore SYSLIB0011
                        }
                        return true;
                    default:
                        Console.WriteLine("Formato de archivo no válido para guardar órdenes.");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar órdenes: {ex.Message}");
                return false;
            }
        }

        public static void CrearOrden(mOrdenes orden)
        {
            ordenes.Add(orden);
        }
        public static List<mOrdenes> ObtenerOrdenes()
        {
            return [.. ordenes];
        }

        public static mOrdenes? BuscarPorID(string id)
        {
            id = id.ToUpper();
            return ordenes.FirstOrDefault(o => o.ID == id);
        }
    }
}
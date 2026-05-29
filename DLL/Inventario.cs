// Lógica de negocio para gestionar el inventario
using AppInventario.DAL;
using AppInventario.Models;
using AppInventario.UI;
#pragma warning disable CS8604

namespace AppInventario.DLL
{
    public static class Inventario
    {
        const int ITEMS_POR_PAGINA = 20; // Delimita la cantidad de productos que se ven en cada pagina.
        public static void Listar(List<mProducto> productos, bool listar) // Tanto para intentario completo como stock bajo
        {
            
            if (productos.Count == 0)
            {
                Console.WriteLine("El inventario está vacío.");
                return;
            }

            int totalPaginas = (int)Math.Ceiling((double)productos.Count / ITEMS_POR_PAGINA);
            int paginaActual = 1;
            bool navegando = true;

            while (navegando)
            {
                Console.WriteLine("═════════════════════════════════════════════════════════════════════════════════════");
                Console.WriteLine("                          === INVENTARIO ACTUAL ===");
                Console.WriteLine("═════════════════════════════════════════════════════════════════════════════════════");

                Console.WriteLine($"{"ID",-4} | {"Nombre",-20} | {"Stock Act.",-11} | {"Stock Mín.",-11} | {"Precio Unit.",-12} | {"Precio Total",-12}");
                Console.WriteLine("─────────────────────────────────────────────────────────────────────────────────────");
                
                // Obtener productos de la página actual
                var productosPagina = productos
                    .Skip((paginaActual - 1) * ITEMS_POR_PAGINA)
                    .Take(ITEMS_POR_PAGINA)
                    .ToList();

                // Mostrar productos
                foreach (var producto in productosPagina)
                {
                    Console.WriteLine($"{producto.ID,-4} | {producto.Nombre,-20} | {producto.StockActual,-11:F2} | {producto.StockMinimo,-11} | {producto.Precio,-12:F2} | {producto.ValorTotal,-12:F2}");
                }

                Console.WriteLine("═════════════════════════════════════════════════════════════════════════════════════");
                Console.WriteLine($"Página {paginaActual} de {totalPaginas} | Total de productos: {productos.Count}");
                Console.WriteLine("═════════════════════════════════════════════════════════════════════════════════════");
                
                // Opciones de navegación
                Console.WriteLine("\nOpciones:");
                if (paginaActual > 1)
                    Console.WriteLine("  [A] Página Anterior");
                if (paginaActual < totalPaginas)
                    Console.WriteLine("  [S] Página Siguiente");
                Console.WriteLine("  [V] Volver al Menú");
                
                if (!listar) {
                    Console.WriteLine("  [G] Generar orden de compra para productos con stock bajo");
                } else {
                    Console.WriteLine("  [L] Listar solo productos con stock bajo");
                }
                Console.Write("\nSeleccione una opción: ");

                string opcion = Console.ReadLine()?.ToUpper() ?? "";

                switch (opcion)
                {
                    case "A":
                        if (paginaActual > 1)
                        {
                            paginaActual--;
                        }
                        else
                        {
                            Console.WriteLine("Ya estás en la primera página.");
                            Console.ReadKey();
                        }
                        break;

                    case "S":
                        if (paginaActual < totalPaginas)
                        {
                            paginaActual++;
                        }
                        else
                        {
                            Console.WriteLine("Ya estás en la última página.");
                            Console.ReadKey();
                        }
                        break;

                    case "V":
                        navegando = false;
                        break;

                    case "L":
                        listar = false;
                        productos = ListarStockBajo();
                        break;
                    case "G":
                        if (!listar)
                        {
                            foreach (var producto in productos)
                            {
                                mOrdenes orden = new mOrdenes
                                {
                                    ID = producto.ID,
                                    Nombre = producto.Nombre,
                                    Cantidad = producto.StockMinimo - producto.StockActual,
                                    Precio = producto.Precio,
                                    Estado = "Pendiente"
                                };
                                Ordenes.OrdenArgregarProducto(orden);
                            }
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Primero debes listar los productos con stock bajo para generar la orden de compra.");
                            Console.ReadKey();
                        }
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static bool ActualizarStockProducto(string id, decimal nuevaCantidad)
        {
            try
            {
                var producto = RepoInventario.BuscarPorID(id);
                if (producto is not null)
                {
                    producto.StockActual = nuevaCantidad;
                    return true;
                }
                Console.WriteLine($"Producto con ID {id} no encontrado.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar stock: {ex.Message}");
                return false;
            }
        }

        public static bool AgregarProducto(mProducto producto)
        {
            try
            {
                if (producto == null)
                {
                    Console.WriteLine("El producto no puede ser nulo.");
                    return false;
                }

                var existente = RepoInventario.BuscarPorID(producto.ID);
                if (existente != null)
                {
                    Console.WriteLine($"El producto con ID {producto.ID} ya existe.");
                    return false;
                }

                RepoInventario.AgregarProducto(producto);
                Console.WriteLine($"Producto {producto.Nombre} agregado exitosamente.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar producto: {ex.Message}");
                return false;
            }
        }
        public static bool EliminarProducto(string id)
        {
            try
            {
                var producto = RepoInventario.BuscarPorID(id);
                if (producto != null)
                {
                    RepoInventario.EliminarProducto(producto);
                    Console.WriteLine($"Producto {producto.Nombre} eliminado exitosamente.");
                    return true;
                }
                Console.WriteLine($"Producto con ID {id} no encontrado.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                return false;
            }
        }
        public static mProducto? BuscarPorID(string id)
        {
            return RepoInventario.BuscarPorID(id);
        }

        public static List<mProducto> ListarStockBajo()
        {
            var productos = RepoInventario.ObtenerInventario();
            return productos.Where(p => p.StockActual < p.StockMinimo).ToList();
        }
    }
}
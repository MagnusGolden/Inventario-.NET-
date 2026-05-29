// Menu principal del programa
using AppInventario.DLL;
using AppInventario.Models;
using AppInventario.DAL;
#pragma warning disable CS8600

namespace AppInventario.UI
{
    public static class Menu
    {
        public static List<mOrdenes> ordenActual = new List<mOrdenes>();
        public static void MostrarMenu()
        {
            
            bool salir = false;
            
            while (!salir)
            {

                Console.WriteLine("\n=== MENÚ PRINCIPAL INVENTARIO ===");
                Console.WriteLine("1. Inventario");
                Console.WriteLine("2. Ordenes");
                Console.WriteLine("3. Opciones");
                Console.WriteLine("0. Guardar y Salir");
                Console.WriteLine("99. Salir sin guardar");
                Console.WriteLine("=================================");
                Console.Write("\nSelecciona una opción: ");
                
                var opcion = Console.ReadLine();
                
                switch (opcion)
                {
                    case "1":
                        MenuInventario();
                        break;
                    case "2":
                        MenuOrdenes();
                        break;
                    case "3":
                        MenuOpciones();
                        break;
                    case "0":
                        salir = true;
                        Console.WriteLine("Saliendo del programa...");
                        Datos.GuardarTodo();
                        break;
                    case "99":
                        salir = true;
                        Console.WriteLine("Saliendo sin guardar...");
                        break;
                    case "debug":
                        MenuDebug();
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, selecciona una opción del menú.");
                        break;
                }
            }
        }
        public static void MenuInventario()
        {
            Console.WriteLine("=== MENÚ DE INVENTARIO ===");
            Console.WriteLine("1. Listar Inventario");
            Console.WriteLine("2. Agregar Producto");
            Console.WriteLine("3. Modificar Producto");
            Console.WriteLine("4. Eliminar Producto");
            Console.WriteLine("5. Listar Productos con Stock Bajo");
            Console.WriteLine("0. Volver al Menú Principal");
            Console.WriteLine("==========================");
            Console.Write("\nSelecciona una opción: ");
            
            string opcion = Console.ReadLine();
            
            switch (opcion)
            {
                case "1":
                    bool lista = true;
                    Inventario.Listar(RepoInventario.ObtenerInventario(), lista);
                    break;
                case "2":
                    Console.WriteLine("=== AGREGAR NUEVO PRODUCTO ===");
                    
                    Console.Write("Ingrese el ID del producto (1 Letra, 3 numero): ");
                    string id = Console.ReadLine() ?? "";
                    while (!System.Text.RegularExpressions.Regex.IsMatch(id, @"^[A-Za-z]\d{3}$"))
                    {
                        Console.Write("ID inválido. Ingrese un ID con 1 letra seguida de 3 números (ej: A123): ");
                        id = Console.ReadLine() ?? "";
                    }
                    if (Inventario.BuscarPorID(id) != null)
                    {
                        Console.WriteLine($"El ID {id} ya existe. Por favor, elige un ID diferente.");
                        return;
                    }
                
                    Console.Write("Ingrese el nombre del producto: ");
                    string nombre = Console.ReadLine() ?? "";
                    
                    Console.Write("Ingrese la cantidad en stock: ");
                    decimal stockActual;
                    while (!decimal.TryParse(Console.ReadLine(), out stockActual) || stockActual < 0)
                    {
                        Console.Write("Cantidad inválida. Ingrese un número decimal positivo: ");
                    }
                    
                    Console.Write("Ingrese el precio unitario: ");
                    decimal precio;
                    while (!decimal.TryParse(Console.ReadLine(), out precio) || precio < 0)
                    {
                        Console.Write("Precio inválido. Ingrese un número decimal positivo: ");
                    }
                    
                    Console.Write("Ingrese el stock minimo: ");
                    int stockMinimo;
                    while (!int.TryParse(Console.ReadLine(), out stockMinimo) || stockMinimo < 0)
                    {
                        Console.Write("Stock mínimo inválido. Ingrese un número entero positivo: ");
                    }
                    Console.Write("Ingrese el stock maximo: ");
                    int stockMaximo;
                    while (!int.TryParse(Console.ReadLine(), out stockMaximo) || stockMaximo < 0 || stockMaximo <= stockMinimo)
                    {
                        Console.Write("Stock máximo inválido. Ingrese un número entero positivo o mayor al stock mínimo: ");
                    }
                    var nuevoProducto = new mProducto(id, nombre, stockActual , stockMaximo, stockMinimo, precio);
                    Inventario.AgregarProducto(nuevoProducto);
                    Console.WriteLine($"Producto {id} agregado exitosamente.");
                    break;
                case "3":
                    Console.Write("Ingrese el ID del producto a modificar: ");
                    string idModificar = Console.ReadLine() ?? "";
                    while (!System.Text.RegularExpressions.Regex.IsMatch(idModificar, @"^[A-Za-z]\d{3}$"))
                    {
                        Console.Write("ID inválido. Ingrese un ID con 1 letra seguida de 3 números (ej: A123): ");
                        idModificar = Console.ReadLine() ?? "";
                    }
                    var productoExistente = Inventario.BuscarPorID(idModificar);
                    if (productoExistente != null)
                    {
                        Console.Write("Ingrese el nombre del producto: ");
                        string mnombre = Console.ReadLine() ?? "";
                        
                        Console.Write("Ingrese la cantidad en stock: ");
                        decimal mstockActual;
                        while (!decimal.TryParse(Console.ReadLine(), out mstockActual) || mstockActual < 0)
                        {
                            Console.Write("Cantidad inválida. Ingrese un número decimal positivo: ");
                        }
                        
                        Console.Write("Ingrese el precio unitario: ");
                        decimal mprecio;
                        while (!decimal.TryParse(Console.ReadLine(), out mprecio) || mprecio < 0)
                        {
                            Console.Write("Precio inválido. Ingrese un número decimal positivo: ");
                        }
                        
                        Console.Write("Ingrese el stock minimo: ");
                        decimal mstockMinimo;
                        while (!decimal.TryParse(Console.ReadLine(), out mstockMinimo) || mstockMinimo < 0)
                        {
                            Console.Write("Stock mínimo inválido. Ingrese un número decimal positivo: ");
                        }
                        Console.Write("Ingrese el stock maximo: ");
                        decimal mstockMaximo;
                        while (!decimal.TryParse(Console.ReadLine(), out mstockMaximo) || mstockMaximo < 0 || mstockMaximo <= mstockMinimo)
                        {
                            Console.Write("Stock máximo inválido. Ingrese un número decimal positivo o mayor al stock mínimo: ");
                        }
                        Inventario.ActualizarStockProducto(idModificar, mstockActual);
                        Console.WriteLine($"Producto {idModificar} actualizado exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine($"Producto con ID {idModificar} no encontrado.");
                    }
                    break;
                case "4":
                    Console.Write("Ingrese el ID del producto a eliminar: ");
                    string idEliminar = Console.ReadLine() ?? "";
                    while (!System.Text.RegularExpressions.Regex.IsMatch(idEliminar, @"^[A-Za-z]\d{3}$"))
                    {
                        Console.Write("ID inválido. Ingrese un ID con 1 letra seguida de 3 números (ej: A123): ");
                        idEliminar = Console.ReadLine() ?? "";
                    }
                    Inventario.EliminarProducto(idEliminar);
                    break;
                case "5":
                    Inventario.Listar(Inventario.ListarStockBajo(), false);
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, selecciona una opción del menú.");
                    break;
                
            }
        }
        public static void MenuOrdenes()
        {
            Console.WriteLine("=== MENÚ DE ORDENES ===");
            Console.WriteLine("1. Ver Orden actual");
            Console.WriteLine("2. Listar ordenes guardadas");
            Console.WriteLine("3. Agregar prodcuto de compra");
            Console.WriteLine("4. Modificar orden actual");
            Console.WriteLine("5. Generar orden de compra");
            Console.WriteLine("0. Volver al Menú Principal");
            Console.WriteLine("========================");
            Console.Write("\nSelecciona una opción: ");
            
            string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Ordenes.ListarOrdenActual();
                        break;
                    case "2":
                        Ordenes.ListarOrdenesGuardadas();
                        break;
                    case "3":
                        AgregarProductoOrden();
                        break;
                    case "4":
                        ModificarOrdenActual();
                        break;
                    case "5":
                        Ordenes.GenerarOrdenesCompra(ordenActual);
                        break;
                    case "0":
                        MostrarMenu();
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, selecciona una opción del menú.");
                        break;
                }
        }
        
        public static void AgregarProductoOrden()
        {
            Console.WriteLine("Ingrese el ID del producto para generar la orden de compra: ");
            string idProducto = Console.ReadLine() ?? "";

            var producto = Inventario.BuscarPorID(idProducto);
            if (producto != null)
            {
                Console.WriteLine($"Producto encontrado: {producto.Nombre}, Stock Actual: {producto.StockActual}, Stock Mínimo: {producto.StockMinimo}");
                Console.WriteLine("Ingrese la cantidad a ordenar: ");
                int cantidad;
                while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0)
                {
                    Console.Write("Cantidad inválida. Ingrese un número entero positivo: ");
                }
                var orden = new mOrdenes
                {
                    ID = producto.ID,
                    Nombre = producto.Nombre,
                    Cantidad = cantidad,
                    Precio = producto.Precio
                };
                Ordenes.OrdenArgregarProducto(orden);
            }
            else
            {
                Console.WriteLine($"Producto con ID {idProducto} no encontrado.");
            }
        }

        static void ModificarOrdenActual()
        {
            if (ordenActual.Count > 0)
            {
                Console.WriteLine("Ingrese el ID del producto en la orden actual que desea modificar/eliminar: ");
                string idProducto = Console.ReadLine() ?? "";

                var orden = ordenActual.FirstOrDefault(o => o.ID == idProducto);
                if (orden != null)
                {
                    Console.WriteLine($"Producto encontrado en la orden actual: {orden.Nombre}, Cantidad: {orden.Cantidad}, Precio: {orden.Precio}");
                
                    Console.WriteLine("¿Desea eliminar este producto de la orden? (S/N)");
                    int eliminar = int.Parse(Console.ReadLine()?.ToUpper() == "S" ? "1" : "0");
                    switch (eliminar)
                    {
                        case 1: 
                            {
                                ordenActual.Remove(orden);
                                Console.WriteLine($"Producto {orden.Nombre} eliminado de la orden actual.");
                                break;
                            }
                        default:
                        {
                            Console.WriteLine("Ingrese la nueva cantidad a ordenar: ");
                            int nuevaCantidad;
                            while (!int.TryParse(Console.ReadLine(), out nuevaCantidad) || nuevaCantidad <= 0)
                            {
                                Console.Write("Cantidad inválida. Ingrese un número entero positivo: ");
                            }
                            orden.Cantidad = nuevaCantidad;
                            Console.WriteLine($"Cantidad del producto {orden.Nombre} actualizada a {nuevaCantidad} en la orden actual.");
                            break;
                        }
                    }
                } 
                else
                {
                    Console.WriteLine($"Producto con ID {idProducto} no encontrado en la orden actual.");
                }
                Console.WriteLine("Producto modificado en la orden actual.");
            } 
            else
            {
                Console.WriteLine("No hay órdenes pendientes.");
            }
        }
        public static void MenuOpciones()
        {
            Console.WriteLine("=== MENÚ DE OPCIONES ===");
            Console.WriteLine("1. Formato de archivo (actual: " + (Opciones.FileFormat == 'j' ? "JSON" : Opciones.FileFormat == 'x' ? "XML" : "BINARIO") + ")");
            Console.WriteLine("2. Cargar inventario y ordenes");
            Console.WriteLine("3. Guardar inventario y ordenes");
            Console.WriteLine("4. Guardar opciones");
            Console.WriteLine("0. Volver al Menú Principal");
            Console.WriteLine("=========================");
            Console.Write("\nSelecciona una opción: ");
            
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.WriteLine("Selecciona el formato de archivo:");
                    Console.WriteLine("1. JSON");
                    Console.WriteLine("2. XML");
                    Console.WriteLine("3. BINARIO");

                    string formato = Console.ReadLine();
                    switch (formato)
                    {
                        case "1":
                            Opciones.FileFormat = 'j';
                            Opciones.RutaProductos = "data/inventario.json";
                            Opciones.RutaOrdenes = "data/ordenes.json";

                            Console.WriteLine("Formato de archivo cambiado a JSON.");
                            break;
                        case "2":
                            Opciones.FileFormat = 'x';
                            Opciones.RutaProductos = "data/inventario.xml";
                            Opciones.RutaOrdenes = "data/ordenes.xml";
                            Console.WriteLine("Formato de archivo cambiado a XML.");
                            break;
                        case "3":
                            Opciones.FileFormat = 'b';
                            Opciones.RutaProductos = "data/inventario.bin";
                            Opciones.RutaOrdenes = "data/ordenes.bin";
                            Console.WriteLine("Formato de archivo cambiado a BINARIO.");
                            break;
                          default:
                              Console.WriteLine("Opción no válida. El formato de archivo no ha sido cambiado.");
                              break;
                    }
                    RepoInventario.Inicializar(Opciones.RutaProductos);
                    RepoOrdenes.Inicializar(Opciones.RutaOrdenes);
                    /*
                    // Tras cambiar de tipo de archivo, se puede utilizar los .raw para cargar directamente
                    // el Inventario sin serializar
                    Console.WriteLine("Al cambiar el formato de archivo, los datos existentes no serán compatibles. Asegúrate de guardar tu inventario actual.");
                    Console.WriteLine("¿Deseas cargar los archivos previos? (S/N)");
                    string cargarPrevios = Console.ReadLine()?.ToUpper() ?? "N";
                    if (cargarPrevios == "S")
                    {
                        Datos.CargarDatos();
                    }
                    */ // Si el usuario ya tenia datos cargados en el sistema, no es necesario la lectura de lso archivos raw.
                    break;
                case "2":
                    Console.WriteLine("Cargando datos...");
                    Datos.CargarTodo();
                    break;
                case "3":
                    Console.WriteLine("Guardando datos...");
                    Datos.GuardarTodo();
                    break;
                case "4":
                    Console.WriteLine("Guardando opciones...");
                    Sistema.GuardarSistema();
                    break;
                case "0":
                    MostrarMenu();
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, selecciona una opción del menú.");
                    break;
            }
        }
        public static void MenuDebug()
        {
            Console.WriteLine("=== MENÚ DE DEBUG ===");
            Console.WriteLine("1. Modo debug");
            // Console.WriteLine("2. Modo raw backup");
            Console.WriteLine("3. Cargar datos de ejemplo");
            Console.WriteLine("0. Volver al Menú Principal");
            Console.WriteLine("=====================");
            switch (Console.ReadLine())
            {
                case "1":
                    if (Opciones.DebugMode)
                    {
                        Opciones.DebugMode = false;
                    }
                    else
                    {
                        Opciones.DebugMode = true;
                    }
                    Console.WriteLine("Modo debug " + (Opciones.DebugMode ? "activado" : "desactivado") + ".");
                    break;
                case "2":
                    Console.WriteLine("Funcionalidad de backup en raw.");
                    break;
                case "3":
                    Console.WriteLine("Cargando datos de ejemplo...");
                    Sistema.debug();
                    break;
                case "0":
                    MostrarMenu();
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, selecciona una opción del menú.");
                    break;
            }
        }
    }
}


// Lógica de negocio para gestionar las órdenes de compra
// Muestra los productos que necesitan ser ordenados y permite gestionar el historial

using AppInventario.DAL;
using AppInventario.Models;
using AppInventario.UI;
using System.Globalization;

namespace AppInventario.DLL
{
    public static class Ordenes
    {
        public static List<mOrdenes> ObtenerOrdenes()
        {
            return RepoOrdenes.ObtenerOrdenes();
        }

        public static mOrdenes? BuscarOrdenPorID(string id)
        {
            return RepoOrdenes.BuscarPorID(id);
        }

        public static void ListarOrdenActual()
        {
            if (Menu.ordenActual.Count > 0)
            {
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════");
                Console.WriteLine("                          === Orden ACTUAL ===");
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════");

                Console.WriteLine($"{"ID",-4} | {"Nombre",-16} | {"Cantidad",-9} | {"Precio Unit.",-11} | {"Precio Total.",-12}");
                Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                decimal valorTotal = 0;
                    
                foreach (var orden in Menu.ordenActual)
                {
                    var costoTotal = orden.ObtenerCostoTotal(orden.Cantidad, orden.Precio);
                    Console.WriteLine($"{orden.ID,-4} | {orden.Nombre,-16} | {orden.Cantidad,-9:#,##0.00} | {orden.Precio,-11:#,##0.00} | {costoTotal,-12:#,##0.00}");
                    valorTotal += costoTotal;
                }
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════");
                Console.WriteLine("                   === Valor Total : RD$ " + valorTotal.ToString("#,##0.00", CultureInfo.InvariantCulture) + " ===");
                Console.WriteLine("                       === Estado: " + Menu.ordenActual[0].Estado + " ===");
                Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════");
            }
            else
            {
                Console.WriteLine("No hay órdenes pendientes.");
            }
            Console.ReadKey();
        }

        public static void OrdenArgregarProducto(mOrdenes orden)
        {
            Menu.ordenActual.Add(orden);
            Console.WriteLine($"Agregado: {orden.Nombre} (Cantidad: {orden.Cantidad})");
        }
        public static void GenerarOrdenesCompra(List<mOrdenes> ordenActual)
        {
            if (ordenActual.Count > 0)
            {
                try
                {
                    string ordenId = DateTime.Now.ToString("yyyyMMddHHmmss");
                    DateTime fechaCreacion = DateTime.Now;

                    foreach (var orden in ordenActual)
                    {
                        orden.OrdenID = ordenId;
                        orden.FechaCreacion = fechaCreacion;
                        RepoOrdenes.CrearOrden(orden);
                    }

                    Console.WriteLine($"Orden de compra generada exitosamente. ID de orden: {ordenId}");
                    ordenActual.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al generar la orden de compra: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No hay productos en la orden actual para generar una orden de compra.");
            }
        }

        public static void ListarOrdenesGuardadas()
        {
            var ordenes = RepoOrdenes.ObtenerOrdenes();
            if (ordenes.Count == 0)
            {
                Console.WriteLine("No hay órdenes guardadas.");
                return;
            }

            var gruposPorOrden = ordenes.GroupBy(o => o.OrdenID ?? "SIN_ID");
            foreach (var grupo in gruposPorOrden)
            {
                var primerItem = grupo.First();
                Console.WriteLine($"═════════════════════════════════════════════════════════════════════");
                Console.WriteLine($"Orden ID: {grupo.Key}| Fecha: {primerItem.FechaCreacion:yyyy-MM-dd HH:mm}  | Productos: {grupo.Count()}");
                Console.WriteLine($"═════════════════════════════════════════════════════════════════════");
                Console.WriteLine($"{"ID",-4} | {"Descripcion",-16} | {"Cantidad",-9} | {"Precio Unit.",-12} | {"Precio Total.",-12}");
                Console.WriteLine("─────────────────────────────────────────────────────────────────────");
                foreach (var orden in grupo)
                {
                    var costoTotal = orden.ObtenerCostoTotal(orden.Cantidad, orden.Precio);
                    Console.WriteLine($"{orden.ID,-4} | {orden.Nombre,-16} | {orden.Cantidad,-9:#,##0.00} | {orden.Precio,-12:#,##0.00} | {costoTotal,-12:#,##0.00}");
                }
                Console.WriteLine("═════════════════════════════════════════════════════════════════════");
                Console.WriteLine("══════════════════   Precio Total: RD$ " + grupo.Sum(o => o.ObtenerCostoTotal(o.Cantidad, o.Precio)).ToString("#,##0.00", CultureInfo.InvariantCulture) + "   ══════════════════");
            }
            Console.ReadKey();
        }
    }
}

// Modelo utilizado en los productos del inventario del supermercado
// Contiene: ID único, nombre del producto, stock actual/máximo/mínimo y precio

namespace AppInventario.Models
{
    [Serializable]
    public class mProducto
    {
        // Constructor sin parámetros para binario y xml
        public mProducto() { }

        public mProducto(string id, string nombre, decimal stockActual, int stockMaximo, int stockMinimo, decimal precio)
        {
            ID = id;
            Nombre = nombre;
            StockActual = stockActual;
            StockMaximo = stockMaximo;
            StockMinimo = stockMinimo;
            Precio = precio;
        }

        public string? ID { get; set; }
        // Valor de 4 dígitos donde el primero es una letra para indicar el tipo de producto.

        public string? Nombre { get; set; }
        // Descripción del producto.

        public decimal StockActual { get; set; }
        // Stock actual. Se utiliza decimal para productos medidos en libras (yuca, azúcar, etc).

        public int StockMaximo { get; set; }
        // Valor al que trata de llegar el stock al generar una orden de compra.

        public int StockMinimo { get; set; }
        // Valor utilizado por comprobarInventario para generar una orden de compra.

        public decimal Precio { get; set; }
        // Precio unitario del producto.

        public decimal ValorTotal => StockActual * Precio;
        // Propiedad calculada que devuelve el valor total del stock actual.
    }
}
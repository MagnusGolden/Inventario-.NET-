// Modelo utilizado para generar las órdenes de compra
// Contiene: ID único, nombre del producto, cantidad a ordenar, stock actual y precio


namespace AppInventario.Models
{
    [Serializable]
    
    public class mOrdenes
    {
        // Constructor sin parámetros para binario y xml.
        public mOrdenes() { }

        public mOrdenes(string ordenID, string id, string nombre, decimal cantidad, decimal precio, DateTime fechaCreacion)
        {
            OrdenID = ordenID;
            ID = id;
            Nombre = nombre;
            Cantidad = cantidad;
            Precio = precio;
            FechaCreacion = fechaCreacion;
        }

        public string? OrdenID { get; set; }
        // ID de la orden, se genera a partir del mes y dia de la orden, separado por un guion.

        public string? ID { get; set; }
        // Valor de 4 dígitos donde el primero es una letra para indicar el tipo de producto.

        public string? Nombre { get; set; }
        // Nombre o descripción del producto.

        public decimal Cantidad { get; set; }
        // Cantidad a pedir, obtenida por el cálculo (StockMaximo - StockActual).

        public decimal Precio { get; set; }
        // Precio unitario del producto.

        public DateTime FechaCreacion { get; set; }
        // Fecha y hora en que se creó la orden, quese utiliza para generar el ID de la orden.

        public string  Estado { get; set; } = "Pendiente";
        // Estado de la orden, puede ser "Pendiente", "En Proceso" o "Completada".

        public override string ToString()
        {
            return $"ID: {ID}, Nombre: {Nombre}, Cantidad: {Cantidad}, Precio Unitario: {Precio:C}, Fecha Creación: {FechaCreacion}";
        }

        public decimal ObtenerCostoTotal(decimal cantidad, decimal precio)
        {
            return cantidad * precio;
        }
    }
}
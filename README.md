# Inventario-.NET-
Aplicacion en .NET abierta desde CMD que permite el manejo de un inventario de productos, asi como la creacion de ordenes de compra.

## Practica
Este trabajo consiste en el manejo de datos serializables en formato XML, JSON y (El ya defectuoso) binario.
## Como funciona?
Esta app consta de un menu de opciones para el manejo de inventario, tanto para agregar, modificar y eliminar los productos, asi como visualizar no solo todos los productos, sino que aquellos que cuentan con un bajo stock de mercancias
Esto se obtiene al comparar su stock actual con el stock minimo establecido por el usuario.
-(Si el usuario lo desea, se puede generar una orden con las mercancias actuales que cuentan con bajo stock para que lleguen a su stock maximo establecido.)
Esto genera una orden temporal, a la cual se puede acceder en el menu de Ordenes, donde se puede confirmar, marcandola como "en Proceso", asi como modificar (agregando, eliminando o cambiando la cantidad de un producto).

### lista de ordenes
Cada orden de un producto tiene un identificador propio que es almacenado dentro de la orden, permitiendo al sistema agrupar cada producto pedido junto al resto de su misma orden, separandolos de otras.

### Opciones
En el menu de opciones podemos cambiar el formato actual utilizado por el sistema para almacenar y leer los archivos de datos.
Tras cambiar de metodo, se puede leer otro tipo de inventario sin modificar el anterior, permitiendo asi leer 2 datos diferentes.

### Debug
Si  desde el menu principal en lugar de escribir una opcion numerica se escribe debug, se entra en un menu diferente que permite mostrar datos extra para depuracion y errores, asi como cargar una platinna de productos como metodo de lectura de archivo inicial.

### Cambios a Futuro
* Permitir que al ejecutable se le agrege directamente la ubicacion de un archivo, para que lo cargue directamente al sistema
* Un archivo html que permita al usuario ver las listas, agregar archivos u modificarlos directamente desde una unidad mas grafica y user-friendly

### Cambios dinamicos
Pense en utilizar Git para detectar los cambios en el archivo de datos, y asi de manera dinamica descargarlo en otros dispositivos, para que tengan sus datos sincronizados con el principal.
(Es la forma que pense, pues aun no manejo bien el tema de backend y servidores)

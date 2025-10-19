using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Caso1
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public float Precio { get; set; }
        public int Cantidad { get; set; }

        public Producto(string codigo, string nombre, float precio, int cantidad)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentNullException("El código del producto no puede estar vacío, por favor verifique e inténtelo de nuevo", nameof(codigo));
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentNullException("El nombre del producto no puede estar vacío, por favor verifique e inténtelo de nuevo", nameof(codigo));
            }

            if (precio < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precio), "El precio del producto no puede ser negativo, por favor verifique e inténtelo de nuevo");
            }

            if (cantidad < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad del producto no puede ser negativo, por favor verifique e inténtelo de nuevo");
            }

            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
            Cantidad = cantidad;
        }

        public override string ToString()
        {
            return $"Código: {Codigo} | Nombre: {Nombre} | Precio(colones): {Precio} | Cantidad: {Cantidad}";
        }
    }

    public class Inventario
    {
        private List<Producto> productos;

        public Inventario()
        {
            productos = new List<Producto>();
        }

        public void AgregarProducto(Producto producto)
        {
            if (producto == null)
            {
                throw new ArgumentNullException("El producto no puede ser nulo", nameof(producto));
            }
            if (productos.Exists(p => p.Codigo == producto.Codigo)) // Verifica elemento por elemento si ya existe un atributo con el mismo código
            {
                throw new InvalidOperationException($"El producto con código {producto.Codigo} ya existe en el inventario");
            }
            productos.Add(producto);
        }

        public List<Producto> ListarProductos()
        {
            return productos;
        }

        public Producto BuscarProducto(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código a buscar no puede ser vacío, por favor verifique e inténdelo de nuevo", nameof(codigo));
            }

            codigo = codigo.ToLower();

            foreach (var p in productos) // Iterar cada elemento en la lista
            {
                if (p.Codigo.ToLower() == codigo)
                {
                    return p;
                }
            }

            throw new KeyNotFoundException($"No se encontró ningún producto con el código '{codigo}'."); // Excepción si el if no se cumple
        }

        public void MustStock ()
        {
            foreach (var p in productos)
            {
                if (p.Cantidad == 0)
                {
                    Console.WriteLine($"El producto {p.Nombre} tiene cantidad {p.Cantidad}, se necesita añadir al siguiente stock");
                }
            }
        }

    }


    // Clase de menú, va de último porque utiliza todas las demás clases
    public class Menu
    {
        static void Main()
        {
            do
            {
                Inventario inventario = new Inventario();

                Console.WriteLine("Bienvenid@ al Sistema de Stock ElectroPlus");
                Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("\n 1. Agregar producto");
                Console.WriteLine("\n 2. Listar productos");
                Console.WriteLine("\n 3. Buscar producto");
                Console.WriteLine("\n 4. Mostrar productos de nuevo ReStock");
                Console.WriteLine("\n 5. Salir");

                Console.Write("\n Seleccione una opción: ");
                string opcion = Console.ReadLine();
                
                switch(opcion)
                {
                    case "1":
                        try
                        {
                            Console.Write("\n Ingrese el código del producto: ");
                            string codigo = Console.ReadLine();

                            Console.Write("\n Ingrese el nombre del producto: ");
                            string nombre = Console.ReadLine();

                            Console.Write("\n Ingrese el precio del producto: ");
                            float precio = float.Parse(Console.ReadLine());

                            Console.Write("\n Ingrese la cantidad del producto: ");
                            int cantidad = int.Parse(Console.ReadLine());

                            Producto newProd = new Producto(codigo, nombre, precio, cantidad);
                            inventario.AgregarProducto(newProd);

                            Console.WriteLine("\n Producto agregado exitosamente");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n Error: {ex.Message}");
                        }
                        break;

                    case "2":
                        var productos = inventario.ListarProductos();
                        if (productos.Count == 0)
                        {
                            Console.WriteLine("\n No hay productos en el inventario");
                        }
                        else
                        {
                            foreach (var p in productos)
                            {
                                Console.WriteLine(p);
                            }
                        }
                        break;

                    case "3":
                        try
                        {
                            Console.Write("\n Ingrese el código del producto a buscar: ");
                            string codSearch = Console.ReadLine();

                            var match = inventario.BuscarProducto(codSearch);
                            Console.WriteLine(match);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n Error: {ex.Message}");
                        }
                        break;

                    case "4":
                        inventario.MustStock();
                        break;

                    case "5":
                        Console.WriteLine("\n Gracias por utilizar el sistema de Stock ElectroPlus, saliendo...");
                        return;

                    default:
                        Console.WriteLine("\n Opción no válida, por favor verifique e intente de nuevo");
                        break;
                }
            }
            while (true);
        }
    }
}

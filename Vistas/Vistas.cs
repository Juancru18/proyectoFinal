using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlquilerBicicletas.Modelos;  // Hace referencia al namespace "Modelos" para poder usarlo
using AlquilerBicicletas.Servicio;  // Hace referencia al namespace "Servicios" para poder usarlo

namespace AlquilerBicicletas.Vistas
{

    // Capa de Vistas o Presentacion

    public class Menu
    {

        public ServicioAlquiler servicio = new ServicioAlquiler();

        // Helper reutilizable para leer un entero positivo desde consola,
        // validando la entrada hasta que sea correcta.
        private int LeerEnteroPositivo(string mensaje)
        {
            int valor;

            Console.Write(mensaje);

            while (!int.TryParse(Console.ReadLine(), out valor) || valor <= 0)
            {
                Console.Write("Valor inválido. Ingrese un número entero positivo: ");
            }

            return valor;
        }

        // Helper reutilizable para leer un decimal positivo desde consola.
        private decimal LeerDecimalPositivo(string mensaje)
        {
            decimal valor;

            Console.Write(mensaje);

            while (!decimal.TryParse(Console.ReadLine(), out valor) || valor <= 0)
            {
                Console.Write("Valor inválido. Ingrese un número mayor a cero: ");
            }

            return valor;
        }

        // Helper reutilizable para leer un texto no vacío desde consola.
        private string LeerTextoNoVacio(string mensaje)
        {
            string valor;

            Console.Write(mensaje);
            valor = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(valor))
            {
                Console.Write("El valor no puede estar vacío. Intente nuevamente: ");
                valor = Console.ReadLine();
            }

            return valor;
        }

        private void RetirarBicicleta()
        {
            Console.Clear();

            int id = LeerEnteroPositivo("Id de la bicicleta: ");
            string usuario = LeerTextoNoVacio("Nombre del usuario: ");
            decimal precio = LeerDecimalPositivo("Precio por hora: ");

            try
            {
                servicio.RegistrarAlquiler(id, usuario, precio);

                Console.WriteLine("\nBicicleta alquilada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MostrarAlquileres()
        {
            Console.Clear();

            List<AlquilerBicicleta> alquileres;

            try
            {
                alquileres = servicio.ObtenerAlquileres();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            if (alquileres.Count == 0)
            {
                Console.WriteLine("No hay bicicletas alquiladas.");
                return;
            }

            Console.WriteLine("BICICLETAS EN USO\n");

            foreach (AlquilerBicicleta alquiler in alquileres)
            {
                Console.WriteLine(alquiler);
            }
        }

        private void DevolverBicicleta()
        {
            Console.Clear();

            int id = LeerEnteroPositivo("Id de la bicicleta a devolver: ");

            try
            {
                decimal importe = servicio.DevolverBicicleta(id);

                if (importe < 0)
                {
                    Console.WriteLine("No se encontró una bicicleta con ese Id alquilada.");
                    return;
                }

                Console.WriteLine($"Importe a pagar: ${importe:F2}");
                Console.WriteLine("Bicicleta devuelta correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Iniciar()
        {
            int opcion;

            do
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine("    ALQUILER DE BICICLETAS");
                Console.WriteLine("==================================");
                Console.WriteLine("1 - Retirar bicicleta");
                Console.WriteLine("2 - Ver bicicletas alquiladas");
                Console.WriteLine("3 - Devolver bicicleta");
                Console.WriteLine("4 - Salir");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida. Ingrese un número.");
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        RetirarBicicleta();
                        break;

                    case 2:
                        MostrarAlquileres();
                        break;

                    case 3:
                        DevolverBicicleta();
                        break;

                    case 4:
                        Console.WriteLine("Hasta luego...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                if (opcion != 4)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 4);

        }
    }
}

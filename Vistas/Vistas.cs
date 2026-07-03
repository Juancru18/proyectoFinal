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

        private void RetirarBicicleta()
        {
            Console.Clear();

            Console.Write("Id de la bicicleta: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Nombre del usuario: ");
            string usuario = Console.ReadLine();

            Console.Write("Precio por hora: ");
            decimal precio = decimal.Parse(Console.ReadLine());

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

            List<AlquilerBicicleta> alquileres = servicio.ObtenerAlquileres();

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

            Console.Write("Id de la bicicleta a devolver: ");
            int id = int.Parse(Console.ReadLine());

            decimal importe = servicio.DevolverBicicleta(id);

            if (importe < 0)
            {
                Console.WriteLine("No se encontró una bicicleta con ese Id alquilada.");
                return;
            }

            Console.WriteLine($"Importe a pagar: ${importe:F2}");
            Console.WriteLine("Bicicleta devuelta correctamente.");
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

                opcion = int.Parse(Console.ReadLine());

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

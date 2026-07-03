using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlquilerBicicletas.Modelos; // Hace referencia al namespace "Modelos" para poder usarlo

namespace AlquilerBicicletas.Servicio
{

    // Capa de Servicio o Negocios

    public class ServicioAlquiler
    {
        private readonly string rutaArchivo = "bicis.txt";

        public ServicioAlquiler()
        {
            if (!File.Exists(rutaArchivo))
            {
                File.Create(rutaArchivo).Close();
            }
        }

        public void RegistrarAlquiler(int idBici, string usuario, decimal precioPorHora)
        {

            if (BuscarBicicleta(idBici) != null)
            {
                throw new Exception("La bicicleta ya se encuentra alquilada.");
            }

            AlquilerBicicleta alquiler = new AlquilerBicicleta(idBici, usuario, precioPorHora);

            string linea = $"{alquiler.IdBici};" +
                           $"{alquiler.Usuario};" +
                           $"{alquiler.HoraInicio};" +
                           $"{alquiler.PrecioPorHora}";

            File.AppendAllText(rutaArchivo, linea + Environment.NewLine);
        }

        public List<AlquilerBicicleta> ObtenerAlquileres()
        {
            List<AlquilerBicicleta> lista = new List<AlquilerBicicleta>();

            string[] lineas = File.ReadAllLines(rutaArchivo);

            foreach (string linea in lineas)
            {

                if (string.IsNullOrWhiteSpace(linea))
                {
                    continue;
                }

                string[] datos = linea.Split(';');

                AlquilerBicicleta alquiler = new AlquilerBicicleta(int.Parse(datos[0]), datos[1], decimal.Parse(datos[3]));

                alquiler.HoraInicio = DateTime.Parse(datos[2]);

                lista.Add(alquiler);
            }

            return lista;
        }

        public AlquilerBicicleta BuscarBicicleta(int idBici)
        {
            List<AlquilerBicicleta> alquileres = ObtenerAlquileres();

            foreach (AlquilerBicicleta alquiler in alquileres)
            {
                if (alquiler.IdBici == idBici)
                {
                    return alquiler;
                }
            }

            return null;
        }

        public decimal CalcularImporte(AlquilerBicicleta alquiler)
        {
            double horas = Math.Max(1, Math.Ceiling((DateTime.Now - alquiler.HoraInicio).TotalHours));

            decimal importe = (decimal)horas * alquiler.PrecioPorHora;

            return importe;
        }

        private void GuardarLista(List<AlquilerBicicleta> alquileres)
        {
            List<string> lineas = new List<string>();

            foreach (AlquilerBicicleta alquiler in alquileres)
            {
                string linea = $"{alquiler.IdBici};" +
                               $"{alquiler.Usuario};" +
                               $"{alquiler.HoraInicio};" +
                               $"{alquiler.PrecioPorHora}";

                lineas.Add(linea);
            }

            File.WriteAllLines(rutaArchivo, lineas);
        }

        public decimal DevolverBicicleta(int idBici)
        {
            // Obtener todos los alquileres
            List<AlquilerBicicleta> alquileres = ObtenerAlquileres();

            // Buscar la bicicleta
            AlquilerBicicleta alquiler = BuscarBicicleta(idBici);

            // Si no existe
            if (alquiler == null)
            {
                return -1;
            }

            // Calcular el importe
            decimal importe = CalcularImporte(alquiler);

            // Eliminar el alquiler de la lista
            alquileres.RemoveAll(a => a.IdBici == idBici);

            // Actualizar el archivo
            GuardarLista(alquileres);

            // Devolver el importe para imprimir el ticket
            return importe;
        }
    }
}

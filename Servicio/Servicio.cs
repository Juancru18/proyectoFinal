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
                throw new InvalidOperationException("La bicicleta ya se encuentra alquilada.");
            }

            AlquilerBicicleta alquiler = new AlquilerBicicleta(idBici, usuario, precioPorHora);

            string linea = $"{alquiler.IdBici};" +
                           $"{alquiler.Usuario};" +
                           $"{alquiler.HoraInicio};" +
                           $"{alquiler.PrecioPorHora}";

            try
            {
                File.AppendAllText(rutaArchivo, linea + Environment.NewLine);
            }
            catch (IOException ex)
            {
                throw new Exception("No se pudo guardar el alquiler. Verifique que el archivo no esté en uso.", ex);
            }
        }

        public List<AlquilerBicicleta> ObtenerAlquileres()
        {
            List<AlquilerBicicleta> lista = new List<AlquilerBicicleta>();

            string[] lineas;

            try
            {
                lineas = File.ReadAllLines(rutaArchivo);
            }
            catch (IOException ex)
            {
                throw new Exception("No se pudo leer el archivo de alquileres. Verifique que no esté en uso.", ex);
            }

            foreach (string linea in lineas)
            {

                if (string.IsNullOrWhiteSpace(linea))
                {
                    continue;
                }

                string[] datos = linea.Split(';');

                if (datos.Length != 4)
                {
                    continue;
                }

                if (!int.TryParse(datos[0], out int idBici) || !decimal.TryParse(datos[3], out decimal precioPorHora) || !DateTime.TryParse(datos[2], out DateTime horaInicio))
                {
                    continue;
                }

                string usuario = datos[1];

                if (idBici <= 0 || string.IsNullOrWhiteSpace(usuario) || precioPorHora <= 0)
                {
                    continue;
                }

                AlquilerBicicleta alquiler = new AlquilerBicicleta(idBici, usuario, precioPorHora)
                {
                    HoraInicio = horaInicio
                };

                lista.Add(alquiler);
            }

            return lista;
        }

        public AlquilerBicicleta BuscarBicicleta(int idBici)
        {
            if (idBici <= 0)
            {
                return null;
            }

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

            try
            {
                File.WriteAllLines(rutaArchivo, lineas);
            }
            catch (IOException ex)
            {
                throw new Exception("No se pudo actualizar el archivo de alquileres. Verifique que no esté en uso.", ex);
            }
        }

        public decimal DevolverBicicleta(int idBici)
        {
            if (idBici <= 0)
            {
                return -1;
            }

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

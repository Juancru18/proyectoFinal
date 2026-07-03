using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlquilerBicicletas.Modelos
{

    // Capa de Modelos

    public class AlquilerBicicleta
    {
        public int IdBici { get;  set; }
        public string Usuario { get; set; }
        public DateTime HoraInicio { get; set; }
        public decimal PrecioPorHora { get; set; }

        public AlquilerBicicleta(int idBici, string usuario, decimal precioPorHora)
        {
            if (idBici <= 0)
            {
                throw new ArgumentException("El id de la bicicleta debe ser un número positivo.", nameof(idBici));
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                throw new ArgumentException("El usuario no puede estar vacío.", nameof(usuario));
            }

            if (precioPorHora <= 0)
            {
                throw new ArgumentException("El precio por hora debe ser mayor a cero.", nameof(precioPorHora));
            }

            IdBici = idBici;
            Usuario = usuario.Trim();
            HoraInicio = DateTime.Now;
            PrecioPorHora = precioPorHora;
        }
        public override string ToString()
        {
            return $"Bici: {IdBici} | Usuario: {Usuario} | Inicio: {HoraInicio} | Precio/Hora: ${PrecioPorHora} ";
        }
    }
}

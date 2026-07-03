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
            IdBici = idBici;
            Usuario = usuario;
            HoraInicio = DateTime.Now;
            PrecioPorHora = precioPorHora;
        }
        public override string ToString()
        {
            return $"Bici: {IdBici} | Usuario: {Usuario} | Inicio: {HoraInicio} | Precio/Hora: ${PrecioPorHora} ";
        }
    }
}

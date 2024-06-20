using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Sistema_de_horarios_U4_S4.Models
{
    public enum Dias { Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo }

    [Table("Clases")]
    public class HorarioModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull,MaxLength(100)]
        public string NombreAC { get; set; } = null!; //Activiad o clase

        [MaxLength(100)]
        public string NombreMaestro { get; set; } = "";

        [MaxLength(50)]
        public string AulaDescripcion { get; set; } = "";

        [NotNull]
        public Dias Dia { get; set; }
        [NotNull]
        public int HoraInicio { get; set; }
        [NotNull]
        public int HoraFin { get; set; }
        [NotNull]
        public string AC { get; set; } = ""; //Actividad o clase
    }
}

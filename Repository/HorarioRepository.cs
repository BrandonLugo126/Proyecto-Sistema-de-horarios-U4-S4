using Proyecto_Sistema_de_horarios_U4_S4.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Sistema_de_horarios_U4_S4.Repository
{
    public class HorarioRepository
    {
        SQLiteConnection conexion;
        public HorarioRepository()
        {
            var ruta = FileSystem.AppDataDirectory + "/SistemaHorarios.sqlite";
            conexion = new SQLiteConnection(ruta);
            conexion.CreateTable<HorarioModel>();
        }

        public IEnumerable<HorarioModel> GetAll()
        {
            return conexion.Table<HorarioModel>().OrderBy(x => x.HoraInicio);
        }

        public bool Validar(int Inicio, int Fin, Dias dia)
        {
            return conexion.Table<HorarioModel>().Any(x => x.HoraInicio == Inicio && x.HoraFin == Fin && x.Dia == dia);
        }

        public void Insert(HorarioModel horario)
        {
            conexion.Insert(horario);
        }

        public void Update(HorarioModel horario)
        {
            conexion.Update(horario);
        }

        public void Delete(HorarioModel horario)
        {
            conexion.Delete(horario);
        }
    }
}

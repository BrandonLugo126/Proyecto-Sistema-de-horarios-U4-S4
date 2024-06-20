using Microsoft.Win32;
using Proyecto_Sistema_de_horarios_U4_S4.Models;
using Proyecto_Sistema_de_horarios_U4_S4.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Proyecto_Sistema_de_horarios_U4_S4.ViewModels
{
    public class HorarioViewModel:INotifyPropertyChanged
    {
       HorarioRepository repository = new();
        public ObservableCollection<HorarioModel> ListaDeDatos { get; set; } = new();

        public ObservableCollection<HorarioModel> ListaObservable { get; set; } = new();

        public IEnumerable<Dias> ListaDias => Enum.GetValues(typeof(Dias)).Cast<Dias>();

        Dias DiaSelecccionado;
        public Dias diaSeleccionado { get => DiaSelecccionado; set { DiaSelecccionado = value; ActualizarFiltro(); } }
        public string MensajeError { get; set; } = "";
        public HorarioModel? Horario { get; set; } = new();

        private int indiceHorarioEditar;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand VerEditarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand EditarActividadCommand { get; set; }

        public ICommand EditarClaseCommand { get; set; }
        public ICommand NavegateCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand AgregarClaseCommand { get; set; }
        public ICommand AgregarActividadCommand { get; set; }


        public HorarioViewModel()
        {
            VerEliminarCommand = new Command<HorarioModel>(VerEliminar);
            EditarActividadCommand = new Command(EditarActividad);
            VerEditarCommand = new Command<HorarioModel>(VerEditar);
            AgregarActividadCommand = new Command(AgregarActividad);
            EditarClaseCommand = new Command(EditarClase);
            EliminarCommand = new Command(Eliminar);
            NavegateCommand = new Command<string>(Navegar);

            AgregarClaseCommand = new Command(AgregarClase);
            foreach (var X in repository.GetAll())
            {
                ListaDeDatos.Add(X);
            }

        }

        private void VerEliminar(HorarioModel H)
        {
            if (H != null)
            {
                Horario = H;
                if (H.AC== "Descripcion")
                {
                    Navegar("EliminarActividad");
                }
                if (H.AC == "Aula")
                {
                    Navegar("EliminarClase");
                }
            }
        }

        private void EditarActividad()
        {
            MensajeError = "";
            if (Horario != null)
            {
                Horario.AC = "Descripcion"; //Actividad

                if (string.IsNullOrWhiteSpace(Horario.NombreAC))
                {
                    MensajeError = "No se puede dejar vacia el nombre de la actividad";
                }
                if (string.IsNullOrWhiteSpace(Horario.AulaDescripcion)) //Descripcion
                {
                    MensajeError = "No se puede dejar vacia el aula";
                }
                if (repository.Validar(Horario.HoraInicio, Horario.HoraFin, Horario.Dia))
                {
                    MensajeError = "No puedes agregar Clases o actividades a la misma hora y en el mismo dia";
                }
                if (Horario.HoraInicio <= 0 || Horario.HoraInicio >= 24 || Horario.HoraFin <= 0 || Horario.HoraFin >= 23)
                {
                    MensajeError = "Solo se pueden agregar clases y actividades en un formato de 24 horas";
                }
                if (Horario.Dia == Dias.Domingo && Horario.AC == "C")
                {
                    MensajeError = "No hay clases los domingos";
                }
                if (Horario.HoraInicio == Horario.HoraFin)
                {
                    MensajeError = "No se puede poner la misma hora de inicio que la hora final";
                }
                if (Horario.HoraInicio > Horario.HoraFin)
                {
                    MensajeError = "No puedes terminar antes de empezar";
                }
                /*if (ListaDeDatos.Any(x => x.HoraInicio < Horario.HoraFin && x.HoraInicio < Horario.HoraFin && x.Dia == Horario.Dia) == true)
                {
                    MensajeError = "No puedes agregar actividades en un horario donde ya existen clases o actividades";
                }*/
                if (MensajeError == "" )
                {
                    ListaDeDatos[indiceHorarioEditar] = Horario;
                    ListaObservable[indiceHorarioEditar] = Horario;
                    repository.Update(Horario);
                    ActualizarFiltro();
                    PropertyChanged?.Invoke(this, new(nameof(ListaObservable)));
                    Navegar("Inicio");
                }
                PropertyChanged?.Invoke(this, new(nameof(MensajeError)));

            }
        }

        private void VerEditar(HorarioModel h)
        {
            var clon = new HorarioModel()
            {
                Id = h.Id,
                NombreAC = h.NombreAC,
                AulaDescripcion = h.AulaDescripcion,
                Dia = h.Dia,
                NombreMaestro = h.NombreMaestro,
                AC = h.AC,
                HoraInicio= h.HoraInicio,
                HoraFin=h.HoraFin
            };
            Horario = clon;
            indiceHorarioEditar = ListaDeDatos.IndexOf(h);
            MensajeError = "";
            if (h.AC=="Descripcion")
            {
                Horario = h;
                Navegar("EditarActividad");                
            }
            if (h.AC == "Aula")
            {
                Horario = h;
                Navegar("EditarClase");
            }
        }

        private void AgregarActividad()
        {
            MensajeError = "";
            if (Horario != null)
            {
                Horario.AC = "Descripcion"; //Actividad
                if (string.IsNullOrWhiteSpace(Horario.NombreAC))
                {
                    MensajeError = "No se puede dejar vacia el nombre de la actividad";
                }
                if (string.IsNullOrWhiteSpace(Horario.AulaDescripcion)) //Descripcion
                {
                    MensajeError = "No se puede dejar vacia el aula";
                }
                if (repository.Validar(Horario.HoraInicio, Horario.HoraFin, Horario.Dia))
                {
                    MensajeError = "No puedes agregar Clases o actividades a la misma hora y en el mismo dia";
                }
                if (Horario.HoraInicio <= 0 || Horario.HoraInicio > 24 || Horario.HoraFin <= 0 || Horario.HoraFin > 23)
                {
                    MensajeError = "Solo se pueden agregar clases y actividades en un formato de 24 horas";
                }
                if (Horario.Dia == Dias.Domingo && Horario.AC == "C")
                {
                    MensajeError = "No hay clases los domingos";
                }
                if (Horario.HoraInicio == Horario.HoraFin)
                {
                    MensajeError = "No se puede poner la misma hora de inicio que la hora final";
                }
                if (Horario.HoraInicio > Horario.HoraFin)
                {
                    MensajeError = "No puedes terminar antes de empezar";
                }
                /*if (ListaDeDatos.Any(x => x.HoraInicio < Horario.HoraFin && x.HoraInicio < Horario.HoraFin && x.Dia == Horario.Dia) == true)
                {
                    MensajeError = "No puedes agregar actividades en un horario donde ya existen clases o actividades";
                }*/
                if (MensajeError == "")
                {
                    ListaDeDatos.Insert(0,Horario);
                    repository.Insert(Horario);
                    ActualizarFiltro();
                    Navegar("Inicio");
                }
                PropertyChanged?.Invoke(this, new(nameof(MensajeError)));
            }
        }

        private void EditarClase()
        {
            MensajeError = "";

            if (Horario != null)
            {
                Horario.AC = "Aula"; //clase

                if (string.IsNullOrWhiteSpace(Horario.NombreAC))
                {
                    MensajeError = "No se puede dejar vacio el nombre de la Clase";
                }
                if (string.IsNullOrWhiteSpace(Horario.NombreMaestro))
                {
                    MensajeError = "No se puede dejar vacio el nombre del maestro o maestra";
                }
                if (string.IsNullOrWhiteSpace(Horario.AulaDescripcion)) //aula
                {
                    MensajeError = "No se puede dejar vacia el aula";
                }
                if (repository.Validar(Horario.HoraInicio, Horario.HoraFin, Horario.Dia))
                {
                    MensajeError = "No puedes agregar Clases o actividades a la misma hora y en el mismo dia";
                }
                if (Horario.HoraInicio <= 0 || Horario.HoraInicio > 24 || Horario.HoraFin <= 0 || Horario.HoraFin > 23)
                {
                    MensajeError = "Solo se pueden agregar clases y actividades en un formato de 24 horas";
                }
                if (Horario.Dia == Dias.Domingo && Horario.AC == "Aula")
                {
                    MensajeError = "No hay clases los domingos";
                }
                if (Horario.HoraInicio == Horario.HoraFin)
                {
                    MensajeError = "No se puede poner la misma hora de inicio que la hora final";
                }
                if (Horario.HoraInicio > Horario.HoraFin)
                {
                    MensajeError = "No puedes terminar antes de empezar";
                }
                /*if (ListaDeDatos.Any(x => x.HoraInicio < Horario.HoraFin && x.HoraInicio < Horario.HoraFin && x.Dia == Horario.Dia) == true)
                {
                    MensajeError = "No puedes agregar actividades en un horario donde ya existen clases o actividades";
                }*/
                if (MensajeError == "")
                {
                    ListaDeDatos[indiceHorarioEditar] = Horario;
                    ListaObservable[indiceHorarioEditar] = Horario;
                    repository.Update(Horario);
                    ActualizarFiltro();
                    Navegar("Inicio");
                    PropertyChanged?.Invoke(this, new(nameof(ListaDeDatos)));
                    PropertyChanged?.Invoke(this, new(nameof(Horario)));
                }
                PropertyChanged?.Invoke(this, new(nameof(MensajeError)));
            }
        }

        private void Eliminar()
        {
            if (Horario != null)
            {
                repository.Delete(Horario);
                ListaDeDatos.Remove(Horario);
                ListaObservable.Remove(Horario);
                PropertyChanged?.Invoke(this, new(nameof(ListaObservable)));
                PropertyChanged?.Invoke(this, new(nameof(ListaDeDatos)));
                Navegar("Inicio");

            }
        }


        private void Navegar(string ruta)
        {
            if (ruta == "AgregarActividad" || ruta == "AgregarClase")
            {
                Horario = new();
            }
            if (ruta == "Inicio")
            {
                Horario = null;
            }

            Shell.Current.GoToAsync("//" + ruta);
            
            MensajeError = "";
            PropertyChanged?.Invoke(this, new(nameof(MensajeError)));
            PropertyChanged?.Invoke(this, new(nameof(Horario)));
        }
        private void ActualizarFiltro()
        {
            ListaObservable.Clear();
            var info = ListaDeDatos.Where(x => x.Dia == DiaSelecccionado).OrderBy(x => x.HoraInicio);
            foreach (var item in info)
            {
                ListaObservable.Add(item);
            }
        }

        private void AgregarClase()
        {
            MensajeError = "";
            if (Horario != null)
            {
                Horario.AC = "Aula"; //clase
                if (string.IsNullOrWhiteSpace(Horario.NombreAC))
                {
                    MensajeError = "No se puede dejar vacio el nombre de la Clase";
                }
                if (string.IsNullOrWhiteSpace(Horario.NombreMaestro))
                {
                    MensajeError = "No se puede dejar vacio el nombre del maestro o maestra";
                }
                if (string.IsNullOrWhiteSpace(Horario.AulaDescripcion)) //aula
                {
                    MensajeError = "No se puede dejar vacia el aula";
                }
                if (repository.Validar(Horario.HoraInicio, Horario.HoraFin, Horario.Dia))
                {
                    MensajeError = "No puedes agregar Clases o actividades a la misma hora y en el mismo dia";
                }
                if (Horario.HoraInicio<=0 || Horario.HoraInicio>24 || Horario.HoraFin<=0 || Horario.HoraFin>23)
                {
                    MensajeError = "Solo se pueden agregar clases y actividades en un formato de 24 horas";
                }
                if (Horario.Dia== Dias.Domingo && Horario.AC=="Aula")
                {
                    MensajeError = "No hay clases los domingos";
                }
                if (Horario.HoraInicio==Horario.HoraFin)
                {
                    MensajeError = "No se puede poner la misma hora de inicio que la hora final";
                }
                if (Horario.HoraInicio>Horario.HoraFin)
                {
                    MensajeError = "No puedes terminar antes de empezar";
                }
                /*if (ListaDeDatos.Any(x=>x.HoraInicio < Horario.HoraFin && x.HoraInicio < Horario.HoraFin && x.Dia == Horario.Dia) == true)
                {
                    MensajeError = "No puedes agregar clases en un horario donde ya halla clases o actividades";
                }*/
                if (MensajeError =="")
                {                   
                    ListaDeDatos.Insert(0, Horario);
                    repository.Insert(Horario);
                    ListaObservable.Insert(0,Horario);
                    PropertyChanged?.Invoke(this, new(nameof(MensajeError)));
                    PropertyChanged?.Invoke(this, new(nameof(Horario)));
                    PropertyChanged?.Invoke(this, new(nameof(ListaObservable)));
                    ActualizarFiltro();
                    Navegar("Inicio");
                }
                PropertyChanged?.Invoke(this, new(nameof(MensajeError)));
            }
        }
    }
}

using SGH.DAOs;
using SGH.Modelos;
using SGH.Vistas.Calificaciones;
using SGH.Vistas.MenuPrincipal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SGH.Calificaciones
{
    /// <summary>
    /// Interaction logic for CalificacionesEstudiante.xaml
    /// </summary>
    public partial class CalificacionesEstudiante : Window
    {
        private Estudiante estudiante;
        private Grupo grupo;
        private Materia materia;
        private Calificacion calificacion;
        private GrupoDAO grupoDAO = new GrupoDAO();
        private MateriaDAO materiaDAO = new MateriaDAO();
        private CalificacionDAO calificacionDAO = new CalificacionDAO();

        public CalificacionesEstudiante(Estudiante estudiante)
        {
            InitializeComponent();
            definirEstudiante(estudiante);
            definirSemestre();
            definirMaterias();
        }

        private void definirEstudiante(Estudiante estudiante)
        {
            this.estudiante = estudiante;
            lbNombreEstudiante.Content = $"{estudiante.Persona.Nombre} {estudiante.Persona.ApellidoPaterno} {estudiante.Persona.ApellidoMaterno}";
            
        }

        private void definirSemestre()
        {
            Grupo grupoDelEstudiante = grupoDAO.recuperarGrupo(estudiante);
            grupo = grupoDelEstudiante;
            lb_semestre.Content = $"{grupoDelEstudiante.Semestre} {grupoDelEstudiante.Letra}";
        }

        private void definirMaterias()
        {
            List<Materia> materias = grupoDAO.recuperarMateriasDeUnGrupo(grupo);

            List<String> nombresMaterias = (from materia in materias
                                           select materia.Nombre).ToList();
            cb_Materia.IsEnabled = true;
            cb_Materia.ItemsSource = nombresMaterias;
        }

        private void ClickRegresar(object sender, RoutedEventArgs e)
        {
            BuscadorEstudiante buscadorEstudiante = new BuscadorEstudiante();
            Application.Current.MainWindow = buscadorEstudiante;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<CalificacionesEstudiante>())
            {
                ((CalificacionesEstudiante)window).Close();
            }
        }

        private void ClickGuardar(object sender, RoutedEventArgs e)
        {
            Console.Write("Click en guardar.");
        }

        private void DefinirMateria()
        {
            string nombreMateria = cb_Materia.SelectedItem.ToString();
            Materia materia = materiaDAO.recuperarMateria(nombreMateria);
            this.materia = materia;
        }

        private void RecuperarCalificacion()
        {
            Calificacion calificacionBD = calificacionDAO.recuperarCalificacion(materia.NRC, estudiante.Matricula);
            calificacion = calificacionBD;
        }

        private void MostrarError(string error)
        {
            lbNombreEstudiante.Content = "";
            lbNombreEstudiante.Foreground = Brushes.Red;
            lbNombreEstudiante.Content = error;
        }

        private void LimpiarError()
        {
            lbNombreEstudiante.Content = "";
            lbNombreEstudiante.Foreground = Brushes.Black;
            lbNombreEstudiante.Content = $"{estudiante.Persona.Nombre} {estudiante.Persona.ApellidoPaterno} {estudiante.Persona.ApellidoMaterno}";
        }

        private void ActivarEdicion()
        {
            dg_Calificaciones.Columns[0].IsReadOnly = false;
            dg_Calificaciones.Columns[1].IsReadOnly = false;
            dg_Calificaciones.Columns[3].IsReadOnly = false;
        }

        private void CargarCalificaciones()
        {
            List<Calificacion_Parcial> parciales = calificacion.Calificacion_Parcial.ToList();
            CalificacionOrdenada calificacionOrdenada = new CalificacionOrdenada();
            calificacionOrdenada.primerParcial = (float)parciales[0].CaliParcial;
            calificacionOrdenada.segundoParcial = (float)parciales[1].CaliParcial;
            calificacionOrdenada.promedioParcial = (calificacionOrdenada.primerParcial + calificacionOrdenada.segundoParcial)/2;
            calificacionOrdenada.evaluacionFinal = (float)calificacion.EvalFinal;
            calificacionOrdenada.ponderado = ((calificacionOrdenada.primerParcial + calificacionOrdenada.segundoParcial + calificacionOrdenada.evaluacionFinal) / 3);

            List<CalificacionOrdenada> calificacionesOrdenadas = new List<CalificacionOrdenada>();
            calificacionesOrdenadas.Add(calificacionOrdenada);

            dg_Calificaciones.ItemsSource = calificacionesOrdenadas;
        }

        private void ActualizarInformacionMateria(object sender, SelectionChangedEventArgs e)
        {
            LimpiarError();
            DefinirMateria();
            RecuperarCalificacion();

            if (calificacion != null)
            {
                //ActivarEdicion();
                CargarCalificaciones();
            }
            else
            {
                MostrarError("No hay registros para la materia seleccionada");
                dg_Calificaciones.ItemsSource=null;
            }
        }
    }
}

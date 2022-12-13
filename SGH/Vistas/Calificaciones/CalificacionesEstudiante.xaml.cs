using SGH.DAOs;
using SGH.Modelos;
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
        private GrupoDAO grupoDAO = new GrupoDAO();
        private MateriaDAO materiaDAO = new MateriaDAO();

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

        private void actualizarInformacionMateria(object sender, SelectionChangedEventArgs e)
        {
            string nombreMateria = cb_Materia.Text;
            Materia materia = materiaDAO.recuperarMateria(nombreMateria);

            
        }
    }
}

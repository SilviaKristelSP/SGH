using SGH.DAOs;
using SGH.Modelos;
using SGH.Validaciones;
using SGH.Vistas.MenuPrincipal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SGH.Calificaciones
{

    public partial class BuscadorEstudiante : Window
    {
        private EstudianteDAO estudianteDAO = new EstudianteDAO();
        private Verificacion verificacion = new Verificacion();
        
        int ERROR_CURPINVALIDO = -1;
        int ERROR_ESTUDIANTENOEXISTENTE = -2;
        int ERROR_DATOSINCOMPLETOS = -3;


        public BuscadorEstudiante()
        {
            InitializeComponent();
        }


        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipalSGH menuPrincipalSGH = new MenuPrincipalSGH();
            Application.Current.MainWindow = menuPrincipalSGH;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<BuscadorEstudiante>())
            {
                ((BuscadorEstudiante)window).Close();
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            limpiarError();

            Estudiante estudiante = GenerarEstudiante();
            int estatusDeVerificacion = verificacion.VerificarDatos(estudiante);


            if (estatusDeVerificacion == 0)
            {
                Estudiante estudianteBD = estudianteDAO.recuperarEstudiante(estudiante);

                
                CalificacionesEstudiante calificacionesEstudiante = new CalificacionesEstudiante(estudianteBD);
                Application.Current.MainWindow = calificacionesEstudiante;
                Application.Current.MainWindow.Show();

                foreach (Window window in Application.Current.Windows.OfType<BuscadorEstudiante>())
                {
                    ((BuscadorEstudiante)window).Close();
                }
                
            }
            if(estatusDeVerificacion == ERROR_CURPINVALIDO)
            {
                mostrarError("CURP invalido");
            }
            if(estatusDeVerificacion == ERROR_ESTUDIANTENOEXISTENTE)
            {
                mostrarError("El estudiante no existe en los registros");
            }

            if(estatusDeVerificacion == ERROR_DATOSINCOMPLETOS)
            {
                Console.Error.WriteLine("Datos incompletos.");
            }

        }

        private Estudiante GenerarEstudiante()
        {
            if (!HayCamposVacios())
            {
                Estudiante estudiante = new Estudiante();
                estudiante.Persona = new Persona();
                estudiante.Persona.Curp = tb_curp.Text;
                return estudiante;
            }

            return null;
        }

        private void mostrarError(string error)
        {
            label_error.Content = error;
        }

        private bool HayCamposVacios()
        {
            limpiarError();

            if(tb_curp.Text == "") 
            {
                mostrarError("Hay campos vacios, rellenelos.");
                return true;
            }

            return false;
        }

        private void limpiarError()
        {
            label_error.Content = "";
        }
    }
}

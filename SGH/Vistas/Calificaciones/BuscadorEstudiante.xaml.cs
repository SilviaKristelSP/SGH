using SGH.Modelos;
using SGH.Vistas.MenuPrincipal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SGH.Calificaciones
{

    public partial class BuscadorEstudiante : Window
    {
        int ERROR_NOMBREINVALIDO = -1;
        int ERROR_ESTUDIANTENOEXISTENTE = -2;
        int ERROR_DATOSINCOMPLETOS = -3;

        public BuscadorEstudiante()
        {
            InitializeComponent();
            recuperarGrupos();
        }

        private void recuperarGrupos()
        {
            ObservableCollection<string> grupos = new ObservableCollection<string>
            {
                "Grupo 1",
                "Grupo 2",
                "Grupo 3"
            };
            cb_grupo.ItemsSource = grupos;
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

            int estatusDeVerificacion = Validaciones.Verificacion.VerificarDatos(GenerarEstudiante());

            if (estatusDeVerificacion == 0)
            {
                CalificacionesEstudiante ventana = new CalificacionesEstudiante();
                this.Close();
                ventana.Show();
            }
            if(estatusDeVerificacion == ERROR_NOMBREINVALIDO)
            {
                mostrarError("Nombre invalido");
            }
            if(estatusDeVerificacion == ERROR_ESTUDIANTENOEXISTENTE)
            {
                mostrarError("El Estudiante no existe en los registros");
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
                estudiante.Persona.Nombre = tb_nombre.Text;
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

            if(tb_nombre.Text == "") 
            {
                mostrarError("Hay campos vacios, rellenelos.");
                return true;
            }
            if (cb_grupo.SelectedIndex == -1) 
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

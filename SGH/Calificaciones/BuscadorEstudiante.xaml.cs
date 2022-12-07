using SGH.POCO;
using System;
using System.ComponentModel;
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
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            SelectorCalificaciones ventana = new SelectorCalificaciones();
            this.Close();
            ventana.Show();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            limpiarError();

            int estatusDeVerificacion = SGH.Util.Verificacion.VerificarDatos(GenerarEstudiante());

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
                estudiante.nombre = tb_nombre.Text;
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

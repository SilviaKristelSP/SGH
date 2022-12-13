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
using SGH.Vistas.MenuPrincipal;
using SGH.Vistas.LogIn;
using SGH.Utiles;
using SGH.Modelos;
using SGH.DAOs;


namespace SGH.Vistas.Estudiantes
{
    /// <summary>
    /// Lógica de interacción para ConsultarEstudiante.xaml
    /// </summary>
    public partial class ConsultarEstudiante : Window
    {

        private static Administrador administradorMenu = new Administrador();
        private Persona persona = new Persona();
        private Estudiante estudiante = new Estudiante();
        private String id = "";
        private String matricula = "";


        public ConsultarEstudiante(string idPersona)
        {
            InitializeComponent();
            this.id = idPersona;
            this.matricula = matricula;

            recuperarPersonaYEstudiante();
            cargarDatosEstudiante();


            administradorMenu.Rol = "secretaria";
            administradorMenu.NombreCompleto = "usuario prueba";

            FiltrarMenus(administradorMenu.Rol);
            SetInformacionAdministrador(administradorMenu);
        }

        private void recuperarPersonaYEstudiante()
        {
            persona = PersonaDAO.recuperarPersonaID(id);
            estudiante = EstudianteDAO.recuperarEstudianteID(id);
        }

        private void cargarDatosEstudiante()
        {
            lbNombre.Content = persona.Nombre;
            lbApellidoM.Content = persona.ApellidoMaterno;
            lbApellidoP.Content = persona.ApellidoPaterno;
            lbSeguridadSocial.Content = estudiante.NumSeguroSocial;
            lbCURP.Content = persona.Curp;
            lbTipoSangre.Content = persona.TipoSangre;
            inicializarNombreArchivos();
            Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, tbNombreFoto.Text));
            imgFoto.Source = new BitmapImage(uri);
          
        }

        private void inicializarNombreArchivos()
        {
            string nombreCompleto = Util.obtenerNombreSinEspacios(persona);

            tbNombreCURP.Text = "CURP_" + nombreCompleto + ".pdf";
            tbNombreActa.Text = "ActaNacimiento_" + nombreCompleto + ".pdf";
            tbNombreCURPTutor.Text = "CURPTutor_" + nombreCompleto + ".pdf";
            tbNombreCertificadoSecundaria.Text = "CertificadoSecundaria_" + nombreCompleto + ".pdf";
            tbNombreBuenaConducta.Text = "CartaBuenaConducta_" + nombreCompleto + ".pdf";

        }


        //Funcionalidades Archivos

        private void clickAbrirArchivoCURP(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreCURP.Text.Equals(""))
            {
                Util.abrirArchivoPDF(persona.DocCurp, tbNombreCURP.Text);
            }
        }
        

        private void clickAbrirArchivoActaNacimiento(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreActa.Text.Equals(""))
            {
                Util.abrirArchivoPDF(persona.ActaNacimiento, tbNombreActa.Text);
            }
        }

        private void clickAbrirArchivoCURPTutor(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreCURPTutor.Text.Equals(""))
            {
                Util.abrirArchivoPDF(estudiante.DocCurpTutor, tbNombreCURPTutor.Text);
            }
        }

        private void clickAbrirArchivoCertificadoSecundaria(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreCertificadoSecundaria.Text.Equals(""))
            {
                Util.abrirArchivoPDF(estudiante.DocCertificadoSecundaria, tbNombreCertificadoSecundaria.Text);
            }
        }

        private void clickAbrirArchivoBuenaConducta(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreBuenaConducta.Text.Equals(""))
            {
                Util.abrirArchivoPDF(estudiante.DocCartaBuenaConducta, tbNombreBuenaConducta.Text);
            }
        }

        //Funcionalidad botones
        private void clickEditar(object sender, RoutedEventArgs e)
        {
                EditarEstudiante editarEstudiante = new EditarEstudiante(id);
                editarEstudiante.Show();
                this.Close();
        }

        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            Estudiantes estudiantes = new Estudiantes();
            estudiantes.Show();
            this.Close();
        }

        //Funcionalidad MENÚ
        public void SetInformacionAdministrador(Administrador administrador)
        {

            toggleAdministrador.Content = administrador.NombreCompleto.ToUpper().First();
            textBlockAdministrador.Text = administrador.NombreCompleto;

        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            LogInSGH logInSGH = new LogInSGH();
            Application.Current.MainWindow = logInSGH;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<MenuPrincipalSGH>())
            {
                ((MenuPrincipalSGH)window).Close();
            }

        }

        public void FiltrarMenus(string rol)
        {
            if (rol == "secretario")
            {

                menuCalificaciones.Visibility = Visibility.Visible;
                menuHorario.Visibility = Visibility.Visible;
                menuEstudiantes.Visibility = Visibility.Visible;
                asignacionMateriasButton.Visibility = Visibility.Visible;
                generarHorarioButton.Visibility = Visibility.Visible;

                menuGrupos.Visibility = Visibility.Collapsed;
                menuProfesores.Visibility = Visibility.Collapsed;


            }
            else
            {
                menuEstudiantes.Visibility = Visibility.Visible;
                menuGrupos.Visibility = Visibility.Visible;
                menuProfesores.Visibility = Visibility.Visible;
                menuHorario.Visibility = Visibility.Visible;

                menuCalificaciones.Visibility = Visibility.Collapsed;
                asignacionMateriasButton.Visibility = Visibility.Collapsed;
                generarHorarioButton.Visibility = Visibility.Collapsed;


            }
        }
    }
}

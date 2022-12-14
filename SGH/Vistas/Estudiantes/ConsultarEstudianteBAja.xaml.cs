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
using SGH.Vistas.Alertas;
using SGH.Vistas.Horario.Consulta;
using SGH.Vistas.Estudiantes;
using SGH.Calificaciones;
using SGH.Vistas.Horario;


namespace SGH.Vistas.Estudiantes
{
    /// <summary>
    /// Lógica de interacción para ConsultarEstudianteBAja.xaml
    /// </summary>
    public partial class ConsultarEstudianteBAja : Window
    {
        private Persona persona = new Persona();
        private Estudiante estudiante = new Estudiante();
        private Baja baja = new Baja();
        private String id = "";

        public ConsultarEstudianteBAja(String idPersona)
        {
            InitializeComponent();
            this.id = idPersona;

            recuperarPersonaEstudianteYBaja();
            cargarDatosEstudiante();
        }

        private void recuperarPersonaEstudianteYBaja()
        {
            persona = PersonaDAO.recuperarPersonaID(id);
            estudiante = EstudianteDAO.recuperarEstudianteID(id);
            baja = BajaDAO.recuperarBaja(id);
        }

        private void cargarDatosEstudiante()
        {
            lbNombre.Content = persona.Nombre;
            lbApellidoM.Content = persona.ApellidoMaterno;
            lbApellidoP.Content = persona.ApellidoPaterno;
            lbNo_SeguridadSocial.Content = estudiante.NumSeguroSocial;
            lbCURP.Content = persona.Curp;
            lbTipoSangre.Content = persona.TipoSangre;
            tbDescripcion.Text = baja.Descripcion;
            lbMotivo.Content = baja.Motivo;
            inicializarNombreArchivos();
            Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, tbNombreFoto.Text + Util.generarID(30)));
            imgFoto.Source = new BitmapImage(uri);
        }

        private void inicializarNombreArchivos()
        {
            string nombreCompleto = Util.obtenerNombreSinEspacios(persona);

            tbNombreCURP.Text = "CURP_" + nombreCompleto + ".pdf";
            tbNombreActa.Text = "ActaNacimiento_" + nombreCompleto + ".pdf";
            tbNombreCURPTutor.Text = "CURPTutor_" + nombreCompleto + ".pdf";
            tbNombreCertificadoSecundaria.Text = "CertificadoSecundaria_" + nombreCompleto + ".pdf";
            tbNombreCartaConducta.Text = "CartaBuenaConducta_" + nombreCompleto + ".pdf";
            txbDocProbatorio.Text = "Baja_" + nombreCompleto + ".pdf";

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
            if (!tbNombreCartaConducta.Text.Equals(""))
            {
                Util.abrirArchivoPDF(estudiante.DocCartaBuenaConducta, tbNombreCartaConducta.Text);
            }
        }

        private void clickAbrirArchivoDocProb(object sender, MouseButtonEventArgs e)
        {
            if (!txbDocProbatorio.Text.Equals(""))
            {
                Util.abrirArchivoPDF(baja.DocumentoProbatorio, txbDocProbatorio.Text);
            }
        }

        //Funcionalidad botones
        private void ClickCancelarBaja(object sender, RoutedEventArgs e)
        {
            if (mostrarVentanaConfirmacionCancelarBaja())
            {
                if (PersonaDAO.cancelarBajaPersona(persona.ID))
                    mostrarVentanaExito();
                else
                    mostrarVentanaError();
            }
        }
        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            Estudiantes estudiantes = new Estudiantes();
            estudiantes.Show();
            this.Close();
        }

        //Dialogos
        private bool mostrarVentanaConfirmacionCancelarBaja()
        {
            Alerta alerta = new Alerta("¿Está seguro de querer Cancelar la Baja del estudiante?",
                        Alertas.MessageType.Warning,
                        MessageButtons.AcceptCancel, "medium");
            bool seleccion = false;

            MessageBoxCustom confirmation = new MessageBoxCustom(alerta);
            confirmation.ShowDialog();
            if (confirmation.GetDialog())
            {
                seleccion = true;
            }
            else
            {
                confirmation.Close();
            }
            return seleccion;
        }

        private void mostrarVentanaExito()
        {
            Alerta alerta = new Alerta("La Baja fue cancelada exitosamente", Alertas.MessageType.Success,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
            cambiarAVentanaEstudiantes();
        }

        private void mostrarVentanaError()
        {
            Alerta alerta = new Alerta("Error con la base de datos, intente más tarde", Alertas.MessageType.Error,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
            cambiarAVentanaEstudiantes();
        }



        private void cambiarAVentanaEstudiantes()
        {
            Estudiantes estudiantes = new Estudiantes();
            estudiantes.Show();
            this.Close();
        }


        private void ClickConsultaHorarios(object sender, RoutedEventArgs e)
        {
            ConsultaHorarios consultaHorarios = new ConsultaHorarios();
            Application.Current.MainWindow = consultaHorarios;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<MenuPrincipalSGH>())
            {
                ((MenuPrincipalSGH)window).Close();
            }
            this.Close();
        }

        private void ClickGeneracionHorarios(object sender, RoutedEventArgs e)
        {
            GenerarHorario generarHorario = new GenerarHorario();
            Application.Current.MainWindow = generarHorario;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<MenuPrincipalSGH>())
            {
                ((MenuPrincipalSGH)window).Close();
            }
            this.Close();
        }

        private void ClickCalificacionesEstudiante(object sender, RoutedEventArgs e)
        {
            BuscadorEstudiante buscadorEstudiante = new BuscadorEstudiante();
            Application.Current.MainWindow = buscadorEstudiante;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<MenuPrincipalSGH>())
            {
                ((MenuPrincipalSGH)window).Close();
            }
            this.Close();
        }

        private void ClickCalificacionesGrupo(object sender, RoutedEventArgs e)
        {
            CalificacionesGrupal calificacionesGrupal = new CalificacionesGrupal();
            Application.Current.MainWindow = calificacionesGrupal;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<MenuPrincipalSGH>())
            {
                ((MenuPrincipalSGH)window).Close();
            }
            this.Close();
        }

        private void clickConsultarEstudiantes(object sender, RoutedEventArgs e)
        {
            Estudiantes estudiantes = new Estudiantes();
            Application.Current.MainWindow = estudiantes;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<MenuPrincipalSGH>())
            {
                ((MenuPrincipalSGH)window).Close();
            }
            this.Close();
        }

        private void clickRegistrarEstudiante(object sender, RoutedEventArgs e)
        {
            AgregarEstudiante agregarEstudiante = new AgregarEstudiante();
            Application.Current.MainWindow = agregarEstudiante;
            Application.Current.MainWindow.Show();
            foreach (Window window in Application.Current.Windows.OfType<MenuPrincipalSGH>())
            {
                ((MenuPrincipalSGH)window).Close();
            }
            this.Close();
        }
    }
}

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
    /// Lógica de interacción para BajaEstudiante.xaml
    /// </summary>
    public partial class BajaEstudiante : Window
    {
        private Persona persona = new Persona();
        private Estudiante estudiante = new Estudiante();
        private String id = "";
        private Baja baja = new Baja();
        public BajaEstudiante(String idPersona)
        {
            InitializeComponent();
            this.id = idPersona;

            recuperarPersonaYEstudiante();
            cargarDatosEstudiante();
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
            lbNo_SeguridadSocial.Content = estudiante.NumSeguroSocial;
            lbCURP.Content = persona.Curp;
            lbTipoSangre.Content = persona.TipoSangre;
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

        }

        private int verificarFormulario()
        {
            int estadoFormulario = 0;
            if (tbDescripcion.Text.Equals("") && (cbMotivo.SelectedIndex < 0))
            {
                estadoFormulario = -1;
            }
            else if (txbDocProbatorio.Text.Equals(""))
            {
                estadoFormulario = 0;
            }
            else if (!txbDocProbatorio.Text.Equals(""))
            {
                estadoFormulario = 1;
            }
            return estadoFormulario;
        }

        //Funcionalidad botones

        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            Estudiantes estudiantes = new Estudiantes();
            estudiantes.Show();
            this.Close();
        }

        private void ClickDarDeBaja(object sender, RoutedEventArgs e)
        {
            switch (verificarFormulario())
            {
                case 0:
                    if (mostrarVentanaConfirmacion())
                        darDeBaja(false);
                    break;
                case 1:
                    darDeBaja(true);
                    break;
                case -1:
                    mostrarVentanaFormularioVacio();
                    break;
            }
        }

        private void darDeBaja(bool conDoc)
        {
            string idBaja = Util.generarID(50);
            if (conDoc)
            {
                llenarObjetoBaja(idBaja, conDoc);
                if (PersonaDAO.darDeBajaPersona(persona.ID, baja))
                {
                    mostrarVentanaExito();

                }
                else
                {
                    mostrarVentanaError();
                }
            }
            else
            {
                llenarObjetoBaja(idBaja, conDoc);
                if (PersonaDAO.darDeBajaPersona(persona.ID, baja))
                {
                   mostrarVentanaExito();

                }
                else
                {
                    mostrarVentanaError();
                }

            }
        }

        private void llenarObjetoBaja(string id, bool conDoc)
        {
            baja.ID_Persona = persona.ID;
            baja.ID = id;
            baja.Descripcion = tbDescripcion.Text;
            ComboBoxItem item = (ComboBoxItem)cbMotivo.SelectedItem;
            baja.Motivo = "" + item.Content;
            if (conDoc == false)
                asignarDocVacio();
        }

        private void asignarDocVacio()
        {
            string s = "Sin Doc";
            byte[] docVacio = Encoding.Default.GetBytes(s);
            baja.DocumentoProbatorio = docVacio;
        }

        private void ClickAgregarDocProbatorio(object sender, RoutedEventArgs e)
        {
            DatosArchivo datosArchivo = Util.convertirPDFABites();
            baja.DocumentoProbatorio = datosArchivo.PDFEnByte;
            txbDocProbatorio.Text = datosArchivo.NombreArchivo;
        }

        //Dialogos de Texto
        private bool mostrarVentanaConfirmacion()
        {
            Alerta alerta = new Alerta("¿Está Baja realmente no tiene un documento probatorio?",
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

        private void mostrarVentanaFormularioVacio()
        {
            Alerta alerta = new Alerta("Los campos Motivo y Descripción Detallada del Motivo son obligatorios para continuar", Alertas.MessageType.Warning,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
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
            cambiarAVentanaProfesores();
        }

        private void mostrarVentanaExito()
        {
            Alerta alerta = new Alerta("El estudiante ha sido dado de baja", Alertas.MessageType.Success,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
            cambiarAVentanaProfesores();
        }

        private void cambiarAVentanaProfesores()
        {
            Estudiantes estudiantes = new Estudiantes();
            estudiantes.Show();
            this.Close();
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

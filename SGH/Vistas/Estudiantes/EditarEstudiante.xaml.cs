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
using System.IO;
using System.Drawing;
using SGH.Vistas.Alertas;
using SGH.Vistas.Horario.Consulta;
using SGH.Vistas.Estudiantes;
using SGH.Calificaciones;
using SGH.Vistas.Horario;

namespace SGH.Vistas.Estudiantes
{
    /// <summary>
    /// Lógica de interacción para EditarEstudiante.xaml
    /// </summary>
    public partial class EditarEstudiante : Window
    {
        private Persona persona = new Persona();
        private Estudiante estudiante = new Estudiante();
        private String id = "";

        public EditarEstudiante(string idPersona)
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
            txbNombre.Text = persona.Nombre;
            txbApellidoM.Text = persona.ApellidoMaterno;
            txbApellidoP.Text = persona.ApellidoPaterno;
            txbSeguridadSocial.Text = estudiante.NumSeguroSocial;
            txbCURP.Text = persona.Curp;
            cargarTipoSangre();
            inicializarNombreArchivos();
            Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, tbNombreFoto.Text + Util.generarID(30)));
            imgFoto.Source = new BitmapImage(uri);

        }

        private void cargarTipoSangre()
        {
            string tipoSangre = persona.TipoSangre;
            switch (tipoSangre)
            {
                case "A+":
                    cbTipoSangre.SelectedIndex = 0;
                    break;
                case "A-":
                    cbTipoSangre.SelectedIndex = 1;
                    break;
                case "B+":
                    cbTipoSangre.SelectedIndex = 2;
                    break;
                case "B-":
                    cbTipoSangre.SelectedIndex = 3;
                    break;
                case "AB+":
                    cbTipoSangre.SelectedIndex = 4;
                    break;
                case "AB-":
                    cbTipoSangre.SelectedIndex = 5;
                    break;
                case "O+":
                    cbTipoSangre.SelectedIndex = 6;
                    break;
                case "O-":
                    cbTipoSangre.SelectedIndex = 7;
                    break;
            }
            
        }

        private void inicializarNombreArchivos()
        {
            string nombreCompleto = Util.obtenerNombreSinEspacios(persona);

            tbNombreCURP.Text = "CURP_" + nombreCompleto + ".pdf";
            tbNombreActa.Text = "ActaNacimiento_" + nombreCompleto + ".pdf";
            tbNombreCURPTutor.Text = "CURPTutor_" + nombreCompleto + ".pdf";
            tbNombreCertificadoSecundaria.Text = "CertificadoSecundaria_" + nombreCompleto + ".pdf";
            tbNombreBuenaConducta.Text = "CartaBuenaConducta_" + nombreCompleto + ".pdf";
            tbNombreFoto.Text = "Foto_" + nombreCompleto + ".jpg";

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

        //Funcionalidades Manejo Archivos e Imagen

        private void clickAgregarCURP(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreCURP.Text = archivo.NombreArchivo;
                persona.DocCurp = archivo.PDFEnByte;
            }
        }

        private void clickAgregarActaNacimiento(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreActa.Text = archivo.NombreArchivo;
                persona.ActaNacimiento = archivo.PDFEnByte;
            }
        }



        private void clickAgregarCURPTutor(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreCURPTutor.Text = archivo.NombreArchivo;
                estudiante.DocCurpTutor = archivo.PDFEnByte;
            }
        }


        private void clickAgregarActaBuenaConducta(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreBuenaConducta.Text = archivo.NombreArchivo;
                estudiante.DocCartaBuenaConducta = archivo.PDFEnByte;
            }
        }

        private void clickAgregarCertificadoSecundaria(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreCertificadoSecundaria.Text = archivo.NombreArchivo;
                estudiante.DocCertificadoSecundaria = archivo.PDFEnByte;
            }
        }

        private void clickAgregarFoto(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivoImg = Util.convertirImgABitesYBitMap();
            if (archivoImg != null)
            {
                Console.WriteLine(archivoImg.ImagenBitMap.ToString());
                imgFoto.Source = archivoImg.ImagenBitMap;
                tbNombreFoto.Text = archivoImg.NombreArchivo;
                persona.Foto = archivoImg.ImgEnByte;
            }
        }

      

        //Guardar
        private void clickGuardarEstudiante(object sender, RoutedEventArgs e)
        {
            
            if (verificarFormulario())
            {
                

                if (registrarEnBD(estudiante.ID_Persona))
                    mostrarVentanaExito();
                else
                    mostrarVentanaError();
            }
            else
            {
                mostrarVentanaFormularioVacio();
            }
        }
        private bool verificarFormulario()
        {
            bool formularioAprobado = true;

            if (txbNombre.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbApellidoP.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbApellidoM.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbCURP.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreActa.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreCURP.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreCURPTutor.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbSeguridadSocial.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreFoto.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreBuenaConducta.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreCertificadoSecundaria.Text.Equals(""))
            {
                formularioAprobado = false;
            }

            return formularioAprobado;
        }

        private void llenarObjetoPersona(string idPersona)
        {
            persona.Nombre = txbNombre.Text;
            persona.ApellidoPaterno = txbApellidoP.Text;
            persona.ApellidoMaterno = txbApellidoM.Text;
            persona.Curp = txbCURP.Text;
            persona.Estado = "Activo";
            persona.ID = idPersona;
            persona.Clave_Escuela = "escuela-1";

            ComboBoxItem item = (ComboBoxItem)cbTipoSangre.SelectedItem;
            persona.TipoSangre = "" + item.Content;
        }

        private void llenarObjetoEstudiante(string idPersona)
        {
            estudiante.NumSeguroSocial = txbSeguridadSocial.Text;
            estudiante.ID_Persona = idPersona;
       
        }

        private bool registrarEnBD(string idPersona)
        {
            bool exito = false;
            llenarObjetoPersona(idPersona);
            if (PersonaDAO.editarPersona(persona))
            {
                llenarObjetoEstudiante(idPersona);
                if (EstudianteDAO.editarEstudiante(estudiante))
                {
                    exito = true;
                }
            }

            return exito;
        }

        //Dialogos

        private bool mostrarVentanaConfirmacion2()
        {
            Alerta alerta = new Alerta("Cualquier cambio en proceso será perdido, ¿está seguro de querer continuar?",
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
            Alerta alerta = new Alerta("Estudiante editado exitosamente", Alertas.MessageType.Success,
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

        private void mostrarVentanaFormularioVacio()
        {
            Alerta alerta = new Alerta("Debe llenar el formulario para continuar", Alertas.MessageType.Warning,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
        }

        private void cambiarAVentanaEstudiantes()
        {
            Estudiantes estudiantes = new Estudiantes();
            estudiantes.Show();
            this.Close();
        }


        //Funcionalidad botones

        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            if (mostrarVentanaConfirmacion2())
            {
                Estudiantes estudiantes = new Estudiantes();
                estudiantes.Show();
                this.Close();
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

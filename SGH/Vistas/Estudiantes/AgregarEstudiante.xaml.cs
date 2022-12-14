using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SGH.Modelos;
using SGH.Vistas.MenuPrincipal;
using SGH.Vistas.LogIn;
using SGH.Utiles;
using System.IO;
using SGH.Vistas.Alertas;
using System.Drawing;
using SGH.Vistas.Horario.Consulta;
using SGH.Calificaciones;
using System.Drawing.Imaging;
using SGH.DAOs;
using SGH.Vistas.Horario;
using System.Text.RegularExpressions;

namespace SGH.Vistas.Estudiantes
{
    /// <summary>
    /// Lógica de interacción para AgregarEstudiante.xaml
    /// </summary>
    public partial class AgregarEstudiante : Window
    {
        private Persona persona = new Persona();
        private Estudiante estudiante = new Estudiante();
        

        public AgregarEstudiante()
        {
            InitializeComponent();
            
        }

        private void clickAgregarEstudiante(object sender, RoutedEventArgs e)
        {
            String id = "";
            if (verificarFormulario() && validarNombreAppelidos(txbNombre.Text) && validarNombreAppelidos(txbApellidoM.Text) 
                && validarNombreAppelidos(txbApellidoP.Text) && validarCURP(txbCURP.Text) && validarSeguridadSocial(txbNombreSeguridadSocial.Text))
            {
                id = Util.generarID(50);

                if (registrarEnBD(id))
                    mostrarVentanaExito();
                else
                    mostrarVentanaError();
            }
            else
            {
                mostrarVentana();
            }
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
            estudiante.NumSeguroSocial = txbNombreSeguridadSocial.Text;
            estudiante.ID_Persona = idPersona;
            estudiante.Matricula = "ESC1-" + txbCURP.Text;
            estudiante.ID_Grupo = "Sin grupo";
        }

        private bool registrarEnBD(string idPersona)
        {
            bool exito = false;
            llenarObjetoPersona(idPersona);
            if (PersonaDAO.registrarPersona(persona))
            {
                llenarObjetoEstudiante(idPersona);
                if (EstudianteDAO.registrarEstudiante(estudiante))
                {                
                        exito = true;
                }
            }

            return exito;
        }

        //Dialogos

        private void mostrarVentana()
        {
            if (verificarFormulario())
            {
                mostrarVentanaErrosDatos();
            }
            else
            {
                mostrarVentanaFormularioVacio();
                
            }
        }

        private bool mostrarVentanaConfirmacion2()
        {
            Alerta alerta = new Alerta("Cualquier registro en proceso será perdido, ¿está seguro de querer continuar?",
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
            Alerta alerta = new Alerta("Estudiante resgitrado exitosamente", Alertas.MessageType.Success,
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

        private void mostrarVentanaErrosDatos()
        {
            Alerta alerta = new Alerta("Los datos ingresados son invalidos", Alertas.MessageType.Warning,
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
            Estudiantes estudiantes  = new Estudiantes();
            estudiantes.Show();
            this.Close();
        }

        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            if (mostrarVentanaConfirmacion2())
            {
                Estudiantes estudiantes = new Estudiantes();
                estudiantes.Show();
                this.Close();
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

        //Validaciones
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
            else if (txbNombreSeguridadSocial.Text.Equals(""))
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

        public Boolean validarNombreAppelidos(string nombre)
        {

            if (Regex.IsMatch(nombre, @"^([a-zA-Z]+)(\s[a-zA-Z]+)*$"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        
        public Boolean validarCURP(string nombre)
        {
            if (Regex.IsMatch(nombre, @"[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}" +
                                "(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])" +
                                "[HM]{1}" +
                                "(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)" +
                                "[B-DF-HJ-NP-TV-Z]{3}" +
                                "[0-9A-Z]{1}[0-9]{1}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean validarSeguridadSocial(string nombre)
        {

            if (Regex.IsMatch(nombre, @"^\d{10}$"))
            {
                return true;
            }
            else
            {
                return false;
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

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

namespace SGH.Vistas.Profesores
{
    /// <summary>
    /// Lógica de interacción para ConsultarProfesorBaja.xaml
    /// </summary>
    public partial class ConsultarProfesorBaja : Window
    {
        private static Administrador administradorMenu = new Administrador();
        private Persona persona = new Persona();
        private Profesor profesor = new Profesor();
        private Baja baja = new Baja();
        private String id = "";

        public ConsultarProfesorBaja(String idPersona)
        {
            InitializeComponent();
            this.id = idPersona;

            recuperarPersonaProfesorYBaja();
            cargarDatosProfesor();


            administradorMenu.Rol = "secretaria";
            administradorMenu.NombreCompleto = "usuario prueba";

            FiltrarMenus(administradorMenu.Rol);
            SetInformacionAdministrador(administradorMenu);
        }

        private void recuperarPersonaProfesorYBaja()
        {
            persona = PersonaDAO.recuperarPersonaID(id);
            profesor = ProfesorDAO.recuperarProfesorID(id);
            baja = BajaDAO.recuperarBaja(id);
        }

        private void cargarDatosProfesor()
        {
            lbNombre.Content = persona.Nombre;
            lbApellidoM.Content = persona.ApellidoMaterno;
            lbApellidoP.Content = persona.ApellidoPaterno;
            lbCarrera.Content = profesor.Carrera;
            lbCURP.Content = persona.Curp;
            lbTipoSangre.Content = persona.TipoSangre;
            tbDescripcion.Text = baja.Descripcion;
            lbMotivo.Content = baja.Motivo;
            inicializarNombreArchivos();
            Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, tbNombreFoto.Text));
            imgFoto.Source = new BitmapImage(uri);
        }



        private void inicializarNombreArchivos()
        {
            string nombreCompleto = Util.obtenerNombreSinEspacios(persona);
            tbNombreTitulo.Text = "Titulo_" + nombreCompleto + ".pdf";
            tbNombreActa.Text = "ActaNacimiento_" + nombreCompleto + ".pdf";
            tbNombreINE.Text = "INE_" + nombreCompleto + ".pdf";
            tbNombreCURP.Text = "CURP_" + nombreCompleto + ".pdf";
            tbNombreContrato.Text = "Contrato_" + nombreCompleto + ".pdf";
            tbNombreFoto.Text = "Foto_" + nombreCompleto;
            if (Util.comprobarArchivoByteVacio(baja.DocumentoProbatorio))
                txbDocProbatorio.Text = "DocProbatorio_" + nombreCompleto + ".pdf";
        }


        //Funcionalidades Archivos
        private void clickAbrirArchivoTitulo(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreTitulo.Text.Equals(""))
            {
                Util.abrirArchivoPDF(profesor.Titulo, tbNombreTitulo.Text);
            }
        }

        private void clickAbrirArchivoActaNacimiento(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreActa.Text.Equals(""))
            {
                Util.abrirArchivoPDF(persona.ActaNacimiento, tbNombreActa.Text);
            }
        }

        private void clickAbrirArchivoINE(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreINE.Text.Equals(""))
            {
                Util.abrirArchivoPDF(profesor.INE, tbNombreINE.Text);
            }
        }

        private void clickAbrirArchivoCURPDoc(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreCURP.Text.Equals(""))
            {
                Util.abrirArchivoPDF(persona.DocCurp, tbNombreCURP.Text);
            }
        }

        private void clickAbrirArchivoContrato(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreContrato.Text.Equals(""))
            {
                Util.abrirArchivoPDF(profesor.DocContrato, tbNombreContrato.Text);
            }
        }

        private void clickAbrirArchivoDocProb(object sender, MouseButtonEventArgs e)
        {
            if (!txbDocProbatorio.Text.Equals(""))
            {
                Util.abrirArchivoPDF(profesor.DocContrato, tbNombreContrato.Text);
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
            Profesores profesores = new Profesores();
            profesores.Show();
            this.Close();
        }

        //Dialogos
        private bool mostrarVentanaConfirmacionCancelarBaja()
        {
            Alerta alerta = new Alerta("¿Está seguro de querer Cancelar la Baja del profesor?",
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
            cambiarAVentanaProfesores();
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



        private void cambiarAVentanaProfesores()
        {
            Profesores profesores = new Profesores();
            profesores.Show();
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

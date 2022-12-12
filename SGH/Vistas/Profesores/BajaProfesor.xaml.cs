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
    /// Lógica de interacción para BajaProfesor.xaml
    /// </summary>
    public partial class BajaProfesor : Window
    {
        private static Administrador administradorMenu = new Administrador();
        private Persona persona = new Persona();
        private Profesor profesor = new Profesor();
        private String id = "";
        private Baja baja = new Baja();

        public BajaProfesor(String idPersona)
        {
            InitializeComponent();
            this.id = idPersona;

            recuperarPersonaYProfesor();
            cargarDatosProfesor();


            administradorMenu.Rol = "secretaria";
            administradorMenu.NombreCompleto = "usuario prueba";

            FiltrarMenus(administradorMenu.Rol);
            SetInformacionAdministrador(administradorMenu);
        }


        private void recuperarPersonaYProfesor()
        {
            persona = PersonaDAO.recuperarPersonaID(id);
            profesor = ProfesorDAO.recuperarProfesorID(id);
        }

        private void cargarDatosProfesor()
        {
            lbNombre.Content = persona.Nombre;
            lbApellidoM.Content = persona.ApellidoMaterno;
            lbApellidoP.Content = persona.ApellidoPaterno;
            lbCarrera.Content = profesor.Carrera;
            lbCURP.Content = persona.Curp;
            lbTipoSangre.Content = persona.TipoSangre;
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
            Profesores profesores = new Profesores();
            profesores.Show();
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
                    if (MateriaDAO.borrarMateriasProfesor(profesor.RFC))
                        mostrarVentanaExito();
                    else
                        mostrarVentanaError();
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
                    if (MateriaDAO.borrarMateriasProfesor(profesor.RFC))
                        mostrarVentanaExito();
                    else
                        mostrarVentanaError();
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
            Alerta alerta = new Alerta("El profesor ha sido dado de baja", Alertas.MessageType.Success,
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

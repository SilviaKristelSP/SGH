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
using SGH.Modelos;
using SGH.Vistas.LogIn;
using SGH.Vistas.MenuPrincipal;
using SGH.DAOs;
using SGH.Utiles;
using SGH.Vistas.Alertas;
using System.IO;
using SGH.Vistas.Horario.Consulta;
using SGH.Vistas.Estudiantes;
using SGH.Calificaciones;
using SGH.Vistas.Horario;

namespace SGH.Vistas.Estudiantes
{
    /// <summary>
    /// Lógica de interacción para Estudiantes.xaml
    /// </summary>
    public partial class Estudiantes : Window
    {
        private static Administrador administradorMenu = new Administrador();
        private List<DatosTabla> datosConvertidos = new List<DatosTabla>();
        private List<DatosTabla> filtros = new List<DatosTabla>();
        public Estudiantes()
        {
            InitializeComponent();
            administradorMenu.Rol = "secretaria";
            administradorMenu.NombreCompleto = "usuario prueba";

            FiltrarMenus(administradorMenu.Rol);
            SetInformacionAdministrador(administradorMenu);

            ConfigurarVentanaInicio();
        }

        //Configuración Ventana
        private void ConfigurarVentanaInicio()
        {
            cbEstado.SelectedIndex = 0;
            btnCancelarBaja.Visibility = System.Windows.Visibility.Collapsed;
            btnConsultarBaja.Visibility = System.Windows.Visibility.Collapsed;
            LlenarTabla();

        }

        private void LlenarTabla()
        {
            datosConvertidos = ConfigurarADatosTabla(PersonaDAO.recuperarPersonas(EstudianteDAO.recuperarIDsEstudiantesActivos(), "Activo"));
            dgEstudiantes.ItemsSource = datosConvertidos;
        }

        private void LlenarTablaFiltros()
        {
            datosConvertidos = ConfigurarADatosTabla(PersonaDAO.recuperarPersonas(EstudianteDAO.recuperarIDsEstudiantesActivos(), "Baja"));
            dgEstudiantes.ItemsSource = datosConvertidos;
        }

        private void filtrosGrupoActivos() {
            datosConvertidos = ConfigurarADatosTabla(PersonaDAO.recuperarPersonas(EstudianteDAO.recuperarIDsEstudiantesActivos(), "Activo"));
            dgEstudiantes.ItemsSource = datosConvertidos;
        }

   
        private List<DatosTabla> ConfigurarADatosTabla(List<Persona> personas)
        {
            List<DatosTabla> datos = new List<DatosTabla>();
            if (personas != null)
            {
                foreach (Persona persona in personas)
                {
                    DatosTabla dt = new DatosTabla();
                    dt.Id = persona.ID;
                    dt.NombreCompleto = Util.obtenerNombreConEspacios(persona);
                    dt.Matricula = "ESC1-" + persona.Curp;
                    datos.Add(dt);
                }
            }
            else
            {
                datos = null;
            }


            return datos;
        }

        


        //Funcionalidades Botones
        private void ClickLimpiarFiltros(object sender, RoutedEventArgs e)
        {
            
            cbEstado.SelectedIndex = 0;
            dgEstudiantes.ItemsSource = null;
            dgEstudiantes.Items.Clear();
            LlenarTabla();
        }

        private void ClickAplicarFiltros(object sender, RoutedEventArgs e)
        {
            if (cbEstado.SelectedIndex == 1)
            {
                btnAgregar.Visibility = System.Windows.Visibility.Collapsed;
                btnConsultar.Visibility = System.Windows.Visibility.Collapsed;
                btnDarDeBaja.Visibility = System.Windows.Visibility.Collapsed;
                btnEditar.Visibility = System.Windows.Visibility.Collapsed;
                btnCancelarBaja.Visibility = System.Windows.Visibility.Visible;
                btnConsultarBaja.Visibility = System.Windows.Visibility.Visible;
                LlenarTablaFiltros();
                
                
            }
            else if (cbEstado.SelectedIndex == 0)
            {
                btnAgregar.Visibility = System.Windows.Visibility.Visible;
                btnConsultar.Visibility = System.Windows.Visibility.Visible;
                btnDarDeBaja.Visibility = System.Windows.Visibility.Visible;
                btnEditar.Visibility = System.Windows.Visibility.Visible;
                btnCancelarBaja.Visibility = System.Windows.Visibility.Collapsed;
                btnConsultarBaja.Visibility = System.Windows.Visibility.Collapsed;
                LlenarTabla();
                


            }
        }

        private void reconfigurarBotones()
        {
            btnAgregar.Visibility = System.Windows.Visibility.Visible;
            btnConsultar.Visibility = System.Windows.Visibility.Visible;
            btnDarDeBaja.Visibility = System.Windows.Visibility.Visible;
            btnEditar.Visibility = System.Windows.Visibility.Visible;
            btnCancelarBaja.Visibility = System.Windows.Visibility.Collapsed;
            btnConsultarBaja.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ClickBuscar(object sender, RoutedEventArgs e)
        {
            String textoBusqueda = txbBuscador.Text;
            if (!textoBusqueda.Equals("Buscar por nombre o matricula"))
            {
                List<DatosTabla> datosFiltrados = new List<DatosTabla>();
                foreach (DatosTabla dt in datosConvertidos)
                {
                    if (dt.NombreCompleto.Contains(textoBusqueda))
                    {
                        datosFiltrados.Add(dt);
                    }
                    if (dt.Matricula.Contains(textoBusqueda))
                    {
                        datosFiltrados.Add(dt);
                    }
                }
                dgEstudiantes.ItemsSource = datosFiltrados;
            }
        }

        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            AgregarEstudiante agregarEstudiante = new AgregarEstudiante();
            agregarEstudiante.Show();
            this.Close();
        }

        private void ClickCancelarBaja(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgEstudiantes.SelectedItem;
            if (seleccionado != null)
            {
                if (mostrarVentanaConfirmacionCancelarBaja())
                {
                    if (PersonaDAO.cancelarBajaPersona(seleccionado.Id))
                    {
                        mostrarVentanaExito();
                        LlenarTabla();
                     
                        reconfigurarBotones();
                    }
                    else
                    {
                        mostrarVentanaError();
                    }
                }
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

        private void ClickDarDeBaja(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgEstudiantes.SelectedItem;
            if (seleccionado != null)
            {
                BajaEstudiante bajaEstudiante = new BajaEstudiante(seleccionado.Id);
                bajaEstudiante.Show();
                this.Close();
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

        private void ClickEditar(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgEstudiantes.SelectedItem;
            if (seleccionado != null)
            {
                EditarEstudiante editarEstudiante = new EditarEstudiante(seleccionado.Id);
                editarEstudiante.Show();
                this.Close();
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

        private void ClickConsultar(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgEstudiantes.SelectedItem;
            
            
            if (seleccionado != null)
            {
                
                ConsultarEstudiante consultarEstudiante = new ConsultarEstudiante(seleccionado.Id);
                consultarEstudiante.Show();
                this.Close();
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

 
        private void ClickConsultarBaja(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgEstudiantes.SelectedItem;
            if (seleccionado != null)
            {
                ConsultarEstudianteBAja consultarEstudianteBaja = new ConsultarEstudianteBAja(seleccionado.Id);
                consultarEstudianteBaja.Show();
                this.Close();
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

        //Dialogos
        private void mostrarVentanaSeleccionar()
        {
            Alerta alerta = new Alerta("Debe seleccionar un estudiante de la lista para continuar", Alertas.MessageType.Warning,
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
        }

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

        private void clickEnBuscador(object sender, MouseButtonEventArgs e)
        {
            txbBuscador.Text = "";
            txbBuscador.Clear();
        }

        
    }

    partial class DatosTabla
    {
        public string Id { get; set; }
        public string NombreCompleto { get; set; }
        public BitmapImage Imagen { get; set; }
        public string Matricula { get; set; }
    }
}

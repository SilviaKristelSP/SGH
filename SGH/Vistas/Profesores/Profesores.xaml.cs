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

namespace SGH.Vistas.Profesores
{
    /// <summary>
    /// Lógica de interacción para Profesores.xaml
    /// </summary>
    public partial class Profesores : Window
    {
        private static Administrador administradorMenu = new Administrador();
        private List<Materia> materias = new List<Materia>();
        private List<DatosTabla> datosConvertidos = new List<DatosTabla>();

        public Profesores()
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
            LlenarTabla();
            ConfigurarComboBoxes();
        }

        private void LlenarTabla()
        {
            datosConvertidos = ConfigurarADatosTabla(PersonaDAO.recuperarPersonas(ProfesorDAO.recuperarIDsProfesoresActivos(), "Activo"));
            dgProfesores.ItemsSource = datosConvertidos;
        }

        private void LlenarTablaFiltros()
        {
            datosConvertidos = ConfigurarADatosTabla(PersonaDAO.recuperarPersonas(ProfesorDAO.recuperarIDsProfesoresActivos(), "Baja"));
            dgProfesores.ItemsSource = datosConvertidos;
        }

        private void ConfigurarComboBoxes()
        {
            cbCarrera.ItemsSource = ProfesorDAO.recuperarCarreras();
            int semestreMax = MateriaDAO.obtenerUltimoSemestre();
            for (int i = 1; i <= semestreMax; i++)
            {
                cbSemestre.Items.Add(i.ToString());
            }
        }

        private List<DatosTabla> ConfigurarADatosTabla(List<Persona> personas)
        {
            List<DatosTabla> datos = new List<DatosTabla>();
            if(personas != null)
            {
                foreach (Persona persona in personas)
                {
                    DatosTabla dt = new DatosTabla();
                    dt.Id = persona.ID;
                    dt.NombreCompleto = Util.obtenerNombreConEspacios(persona);
                    /*Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, Util.obtenerNombreSinEspacios(persona)));
                    dt.Imagen = new BitmapImage(uri);*/
                    datos.Add(dt);
                }
            }
            else
            {
                datos = null;
            }

            
            return datos;
        }

        private void Semestre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string semSeleccionado = (string)cbSemestre.SelectedItem;

                int sem = Int32.Parse(semSeleccionado);

                materias = MateriaDAO.recuperarMaterias();

                List<Materia> materiasFiltradas = new List<Materia>();

                foreach (Materia m in materias)
                {
                    if (sem == m.Semestre)
                    {
                        materiasFiltradas.Add(m);
                    }
                }
                cbMateria.Items.Clear();
                cbMateria.ItemsSource = materiasFiltradas;
            }
            catch(System.ArgumentNullException ex){}

        }

        //Funcionalidades Botones
        private void ClickLimpiarFiltros(object sender, RoutedEventArgs e)
        {
            cbMateria.SelectedIndex = -1;
            cbSemestre.SelectedIndex = -1;
            cbEstado.SelectedIndex = 0;
            cbCarrera.SelectedIndex = -1;
            dgProfesores.ItemsSource = null;
            dgProfesores.Items.Clear();
            LlenarTabla();
        }

        private void ClickAplicarFiltros(object sender, RoutedEventArgs e)
        {
            if(cbEstado.SelectedIndex == 1)
            {
                btnAgregar.Visibility = System.Windows.Visibility.Collapsed;
                btnConsultar.Visibility = System.Windows.Visibility.Collapsed;
                btnDarDeBaja.Visibility = System.Windows.Visibility.Collapsed;
                btnEditar.Visibility = System.Windows.Visibility.Collapsed;
                btnCancelarBaja.Visibility = System.Windows.Visibility.Visible;
                LlenarTablaFiltros();
            }
            else if(cbEstado.SelectedIndex == 0)
            {
                btnAgregar.Visibility = System.Windows.Visibility.Visible;
                btnConsultar.Visibility = System.Windows.Visibility.Visible;
                btnDarDeBaja.Visibility = System.Windows.Visibility.Visible;
                btnEditar.Visibility = System.Windows.Visibility.Visible;
                btnCancelarBaja.Visibility = System.Windows.Visibility.Collapsed;
                LlenarTabla();
            }
        }

        private void ClickBuscar(object sender, RoutedEventArgs e)
        {
            String textoBusqueda = txbBuscador.Text;
            if(!textoBusqueda.Equals("Buscar por nombre"))
            {
                List<DatosTabla> datosFiltrados = new List<DatosTabla>();
                foreach (DatosTabla dt in datosConvertidos)
                {
                    if (dt.NombreCompleto.Contains(textoBusqueda))
                    {
                        datosFiltrados.Add(dt);
                    }
                }
                dgProfesores.ItemsSource = datosFiltrados;
            }
        }

        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            AgregarProfesor agregarProfesor = new AgregarProfesor();
            agregarProfesor.Show();
            this.Close();
        }

        private void ClickCancelarBaja(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgProfesores.SelectedItem;
            if (seleccionado != null)
            {
                //TO DO
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

        private void ClickDarDeBaja(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgProfesores.SelectedItem;
            if (seleccionado != null)
            {
                if (mostrarVentanaConfirmacionCancelarBaja())
                {
                    if (PersonaDAO.cancelarBajaPersona(seleccionado.Id))
                        mostrarVentanaExito();
                    else
                        mostrarVentanaError();
                }
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

        private void ClickEditar(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgProfesores.SelectedItem;
            if(seleccionado != null)
            {
                //Editar
            }
            else
            {
                mostrarVentanaSeleccionar();
            }
        }

        private void ClickConsultar(object sender, RoutedEventArgs e)
        {
            DatosTabla seleccionado = (DatosTabla)dgProfesores.SelectedItem;
            if (seleccionado != null)
            {
                ConsultarProfesor consultarProfesor = new ConsultarProfesor(seleccionado.Id);
                consultarProfesor.Show();
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
            Alerta alerta = new Alerta("Debe seleccionar un profesor de la lista para continuar", Alertas.MessageType.Warning,
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

    partial class DatosTabla{
        public string Id { get; set; }
        public string NombreCompleto { get; set; }
        public BitmapImage Imagen { get; set; }
    }
}

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
    /// Lógica de interacción para EdicionProfesor.xaml
    /// </summary>
    public partial class EdicionProfesor : Window
    {
        private static Administrador administradorMenu = new Administrador();
        private Persona persona = new Persona();
        private Profesor profesor = new Profesor();
        private List<Profesor_Materia> impartidasPrevioEdicion = new List<Profesor_Materia>();
        private String id = "";

        public EdicionProfesor(String idPersona)
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
            txbNombre.Text = persona.Nombre;
            txbApellidoM.Text = persona.ApellidoMaterno;
            txbApellidoP.Text = persona.ApellidoPaterno;
            txbCarrera.Text = profesor.Carrera;
            txbCURP.Text = persona.Curp;
            for(int i = 0; i < 8; i++)
            {
                cbTipoSangre.SelectedIndex = i;
                ComboBoxItem item = (ComboBoxItem)cbTipoSangre.SelectedItem;
                String ts = "" + item.Content;
                if (ts.Equals(persona.TipoSangre))
                {
                    cbTipoSangre.SelectedIndex = i;
                    break;
                }
            }
            inicializarNombreArchivos();
            /*Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, tbNombreFoto.Text));
            imgFoto.Source = new BitmapImage(uri);*/
            cargarMaterias();
            marcarMateriasSeleccionadas();
        }

        private void cargarMaterias()
        {
            List<Materia> listaMaterias = MateriaDAO.recuperarMaterias();
            int semestreMax = MateriaDAO.obtenerUltimoSemestre();
            Console.WriteLine("" + listaMaterias.Count);
            if (listaMaterias.Count <= 0)
            {
                Label labelDinamico = new Label();
                labelDinamico.FontSize = 14;
                labelDinamico.FontWeight = System.Windows.FontWeights.Bold;
                labelDinamico.Content = "No tiene materias asiganadas";
                wpMaterias.Children.Add(labelDinamico);
            }
            else
            {
                for (int i = 1; i <= semestreMax; i++)
                {
                    Label labelDinamico = new Label();
                    labelDinamico.FontSize = 14;
                    labelDinamico.FontWeight = System.Windows.FontWeights.Bold;
                    labelDinamico.Content = "Semestre " + i;
                    wpMaterias.Children.Add(labelDinamico);
                    foreach (Materia materia in listaMaterias)
                    {
                        if (materia.Semestre == i)
                        {
                            CheckBox checkDinamico = new CheckBox();

                            labelDinamico.Margin = new Thickness(5);
                            checkDinamico.Content = materia.Nombre;
                            checkDinamico.Name = materia.NRC;
                            checkDinamico.Margin = new Thickness(0, 0, 15, 0);

                            wpMaterias.Children.Add(checkDinamico);
                        }
                    }
                }
            }

        }

        private void marcarMateriasSeleccionadas()
        {
            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();
            impartidasPrevioEdicion = MateriaDAO.recuperarProfesorMaterias(profesor.RFC);
            foreach (CheckBox check in listaCheckDinamicos)
            {
                foreach(Profesor_Materia materia in impartidasPrevioEdicion)
                {
                    if (check.Name.Equals(materia.NRC_Materia))
                        check.IsChecked = true;
                }
            }
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

        private void modificarDatosPersonaYProfesor()
        {
            persona.Nombre = txbNombre.Text;
            persona.ApellidoPaterno = txbApellidoP.Text;
            persona.ApellidoMaterno = txbApellidoM.Text;
            persona.Curp = txbCURP.Text;
            persona.Estado = "Activo";
            persona.ID = id;
            persona.Clave_Escuela = "escuela-1";

            ComboBoxItem item = (ComboBoxItem)cbTipoSangre.SelectedItem;
            persona.TipoSangre = "" + item.Content;

            profesor.RFC = txbRFC.Text;
            profesor.Carrera = txbCarrera.Text;
            profesor.ID_Persona = id;
        }

        private List<Profesor_Materia> encontrarMateriasAQuitar()
        {
            List<Profesor_Materia> aBorrar = new List<Profesor_Materia>();
            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();
            foreach (CheckBox check in listaCheckDinamicos)
            {
                foreach (Profesor_Materia materia in impartidasPrevioEdicion)
                {
                    if (check.Name.Equals(materia.NRC_Materia) && check.IsChecked == false)
                        aBorrar.Add(materia);
                }
            }

            if (aBorrar.Count <= 0)
                aBorrar = null;

            return aBorrar;
        }

        private List<Profesor_Materia> encontrarMateriasAAsignar()
        {
            List<Profesor_Materia> aAsignar = new List<Profesor_Materia>();
            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();
            foreach (CheckBox check in listaCheckDinamicos)
            {
                foreach (Profesor_Materia materia in impartidasPrevioEdicion)
                {
                    if (check.IsChecked == true && (!check.Name.Equals(materia.NRC_Materia)))
                    {
                        Profesor_Materia profesor_Materia = new Profesor_Materia();
                        profesor_Materia.RFC_Profesor = profesor.RFC;
                        profesor_Materia.NRC_Materia = check.Name;
                        aAsignar.Add(profesor_Materia);
                    }
                }
            }

            if (aAsignar.Count <= 0)
                aAsignar = null;

            return aAsignar;
        }

        private List<Profesor_Materia>[] generarArregloMateriasProfesor()
        {
            List<Profesor_Materia>[] m = new List<Profesor_Materia>[2];
            m[0] = encontrarMateriasAAsignar();
            m[1] = encontrarMateriasAQuitar();
            return m;
        }

        //Funcionalidades Archivos

        private void clickAgregarTitulo(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreTitulo.Text = archivo.NombreArchivo;
                profesor.Titulo = archivo.PDFEnByte;
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

        private void clickAgregarINE(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreINE.Text = archivo.NombreArchivo;
                profesor.INE = archivo.PDFEnByte;
            }

        }

        private void clickAgregarContrato(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreContrato.Text = archivo.NombreArchivo;
                profesor.DocContrato = archivo.PDFEnByte;
            }
        }

        private void clickAgregarCURPDoc(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreCURP.Text = archivo.NombreArchivo;
                persona.DocCurp = archivo.PDFEnByte;
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

        //Funcionalidad botones

        private void clickGuardarCambios(object sender, RoutedEventArgs e)
        {

        }

        private void ClickRetroceder(object sender, RoutedEventArgs e)
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


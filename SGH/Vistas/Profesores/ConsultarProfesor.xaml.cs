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

namespace SGH.Vistas.Profesores
{
    /// <summary>
    /// Lógica de interacción para ConsultarProfesor.xaml
    /// </summary>
    public partial class ConsultarProfesor : Window
    {
        private Persona persona = new Persona();
        private Profesor profesor = new Profesor();
        private String id = "";

        public ConsultarProfesor(String idPersona)
        {
            InitializeComponent();
            this.id = idPersona;

            recuperarPersonaYProfesor();
            cargarDatosProfesor();
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
            Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, tbNombreFoto.Text + Util.generarID(30)));
            imgFoto.Source = new BitmapImage(uri);
          cargarMaterias();
        }

        private void cargarMaterias()
        {
            
            List<Materia> listaMaterias = MateriaDAO.recuperarMateriasAsignadas(MateriaDAO.obtenerIDsMateriasAsignadas(profesor.RFC));
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
                            checkDinamico.IsChecked = true;
                            checkDinamico.Margin = new Thickness(0, 0, 15, 0);

                            wpMaterias.Children.Add(checkDinamico);
                        }
                    }
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

        //Funcionalidad botones
        private void clickEditar(object sender, RoutedEventArgs e)
        {
            EdicionProfesor edicion = new EdicionProfesor(persona.ID);
            Application.Current.MainWindow = edicion;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<ConsultarProfesor>())
            {
                ((ConsultarProfesor)window).Close();
            }
        }

        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            ListaProfesores lista = new ListaProfesores();
            Application.Current.MainWindow = lista;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<ConsultarProfesor>())
            {
                ((ConsultarProfesor)window).Close();
            }
        }
    }
}

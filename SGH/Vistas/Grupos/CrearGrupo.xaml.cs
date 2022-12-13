using SGH.DAOs;
using SGH.Modelos;
using SGH.Validaciones;
using SGH.Vistas.Alertas;
using SGH.Vistas.Horario;
using SGH.Vistas.MenuPrincipal;
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

namespace SGH
{
    /// <summary>
    /// Lógica de interacción para CrearGrupo.xaml
    /// </summary>
    public partial class CrearGrupo : Window
    {
        Grupo grupo = new Grupo();
        Estudiante estudiante = new Estudiante();
        List<Persona> personas = new List<Persona>();
        List<String> nombresAgregados = new List<string>();
        public CrearGrupo()
        {
            InitializeComponent();
            CargarSemestre();
            CargarLetraGrupo();
            CargarNombresEstudiantesSinGrupo();
        }

        public void CargarSemestre()
        {
            for(int i = 1; i<4 ; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = i + "";
                textBlock.FontSize = 20;
                semestreComboBox.Items.Add(textBlock);
            }
        }

        public void CargarLetraGrupo()
        {
            for( char c = 'A'; c <= 'Z'; c++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = c + "";
                textBlock.FontSize=20;
                letraGrupoComboBox.Items.Add(textBlock);
            }
        }

        public void CargarNombresEstudiantesSinGrupo()
        {
            List<Persona> estudiantes = GrupoDAO.recuperarNombresEstudiantes(GrupoDAO.recuperarEstudiantesInactivos());

            personas = estudiantes;

            if (estudiantes != null)
            {
                foreach (var item in estudiantes)
                {
                    String nombre = item.Nombre;
                    estudiantesSinGrupoDataGrid.Items.Add(nombre);
                }
            }
        }

        private void GeneracionGrupo(object sender, RoutedEventArgs e)
        {
            int semestre = int.Parse(semestreComboBox.Text);
            String letra = letraGrupoComboBox.Text;
            grupo.Semestre = semestre;
            grupo.Letra = letra;
            generarIDGrupo();

            if (GrupoValidator.verificarExistencia(grupo))
            {
                bool agregado = GrupoDAO.registrarGrupo(grupo);

                registroExitoso(agregado);
                estudiantesAgregados.Items.Clear();
                grupo = GrupoDAO.buscarGrupo(semestre,letra);
            }
            else
            {
                MessageBox.Show("Grupo ya existente");
            }

            
        }

        private void generarIDGrupo()
        {
            String ultimoID = GrupoDAO.recuperarultimoIDGrupo();
            int numeroConsecutivo = Int32.Parse(ultimoID);
            String nuevoID = "";

            if(ultimoID != null)
            {
                nuevoID = "grupo-" + (numeroConsecutivo + 1);
                Console.WriteLine(nuevoID);
                grupo.ID = nuevoID;
            }
        }


        private void AgregarEstudiantealGrupo(object sender, RoutedEventArgs e)
        {
            String nombreAnadido = "";
            nombreAnadido = estudiantesSinGrupoDataGrid.SelectedItem.ToString();
            if (nombreAnadido != null)
            {
                if (verificarGrupo())
                {
                    estudiante = GrupoDAO.recuperarEstudianteporIDPersona(buscarPersonaPorNombre(nombreAnadido).ID);

                    estudiante.ID_Grupo = grupo.ID;

                    bool agregado = EstudianteDAO.registrarEstudiante(estudiante);
                    registroExitoso(agregado);
                    actualizarListaEstudiantesAgregados(nombreAnadido);
                    estudiantesSinGrupoDataGrid.Items.Remove(nombreAnadido);
                    
                }
                else
                {
                    MessageBox.Show("No ha generado al grupo seleccionado");
                }
            }
            else
            {
                MessageBox.Show("Seleccione algun estudiante");
            }


        }


        public void actualizarListaEstudiantesAgregados(String nombre)
        {
            nombresAgregados.Add(nombre);
            estudiantesAgregados.ItemsSource = nombresAgregados;
        }

        public void registroExitoso(bool resultado)
        {
            if (resultado)
            {
                MessageBox.Show("Registro exitoso");
            }
            else
            {
                MessageBox.Show("Ocurrió un error inténtelo mas tarde");
            }
        }

        public Persona buscarPersonaPorNombre(String nombre)
        {
            Persona persona = new Persona();
            foreach(Persona p in personas)
            {
                if (p.Nombre.Equals(nombre))
                {
                    persona = p;
                }
            }
            return persona;
        }

        public bool verificarGrupo()
        {
            bool verificacionExitosa = false;
            int semestre = int.Parse(semestreComboBox.Text);
            if (grupo.Semestre == semestre && grupo.Letra == letraGrupoComboBox.Text)
            {
                verificacionExitosa = true;
            }
            return verificacionExitosa;
        }

        private void Salir(object sender, MouseButtonEventArgs e)
        {
            MenuPrincipalSGH menuPrincipalSGH = new MenuPrincipalSGH();
            Application.Current.MainWindow = menuPrincipalSGH;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<CrearGrupo>())
                ((CrearGrupo)window).Close();
        }
    }
}

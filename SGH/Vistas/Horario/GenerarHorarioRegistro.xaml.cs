using SGH.Vistas.Horario.Consulta;
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
using SGH.DAOs;
using SGH.Modelos;
using System.Reflection;
using System.Windows.Media.Media3D;

namespace SGH.Vistas.Horario
{
    /// <summary>
    /// Lógica de interacción para GenerarHorarioRegistro.xaml
    /// </summary>
    public partial class GenerarHorarioRegistro : Window
    {

        #region listas tabla horario
        public List<int> arregloSiete = new List<int>();
        public List<int> arregloOcho = new List<int>();
        public List<int> arregloNueve = new List<int>();
        public List<int> arregloDiez = new List<int>();
        public List<int> arregloOnce = new List<int>();
        public List<int> arregloDoce = new List<int>();

        public List<int> arregloTrece = new List<int>();
        public List<int> arregloCatorce = new List<int>();
        public List<int> arregloQuince = new List<int>();
        public List<int> arregloDieciseis = new List<int>();
        public List<int> arregloDiecisiete = new List<int>();
        public List<int> arregloDieciocho = new List<int>();
        public List<int> arregloDiecinueve = new List<int>();
        public List<int> arregloVeinte = new List<int>();

        public List<int> arregloLunes = new List<int>();
        public List<int> arregloMartes = new List<int>();
        public List<int> arregloMiercoles = new List<int>();
        public List<int> arregloJueves = new List<int>();
        public List<int> arregloViernes = new List<int>();
        public string colorBase = "#fcf4ca";
        #endregion
        private HorarioDAO horarioDAO = new HorarioDAO();
        private static Grupo grupo = new Grupo();        
        private Dictionary<string, int> materiasSesiones = new Dictionary<string, int>();
        private List<ProfesorMateria> listaProfesorMateria = new List<ProfesorMateria>();

        public GenerarHorarioRegistro()
        {
            InitializeComponent();
            //SetListaProfesorMateria();
            SetGrupo();
            SetRangos();
            SetTableroHorario();
            //SetMateriasDisponibles();
            SetMateriasDisponiblesProvicional();


        }

        #region Tabla Horario
        private void Salir(object sender, MouseButtonEventArgs e)
        {
            GenerarHorario generarHorario = new GenerarHorario();
            Application.Current.MainWindow = generarHorario;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<GenerarHorarioRegistro>())
                ((GenerarHorarioRegistro)window).Close();
        }

        public void SetRangos()
        {

            SetRangosHoras(arregloSiete, 6, 11);
            SetRangosHoras(arregloOcho, 12, 17);
            SetRangosHoras(arregloNueve, 18, 23);
            SetRangosHoras(arregloDiez, 24, 29);
            SetRangosHoras(arregloOnce, 30, 35);
            SetRangosHoras(arregloDoce, 36, 41);
            SetRangosHoras(arregloTrece, 42, 47);
            SetRangosHoras(arregloCatorce, 48, 53);
            SetRangosHoras(arregloQuince, 54, 59);
            SetRangosHoras(arregloDieciseis, 60, 65);
            SetRangosHoras(arregloDiecisiete, 66, 71);
            SetRangosHoras(arregloDieciocho, 72, 77);
            SetRangosHoras(arregloDiecinueve, 78, 83);
            SetRangosHoras(arregloVeinte, 84, 89);

            int cantidadHoras = 14;
            int indexLimiteIzquierdo = 6;

            for (int i = 1; i <= cantidadHoras; i++)
            {
                int indexLunes = (indexLimiteIzquierdo * i) + 1;
                arregloLunes.Add(indexLunes);
            }

            for (int i = 1; i <= cantidadHoras; i++)
            {
                int indexMartes = (indexLimiteIzquierdo * i) + 2;
                arregloMartes.Add(indexMartes);
            }

            for (int i = 1; i <= cantidadHoras; i++)
            {
                int indexMiercoles = (indexLimiteIzquierdo * i) + 3;
                arregloMiercoles.Add(indexMiercoles);
            }

            for (int i = 1; i <= cantidadHoras; i++)
            {
                int indexJueves = (indexLimiteIzquierdo * i) + 4;
                arregloJueves.Add(indexJueves);
            }

            for (int i = 1; i <= cantidadHoras; i++)
            {
                int indexViernes = (indexLimiteIzquierdo * i) + 5;
                arregloViernes.Add(indexViernes);
            }
        }

        public void SetRangosHoras(List<int> arreglo, int limiteInferior, int limiteSuperior)
        {
            for (int i = limiteInferior; i <= limiteSuperior; i++)
            {
                arreglo.Add(i);
            }
        }

        public void SetTableroHorario()
        {
            int cantidadHoras = 14 + 1;
            int cantidadDias = 5 + 1;
            int tamañoHorario = (cantidadHoras * cantidadDias) + 1;

            for (int i = 0; i < tamañoHorario; i++)
            {
                Border border = CrearElemento("-", colorBase, false);
                InsertarCampo(border, i, listBoxGenerarHorario);

            }

            Border borderPrimeraPosicion = CrearElemento("", "#fafafa", false);
            SetCampo(borderPrimeraPosicion, 0, listBoxGenerarHorario);

            CargarDias();
            CargarHoras();
        }

        public void InsertarCampo(Border border, int posicion, ListBox lista)
        {            
            lista.Items.Insert(posicion, border);            
        }

        public void InsertarMateria(StackPanel stackPanel, int posicion, ListBox lista)
        {
            lista.Items.Insert(posicion, stackPanel);
        }

        public void SetCampo(Border border, int posicion, ListBox lista)
        {            
            lista.Items[posicion] = border;            
        }

        public void CargarDias()
        {

            Border borderLunes = CrearElemento("Lunes", "#FFD6EF", false);
            Border borderMartes = CrearElemento("Martes", "#FFD6EF", false);
            Border borderMiercoles = CrearElemento("Miercoles", "#FFD6EF", false);
            Border borderJueves = CrearElemento("Jueves", "#FFD6EF", false);
            Border borderViernes = CrearElemento("Viernes", "#FFD6EF", false);

            SetCampo(borderLunes, 1, listBoxGenerarHorario);
            SetCampo(borderMartes, 2, listBoxGenerarHorario);
            SetCampo(borderMiercoles, 3, listBoxGenerarHorario);
            SetCampo(borderJueves, 4, listBoxGenerarHorario);
            SetCampo(borderViernes, 5, listBoxGenerarHorario);
        }

        public void CargarHoras()
        {
            int numeroDeHoras = 13 + 1;
            int horaInicio = 7;
            int horaFin = 8;
            int indexPosicionHorario = 1;


            for (int i = 0; i < numeroDeHoras; i++)
            {
                int posicion = indexPosicionHorario * 6;
                indexPosicionHorario += 1;
                string contenido = (horaInicio + i) + " - " + (horaFin + i);
                Border border = CrearElemento(contenido, "#BCEEF2", false);
                SetCampo(border, posicion, listBoxGenerarHorario);
            }
        }

        public Border CrearElemento(string contenido, string background, bool esMateria)
        {

            TextBlock textBlock = new TextBlock();
            var bc = new BrushConverter();
            textBlock.Foreground = Brushes.Black;
            textBlock.Text = contenido;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;

            Border border = new Border();
            if (!esMateria)
            {
                
                border.Padding = new Thickness(0, 10, 0, 10);
                border.Margin = new Thickness(0);
                border.Background = (Brush)bc.ConvertFrom(background);
                border.CornerRadius = new CornerRadius(10);
                border.Width = 60;
                border.Child = textBlock;
            }
            else
            {                
                border.Padding = new Thickness(0, 20, 0, 20);
                border.Margin = new Thickness(0);
                border.Background = (Brush)bc.ConvertFrom(background);
                border.CornerRadius = new CornerRadius(10);
                border.Width = 200;                
                border.Child = textBlock;                        
            }
          
            return border;
        }
        
        public StackPanel CrearElementoMateria(Materia materia)
        {
            string contenido = "N° Sesiones: " + materia.NumSesiones + "\n" + materia.Nombre;
            StackPanel stackPanel = new StackPanel();
            Border newBorder = CrearElemento(materia.Nombre, materia.Color, true);
            TextBlock textBlock = new TextBlock();
            
            textBlock.Foreground = Brushes.Black;
            textBlock.Text = contenido;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontWeight = FontWeights.Bold;

            newBorder.Child = textBlock;
            ComboBox comboBox = new ComboBox();
            comboBox.Width = 200;

            stackPanel.Children.Add(newBorder);
            stackPanel.Children.Add(comboBox);

            return stackPanel;
        }

        #endregion

        public void SetGrupo()
        {
            GenerarHorario generarHorario = new GenerarHorario();
            grupo = generarHorario.GetGrupo();
        }

        public void SetListaProfesorMateria()
        {
            GenerarHorarioRegistroProfesores generarHorarioRegistroProfesores = new GenerarHorarioRegistroProfesores();
            listaProfesorMateria = generarHorarioRegistroProfesores.GetListaProfesorMateriaFinal();            
        }

        public void SetMateriasDisponibles()
        {
            //listaProfesorMateria
            //Materia : Mate_2-Matemáticas II
            //Profesor : profesor1-Angel

            int indexMateria = 0;
            foreach (ProfesorMateria profesorMateria in listaProfesorMateria)
            {                

                string[] materiaInformacion = profesorMateria.Materia.Split('-');
                string nrc = materiaInformacion[0];
                string nombre = materiaInformacion[1];                

                string[] profesorInformacion = profesorMateria.Profesor.Split('-');
                string rfc = profesorInformacion[0];

                Materia materia = horarioDAO.GetMateriaByNRC(nrc);                
                materiasSesiones.Add(nombre, materia.NumSesiones);
                string contenido = "N° Sesiones: " + materia.NumSesiones+
                                    "\n" +
                                    "NRC: " + nrc +
                                    "\n" +
                                    "RFC: " + rfc
                                    + "\n" +
                                    nombre;
                
                Border border = CrearElemento(contenido, materia.Color, true);                
                InsertarCampo(border, indexMateria, listBoxMaterias);
                indexMateria++;
            }                    
        }

        public void SetMateriasDisponiblesProvicional()
        {

            List<Materia> listaMaterias = horarioDAO.GetMateriasBySemestre(2);
            int indexMateria = 0;

            foreach (Materia materia in listaMaterias)
            {

                materiasSesiones.Add(materia.Nombre, materia.NumSesiones);

                string contenido = "Sesiones: " + materia.NumSesiones + "\n" + materia.Nombre;

                Border border = CrearElemento(contenido, materia.Color, true);
                InsertarCampo(border, indexMateria, listBoxMaterias);
                indexMateria++;

                //stackPanel.Children.Clear();
            }
        }

        public void SeleccionMateria(object sender, RoutedEventArgs e)
        {

            var border = (Border)listBoxMaterias.SelectedItem;
            TextBlock textBlock = (TextBlock)border.Child;

            string contenido = textBlock.Text;
            Console.WriteLine(contenido);

            string[] materiaInformacion = contenido.Split('\n');
            string sesionesInformacion = materiaInformacion[0];

            string[] numSesionesInformacion = sesionesInformacion.Split(' ');
            int numSesiones = Int32.Parse(numSesionesInformacion[1]);

            //int numSesionesActualizadas = numSesiones--;

            Console.WriteLine(numSesiones);

        }
    }
}

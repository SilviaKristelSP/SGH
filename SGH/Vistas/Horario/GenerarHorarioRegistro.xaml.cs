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
using System.Security.Cryptography;
using SGH.Vistas.Alertas;
using SGH.Vistas.LogIn;

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
        private string contenidoSeleccionadoMateria = "";
        private int indexMateriaSeleccionadaGlobal;
        private string contenidoSeleccionadoHorario = "";        
        private string colorSeleccionado = "";
        private Grupo grupoSolapamiento = null;

        public GenerarHorarioRegistro()
        {
            InitializeComponent();
            SetListaProfesorMateria();

            SetGrupo();
            SetRangos();
            SetTableroHorario();
            SetTableroMaterias();
            SetMateriasDisponibles();
            //SetMateriasDisponiblesProvicional();
        }

        #region Tabla Horario        

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

        public void SetTableroMaterias()
        {
            int cantidadFilas = 15;
            int cantidadColumnas = 2;
            int tamañoTablero = cantidadFilas * cantidadColumnas;

            for (int i = 0; i < tamañoTablero; i++)
            {
                Border border = CrearElemento("-", "#fff", true);
                InsertarCampo(border, i, listBoxMaterias);
            }            
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
                border.Width = 95;
                border.Child = textBlock;
            }
            else
            {                                                
                textBlock.Width = 150;
                textBlock.TextWrapping = TextWrapping.Wrap;

                border.Padding = new Thickness(0, 15, 0, 15);
                border.Margin = new Thickness(0);
                border.Background = (Brush)bc.ConvertFrom(background);
                border.CornerRadius = new CornerRadius(10);
                border.Width = 200;                
                border.Child = textBlock;                        
            }
          
            return border;
        }

        public Border CrearElementoHorario(string contenido, string background)
        {
            TextBlock textBlock = new TextBlock();
            var bc = new BrushConverter();
            textBlock.Foreground = Brushes.Black;
            textBlock.Text = contenido;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontSize = 8;
            textBlock.Width = 65;
            textBlock.TextWrapping = TextWrapping.Wrap;

            Border border = new Border();            
            border.Padding = new Thickness(0, 10, 0, 10);
            border.Margin = new Thickness(0);
            border.Background = (Brush)bc.ConvertFrom(background);
            border.CornerRadius = new CornerRadius(10);
            border.Width = 95;
            border.Child = textBlock;
                      

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
                string contenido = "Sesiones: " + materia.NumSesiones+
                                    "\n" +
                                    "NRC: " + nrc +
                                    "\n" +
                                    "RFC: " + rfc
                                    + "\n" +
                                    nombre;
                
                Border border = CrearElemento(contenido, materia.Color, true);
                //InsertarCampo(border, indexMateria, listBoxMaterias);
                SetCampo(border, indexMateria, listBoxMaterias);
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
            }
        }

        public void EliminarCampo(int posicion, string color, ListBox listBox)
        {
            Border border = CrearElemento("-", color, false);
            if (border != null) {
                SetCampo(border, posicion, listBox);
            }
            
        }

        public void SeleccionMateria(object sender, RoutedEventArgs e)
        {

            int indexMateriaSeleccionada = listBoxMaterias.SelectedIndex;            
            var border = (Border)listBoxMaterias.SelectedItem;

            if (border != null && contenidoSeleccionadoHorario.Length == 0)
            {
                TextBlock textBlock = (TextBlock)border.Child;
                string contenido = textBlock.Text.ToString();
                
                if (!contenido.Equals("-"))
                {
                    
                    string[] materiaInformacion = contenido.Split('\n');
                    string sesionesInformacion = materiaInformacion[0];
                    int numSesiones = GetNumSesiones(sesionesInformacion);                    
                    string nrc = GetField(materiaInformacion[1]);
                    string rfc = GetField(materiaInformacion[2]);


                    string materia = materiaInformacion[3];

                    if (numSesiones > 0)
                    {                        
                        string contenidoNuevo = "Sesiones: " + materiasSesiones[materia] +
                                    "\n" +
                                    "NRC: " + nrc +
                                    "\n" +
                                    "RFC: " + rfc
                                    + "\n" +
                                    materia;


                        contenidoSeleccionadoMateria = contenido;
                        colorSeleccionado = border.Background.ToString();
                        indexMateriaSeleccionadaGlobal = indexMateriaSeleccionada;

                        Border borderNuevo = CrearElemento(contenidoNuevo, border.Background.ToString(), true);
                        SetCampo(borderNuevo, indexMateriaSeleccionada, listBoxMaterias);
                    }                                                         
                }
            }            
        }
        
        public void SeleccionHorario(object sender, RoutedEventArgs e)
        {
            int indexHorarioSeleccionada = listBoxGenerarHorario.SelectedIndex;
            string hora = GetHora(indexHorarioSeleccionada);
            string dia = GetDia(indexHorarioSeleccionada);            
            var border = (Border)listBoxGenerarHorario.SelectedItem;

            if (border != null)
            {
                if (!hora.Equals("") && !dia.Equals(""))
                {                
                    TextBlock textBlock = (TextBlock)border.Child;
                    string contenido = textBlock.Text.ToString();                             
                    bool campoVacio = contenido.Equals("-");
                
                    if (!campoVacio)
                    {
                        if (contenidoSeleccionadoMateria.Length == 0 && contenidoSeleccionadoHorario.Length == 0)
                        {
                            contenidoSeleccionadoHorario = contenido;
                            colorSeleccionado = border.Background.ToString();
                            EliminarCampo(indexHorarioSeleccionada, colorBase, listBoxGenerarHorario);
                        }
                    }
                    else
                    {
                        if (contenidoSeleccionadoMateria.Length > 0 && contenidoSeleccionadoHorario.Length == 0)
                        {
                            
                            string[] materiaInformacion = contenidoSeleccionadoMateria.Split('\n');
                            string materia = materiaInformacion[3];

                            int numSesiones = materiasSesiones[materia];
                            int numSesionesActualizadas = numSesiones - 1;
                            materiasSesiones[materia] = numSesionesActualizadas;                           
                            string nrc = GetField(materiaInformacion[1]);
                            string rfc = GetField(materiaInformacion[2]);

                            string contenidoNuevoMateria = "Sesiones: " + materiasSesiones[materia] +
                                   "\n" +
                                   "NRC: " + nrc +
                                   "\n" +
                                   "RFC: " + rfc
                                   + "\n" +
                                   materia;

                            Border borderNuevoMateria = CrearElemento(contenidoNuevoMateria, colorSeleccionado, true);
                            SetCampo(borderNuevoMateria, indexMateriaSeleccionadaGlobal, listBoxMaterias);                                           
                            string contenidoNuevo =
                                   "NRC: " + nrc +
                                   "\n" +
                                   "RFC: " + rfc
                                   + "\n" +
                                   materia;

                            Border borderNuevo = CrearElementoHorario(contenidoNuevo, colorSeleccionado);
                            SetCampo(borderNuevo, indexHorarioSeleccionada, listBoxGenerarHorario);
                            contenidoSeleccionadoMateria = "";
                            colorSeleccionado = "";
                                                        
                        }
                        else if (contenidoSeleccionadoHorario.Length > 0)
                        {
                            string[] materiaInformacion = contenidoSeleccionadoHorario.Split('\n');                                                        
                            string nrc = GetField(materiaInformacion[0]);
                            string rfc = GetField(materiaInformacion[1]);
                            string materia = materiaInformacion[2];


                            string contenidoNuevo =
                                   "NRC: " + nrc +
                                   "\n" +
                                   "RFC: " + rfc
                                   + "\n" +
                                   materia;



                            Border borderNuevo = CrearElementoHorario(contenidoNuevo, colorSeleccionado);
                            SetCampo(borderNuevo, indexHorarioSeleccionada, listBoxGenerarHorario);
                            contenidoSeleccionadoHorario = "";
                            colorSeleccionado = "";
                        }
                    }                  
                }
            }           
        }

        public int GetNumSesiones(string sesionesInformacion)
        {
            string[] numSesionesInformacion = sesionesInformacion.Split(' ');
            int numSesiones = Int32.Parse(numSesionesInformacion[1]);

            return numSesiones;
        }        

        public string GetField(string information)
        {
            string[] arrayInformation = information.Split(' ');
            return arrayInformation[1];
        }

        public string GetHora(int posicion)
        {

            string hora = "";
            int mitadNivelUno = 41;
            int mitadSuperiorNivelDos = 17;
            int mitadSuperiorInferiorNivelTres = 29;
            int mitadInferiorNivelDos = 65;
            int mitadInferiorSuperiorNivelTres = 53;
            int mitadInferiorInferiorNivelTres = 77;


            if (posicion <= mitadNivelUno)
            {
                if (posicion <= mitadSuperiorNivelDos)
                {
                    if (arregloSiete.Contains(posicion))
                    {
                        hora = "7-8";
                    }
                    else if (arregloOcho.Contains(posicion))
                    {
                        hora = "8-9";
                    }
                }
                else
                {
                    if (posicion <= mitadSuperiorInferiorNivelTres)
                    {
                        if (arregloNueve.Contains(posicion))
                        {
                            hora = "9-10";
                        }
                        else
                        {
                            hora = "10-11";
                        }
                    }
                    else if (arregloOnce.Contains(posicion))
                    {
                        hora = "11-12";
                    }
                    else
                    {
                        hora = "12-13";
                    }

                }
            }
            else
            {
                if (posicion <= mitadInferiorNivelDos)
                {
                    if (posicion <= mitadInferiorSuperiorNivelTres)
                    {
                        if (arregloTrece.Contains(posicion))
                        {
                            hora = "13-14";
                        }
                        else
                        {
                            hora = "14-15";
                        }
                    }
                    else if (arregloQuince.Contains(posicion))
                    {
                        hora = "15-16";
                    }
                    else
                    {
                        hora = "16-17";
                    }

                }
                else
                {
                    if (posicion <= mitadInferiorInferiorNivelTres)
                    {
                        if (arregloDiecisiete.Contains(posicion))
                        {
                            hora = "17-18";
                        }
                        else
                        {
                            hora = "18-19";
                        }
                    }
                    else if (arregloDiecinueve.Contains(posicion))
                    {
                        hora = "19-20";
                    }
                    else
                    {
                        hora = "20-21";
                    }

                }

            }

            return hora;
        }

        public string GetDia(int posicion)
        {
            string dia = "";

            if (arregloLunes.Contains(posicion))
            {
                dia = "lunes";
            }
            else if (arregloMartes.Contains(posicion))
            {
                dia = "martes";
            }
            else if (arregloMiercoles.Contains(posicion))
            {
                dia = "miercoles";
            }
            else if (arregloJueves.Contains(posicion))
            {
                dia = "jueves";
            }
            else if (arregloViernes.Contains(posicion))
            {
                dia = "viernes";
            }

            return dia;
        }

        private void ValidarHorario(object sender, RoutedEventArgs e)
        {
            bool solapamientoHorarioLunes;
            bool solapamientoHorarioMartes;
            bool solapamientoHorarioMiercoles;
            bool solapamientoHorarioJueves;
            bool solapamientoHorarioViernes;
            List<bool> listaSolapamiento = new List<bool>();

            solapamientoHorarioLunes = ValidarSesionesPorDia(arregloLunes);
            solapamientoHorarioMartes = ValidarSesionesPorDia(arregloMartes);
            solapamientoHorarioMiercoles = ValidarSesionesPorDia(arregloMiercoles);
            solapamientoHorarioJueves = ValidarSesionesPorDia(arregloJueves);
            solapamientoHorarioViernes = ValidarSesionesPorDia(arregloViernes);

            listaSolapamiento.Add(solapamientoHorarioLunes);
            listaSolapamiento.Add(solapamientoHorarioMartes);
            listaSolapamiento.Add(solapamientoHorarioMiercoles);
            listaSolapamiento.Add(solapamientoHorarioJueves);
            listaSolapamiento.Add(solapamientoHorarioViernes);
           
            bool horarioSolapado = listaSolapamiento.Any(x => x == true);            

            if (horarioSolapado && grupoSolapamiento != null)
            {                
                string mensajeAlerta = "Solapamiento con grupo: " + grupoSolapamiento.Semestre + "-" + grupoSolapamiento.Letra;                
                MostrarAlertaShortOk(mensajeAlerta, MessageType.Warning);
            }
            else
            {
                bool sesionesCubiertas = ValidarCoberturaSesiones();
                if (sesionesCubiertas)
                {
                    GuardarHorario();
                }
                else
                {
                    MostrarAlertaShortOk("Faltan sesiones por asignar", MessageType.Warning);
                }
            }
            grupoSolapamiento = null;
        }

        public bool ValidarCoberturaSesiones()
        {
            bool sesionesCubiertas = true;
            foreach (KeyValuePair<string, int> entry in materiasSesiones)
            {                
                if (entry.Value > 0)
                {
                    sesionesCubiertas = false;
                    break;
                }
            }

            return sesionesCubiertas;
        }

        public void GuardarHorario()
        {            
            bool sesionesGuardadasLunes;
            bool sesionesGuardadasMartes;
            bool sesionesGuardadasMiercoles;
            bool sesionesGuardadasJueves;
            bool sesionesGuardadasViernes;
            List<bool> listaSesionesGuardadas = new List<bool>();

            sesionesGuardadasLunes = GuardarSesionesPorDia(arregloLunes);
            sesionesGuardadasMartes = GuardarSesionesPorDia(arregloMartes);
            sesionesGuardadasMiercoles = GuardarSesionesPorDia(arregloMiercoles);
            sesionesGuardadasJueves = GuardarSesionesPorDia(arregloJueves);
            sesionesGuardadasViernes = GuardarSesionesPorDia(arregloViernes);

            listaSesionesGuardadas.Add(sesionesGuardadasLunes);
            listaSesionesGuardadas.Add(sesionesGuardadasMartes);
            listaSesionesGuardadas.Add(sesionesGuardadasMiercoles);
            listaSesionesGuardadas.Add(sesionesGuardadasJueves);
            listaSesionesGuardadas.Add(sesionesGuardadasViernes);

            bool horarioGuardado = listaSesionesGuardadas.Any(x => x == false);

            if (horarioGuardado)
            {
                MostrarAlertaShortOk("Error de conexión con la base de datos", MessageType.Warning);
                Console.WriteLine("Entre aqui 1");
            }
            else
            {
                MostrarAlertaShortOk("El horario ha sido guardado exitosamente", MessageType.Confirmation);
                SalirGenerarHorario();

            }
        }

        public bool GuardarSesionesPorDia(List<int> arregloDia)
        {

            bool registroGuardados = true;

            foreach (int indexDia in arregloDia)
            {
                var border = (Border)listBoxGenerarHorario.Items[indexDia];

                if (border != null)
                {
                    TextBlock textBlock = (TextBlock)border.Child;
                    string contenido = textBlock.Text.ToString();
                    bool campoVacio = contenido.Equals("-");

                    if (!campoVacio)
                    {

                        string[] materiaInformacion = contenido.Split('\n');
                        string nrc = GetField(materiaInformacion[0]);
                        string rfc = GetField(materiaInformacion[1]);                        

                        Profesor_Materia profesor_Materia = horarioDAO.GetProfesorMateria(rfc, nrc);

                        string dia = GetDia(indexDia);
                        string hora = GetHora(indexDia);
                        string[] horaInformacion = hora.Split('-');
                        string horaInicio = horaInformacion[0];
                        string horaFin = horaInformacion[1];

                        Sesion sesion = new Sesion();                        
                        sesion.DiaSemana = dia;
                        sesion.HoraFinal = horaFin;
                        sesion.HoraInicio = horaInicio;
                        sesion.ID_Grupo = grupo.ID;

                        registroGuardados = GuardarSesion(sesion, profesor_Materia);

                        if (!registroGuardados)
                        {
                            MostrarAlertaShortOk("Error de conexión con la base de datos", MessageType.Warning);
                            Console.WriteLine("Entre aqui 2");
                            break;
                        }                        
                    }
                }
            }

            return registroGuardados;
        }

        public bool GuardarSesion(Sesion sesion, Profesor_Materia profesor_Materia)
        {
            bool registroGuardados = true;
            string idSesion;
            bool existeSesion;

            do
            {
                idSesion = Utiles.Util.generarID(50);
                existeSesion = horarioDAO.EncontrarSesionPorId(idSesion);
                
            } while (existeSesion);

            sesion.ID = idSesion;
            bool sesionGuardada = horarioDAO.GuardarSesion(sesion);

            if (!sesionGuardada)
            {
                registroGuardados = false;                
            }
            else
            {

                string idMateriaSesion;
                bool existeMateriaSesion;
                do
                {
                    idMateriaSesion = Utiles.Util.generarID(50);
                    existeMateriaSesion = horarioDAO.EncontrarMateriaSesionPorId(idMateriaSesion);

                } while (existeMateriaSesion);

                Materia_Sesion materiaSesion = new Materia_Sesion();
                materiaSesion.ID_Materia_Sesion = idMateriaSesion;
                materiaSesion.ID_Profesor_Materia = profesor_Materia.ID_Profesor_Materia;
                materiaSesion.ID_Sesion = idSesion;

                bool materiaSesionGuardada = horarioDAO.GuardarMateriaSesion(materiaSesion);

                if (!materiaSesionGuardada)
                {
                    registroGuardados = false;                    
                }
            }
            return registroGuardados;
        }

        public void MostrarAlertaShortOk(string mensaje, MessageType messageType)
        {
            stackPanelBlack.Visibility = Visibility.Visible;
            Alerta alerta = new Alerta(mensaje,
                                    messageType, MessageButtons.Ok, "short");
            new MessageBoxCustom(alerta).ShowDialog();
            stackPanelBlack.Visibility = Visibility.Collapsed;
        }       

        public bool ValidarSesionesPorDia(List<int> arregloDia)
        {
            bool solapamiento = false;            

            foreach (int indexDia in arregloDia)
            {
                var border = (Border)listBoxGenerarHorario.Items[indexDia];

                if (border != null)
                {
                    TextBlock textBlock = (TextBlock)border.Child;
                    string contenido = textBlock.Text.ToString();
                    bool campoVacio = contenido.Equals("-");

                    if (!campoVacio)
                    {

                        string[] materiaInformacion = contenido.Split('\n');
                        string nrc = GetField(materiaInformacion[0]);
                        string rfc = GetField(materiaInformacion[1]);
                        string materia = materiaInformacion[2];

                        Profesor_Materia profesor_Materia = horarioDAO.GetProfesorMateria(rfc, nrc);

                        string dia = GetDia(indexDia);
                        string hora = GetHora(indexDia);
                        string[] horaInformacion = hora.Split('-');
                        string horaInicio = horaInformacion[0];
                        string horaFin = horaInformacion[1];

                        Sesion sesion = new Sesion();
                        sesion.DiaSemana = dia;
                        sesion.HoraFinal = horaFin;
                        sesion.HoraInicio = horaInicio;                      

                        Sesion sesionEncontrada = horarioDAO.ValidarSesion(sesion, profesor_Materia.ID_Profesor_Materia, grupo.Semestre);
                        if (sesionEncontrada != null)
                        {
                            solapamiento = true;
                            grupoSolapamiento = horarioDAO.GetGrupoByID(sesionEncontrada.ID_Grupo);                            
                            break;
                        }                        
                    }
                }
            }            

            return solapamiento;
        }

        private void ConsultaRapidaHorario(object sender, RoutedEventArgs e)
        {            
            ConsultaRapidaHorario window = new ConsultaRapidaHorario();
            window.Owner = this;
            window.ShowDialog();
        }

        private void Salir(object sender, MouseButtonEventArgs e)
        {
            SalirGenerarHorario();
        }

        public void SalirGenerarHorario()
        {
            GenerarHorario generarHorario = new GenerarHorario();
            Application.Current.MainWindow = generarHorario;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<GenerarHorarioRegistro>())
            {
                ((GenerarHorarioRegistro)window).Close();
            }
        }
    }
}

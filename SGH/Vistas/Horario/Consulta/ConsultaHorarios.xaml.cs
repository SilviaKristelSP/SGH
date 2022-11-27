using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using static System.Net.Mime.MediaTypeNames;

namespace SGH.Vistas.Horario.Consulta
{
    /// <summary>
    /// Lógica de interacción para ConsultaHorarios.xaml
    /// </summary>
    public partial class ConsultaHorarios : Window
    {

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

        public ConsultaHorarios()
        {
            InitializeComponent();
            CargarHorario();
            SetRangos();

            SetCampo("HOLIIII", 89, "#FFD6EF");
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

            int cantidadHoras = 13;
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

        public void SeleccionElemento(object sender, RoutedEventArgs e)
        {         

            Border border = (Border)listBoxConsultaHorario.SelectedItem;
            TextBlock textBlock = (TextBlock)border.Child;

            int posicion = listBoxConsultaHorario.SelectedIndex;
            //Border borderNuevo = CrearElemento("Hola Mundo", border.Background.ToString());

            //listBoxConsultaHorario.Items[posicion] = borderNuevo;


            Console.WriteLine(GetHora(posicion));

                        
        }

        public string GetHora(int posicion)
        {

            string hora;
            int contadorPreguntas = 0;
            int mitadNivelUno = 41;
            int mitadSuperiorNivelDos = 17;            
            int mitadSuperiorInferiorNivelTres = 29;                        
            int mitadInferiorNivelDos = 65;
            int mitadInferiorSuperiorNivelTres = 53;
            int mitadInferiorInferiorNivelTres = 77;


            if (posicion <= mitadNivelUno)
            {
                contadorPreguntas++;

                if (posicion <= mitadSuperiorNivelDos)
                {
                    contadorPreguntas++;
                    if (arregloSiete.Contains(posicion))
                    {
                        contadorPreguntas++;
                        hora = "7-8";
                    }
                    else
                    {
                        contadorPreguntas++;
                        hora = "8-9";
                    }
                }
                else
                {
                    contadorPreguntas++;
                    if (posicion <= mitadSuperiorInferiorNivelTres)
                    {

                        contadorPreguntas++;
                        if (arregloNueve.Contains(posicion))
                        {
                            contadorPreguntas++;
                            hora = "9-10";
                        }
                        else
                        {
                            contadorPreguntas++;
                            hora = "10-11";
                        }
                    }
                    else
                    {
                        contadorPreguntas++;
                        if (arregloOnce.Contains(posicion))
                        {
                            contadorPreguntas++;
                            hora = "11-12";
                        }
                        else
                        {
                            contadorPreguntas++;
                            hora = "12-13";
                        }
                    }
                }                              
            }
            else
            {
                contadorPreguntas++;
                if (posicion <= mitadInferiorNivelDos)
                {
                    contadorPreguntas++;
                    if (posicion <= mitadInferiorSuperiorNivelTres)
                    {                        
                        contadorPreguntas++;
                        if (arregloTrece.Contains(posicion))
                        {
                            contadorPreguntas++;
                            hora = "13-14";
                        }
                        else
                        {
                            contadorPreguntas++;
                            hora = "14-15";
                        }
                    }
                    else
                    {
                        contadorPreguntas++;
                        if (arregloQuince.Contains(posicion))
                        {
                            contadorPreguntas++;
                            hora = "15-16";
                        }
                        else
                        {
                            contadorPreguntas++;
                            hora = "16-17";
                        }
                    }
                }
                else
                {
                    contadorPreguntas++;
                    if (posicion <= mitadInferiorInferiorNivelTres)
                    {
                        contadorPreguntas++;
                        if (arregloDiecisiete.Contains(posicion))
                        {
                            contadorPreguntas++;
                            hora = "17-18";
                        }
                        else
                        {
                            contadorPreguntas++;
                            hora = "18-19";
                        }
                    }
                    else
                    {
                        contadorPreguntas++;
                        if (arregloDiecinueve.Contains(posicion))
                        {
                            contadorPreguntas++;
                            hora = "19-20";
                        }
                        else
                        {
                            contadorPreguntas++;
                            hora = "20-21";
                        }
                    }
                }

            }

            Console.WriteLine(contadorPreguntas);
           
            return hora;
        }

        public void CargarHorario()
        {
            int cantidadHoras = 13 + 1;
            int cantidadDias = 5 + 1;
            int tamañoHorario = (cantidadHoras * cantidadDias) + 1;

            for (int i = 0; i < tamañoHorario; i++)
            {
                if (i % 2 == 0)
                {
                    SetCampo("-", i, "#FFD6EF");
                }
                else
                {
                    SetCampo("-", i, "#FAECAA");
                }
                
            }

            SetCampo("", 0, "#FFF");

            CargarDias();
            CargarHoras();

        }

        public void SetCampo(string contenido, int posicion, string background)
        {


            Border border = CrearElemento(contenido, background);

            listBoxConsultaHorario.Items.Insert(posicion, border);
        }

        public Border CrearElemento(string contenido, string background)
        {

            TextBlock textBlock = new TextBlock();
            var bc = new BrushConverter();
            textBlock.Foreground = Brushes.Black;
            textBlock.Text = contenido;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;


            Border border = new Border();
            border.Padding = new Thickness(0, 10, 0, 10);
            border.Margin = new Thickness(0);
            border.Background = (Brush)bc.ConvertFrom(background);
            border.CornerRadius = new CornerRadius(10);
            border.Width = 100;
            border.Child = textBlock;

            return border;

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
                SetCampo(contenido, posicion, "#BCEEF2");
            }            
        }

        public void CargarDias()
        {
           
            SetCampo("Lunes", 1, "#FFD6EF");
            SetCampo("Martes", 2, "#FAECAA");
            SetCampo("Miércoles", 3, "#FFD6EF");
            SetCampo("Jueves", 4, "#FAECAA");
            SetCampo("Viernes", 5, "#FFD6EF");
        }
               
    }
}

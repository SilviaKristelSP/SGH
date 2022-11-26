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

namespace SGH.Vistas.Horario.Consulta
{
    /// <summary>
    /// Lógica de interacción para ConsultaHorarios.xaml
    /// </summary>
    public partial class ConsultaHorarios : Window
    {
        public ConsultaHorarios()
        {
            InitializeComponent();
            CargarHorario();
        }

        public void CargarHorario()
        {
            int cantidadHoras = 13 + 1;
            int cantidadDias = 5 + 1;
            int tamañoHorario = (cantidadHoras * cantidadDias) + 1;

            for (int i = 0; i < tamañoHorario; i++)
            {

                SetCampo(i, "-");               
            }

            SetCampo(0, "");

            CargarDias();
            CargarHoras();




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
                SetCampoDefault(contenido, posicion);
            }
        }

        public void CargarDias()
        {
            SetCampoDefault("Lunes", 1);
            SetCampoDefault("Martes", 2);
            SetCampoDefault("Miércoles", 3);
            SetCampoDefault("Jueves", 4);
            SetCampoDefault("Viernes", 5);
        }

        public void SetCampoDefault(string contenido, int posicion)
        {
            TextBlock text = new TextBlock();
            text.Padding = new Thickness(30,2, 30, 2);
            text.Background = Brushes.Gray;
            text.Foreground = Brushes.White;
            text.Text = contenido;
            text.FontSize = 15;
            text.TextAlignment = TextAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.Width = 150;

            listBoxConsultaHorario.Items.Insert(posicion, text);
        }

        public void SetCampo(int posicion, string contenido)
        {
            TextBlock text = new TextBlock();
            text.TextAlignment = TextAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.Padding = new Thickness(30, 2, 30, 2);
            text.Text = contenido;
            text.Width = 150;

            listBoxConsultaHorario.Items.Insert(posicion, text);
        }
    }
}

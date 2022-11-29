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

namespace SGH.Vistas.Horario
{
    /// <summary>
    /// Lógica de interacción para GenerarHorarioRegistro.xaml
    /// </summary>
    public partial class GenerarHorarioRegistro : Window
    {
        public GenerarHorarioRegistro()
        {
            InitializeComponent();
        }

        private void Salir(object sender, MouseButtonEventArgs e)
        {
            GenerarHorario generarHorario = new GenerarHorario();
            Application.Current.MainWindow = generarHorario;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<GenerarHorarioRegistro>())
                ((GenerarHorarioRegistro)window).Close();
        }
    }
}

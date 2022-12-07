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

namespace SGH.Calificaciones
{
    /// <summary>
    /// Interaction logic for CalificacionesEstudiante.xaml
    /// </summary>
    public partial class CalificacionesEstudiante : Window
    {
        public CalificacionesEstudiante()
        {
            InitializeComponent();
        }

        private void ClickRegresar(object sender, RoutedEventArgs e)
        {
            BuscadorEstudiante ventana = new BuscadorEstudiante();
            this.Close();
            ventana.Show();
        }

        private void ClickGuardar(object sender, RoutedEventArgs e)
        {
            Console.Write("Click en guardar.");
        }
    }
}

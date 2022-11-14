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
    /// Interaction logic for SelectorCalificaciones.xaml
    /// </summary>
    public partial class SelectorCalificaciones : Window
    {
        public SelectorCalificaciones()
        {
            InitializeComponent();
        }

        private void calificacionEstudiante_Click(object sender, RoutedEventArgs e)
        {
            BuscadorEstudiante ventana = new BuscadorEstudiante();
            this.Close();
            ventana.Show();
        }

        private void calificacionGrupal_Click(object sender, RoutedEventArgs e)
        {
            CalificacionesGrupal ventana = new CalificacionesGrupal();
            this.Close();
            ventana.Show();
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ventana = new MainWindow();
            this.Close();
            ventana.Show();
        }
    }
}

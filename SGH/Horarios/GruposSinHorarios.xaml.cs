using SGH.POCO;
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

namespace SGH.Horarios
{
    /// <summary>
    /// Interaction logic for GruposSinHorarios.xaml
    /// </summary>
    public partial class GruposSinHorarios : Window
    {
        public GruposSinHorarios()
        {
            InitializeComponent();
            CargarGruposSinHorario();
        }

        private void CargarGruposSinHorario()
        {
            List<GrupoSinHorario> gruposSinHorario = new List<GrupoSinHorario>();
            gruposSinHorario.Add(new GrupoSinHorario("5to", "A"));
            dg_GruposSinHorario.ItemsSource = gruposSinHorario;
        }

        private void clickRegresar(object sender, RoutedEventArgs e)
        {
            MainWindow ventana = new MainWindow();
            this.Close();
            ventana.Show();
        }

        private void clickGenerarHorario(object sender, RoutedEventArgs e)
        {

        }
    }
}

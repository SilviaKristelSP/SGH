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
    /// Lógica de interacción para GeneracionHorarios.xaml
    /// </summary>
    public partial class GenerarHorario : Window
    {
        public GenerarHorario()
        {
            InitializeComponent();
        }

        public void SeleccionGrupoSinHorario(object sender, SelectionChangedEventArgs e)
        {

            TextBlock comboItem = (TextBlock)gruposSinHorarioComboBox.SelectedItem;
            if (comboItem != null)
            {
                Console.WriteLine("Sin seleccion");
            }
            else
            {
                Console.WriteLine("Sin seleccion");
            }
        }
    }
}

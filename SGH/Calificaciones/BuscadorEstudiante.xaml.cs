﻿using System;
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
    /// Interaction logic for BuscadorEstudiante.xaml
    /// </summary>
    public partial class BuscadorEstudiante : Window
    {
        public BuscadorEstudiante()
        {
            InitializeComponent();
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            SelectorCalificaciones ventana = new SelectorCalificaciones();
            this.Close();
            ventana.Show();
        }
    }
}

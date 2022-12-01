using SGH.DAOs;
using SGH.Modelos;
using SGH.Vistas.Horario.Consulta;
using SGH.Vistas.LogIn;
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
    /// Lógica de interacción para GeneracionHorarios.xaml
    /// </summary>
    public partial class GenerarHorario : Window
    {
        private static Grupo grupoSinHorario = new Grupo();
        private HorarioDAO horarioDAO = new HorarioDAO();

        public GenerarHorario()
        {
            InitializeComponent();
            CargarGrupos();
        }

        public void CargarGrupos()
        {
            List<Grupo> grupos = horarioDAO.GetGruposSinHorario();

            if (grupos.Count() > 0)
            {
                foreach (var item in grupos)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = item.Semestre + "-" + item.Letra;
                    textBlock.FontSize = 20;
                    gruposSinHorarioComboBox.Items.Add(textBlock);
                }
            }
        }        

        private void ContinuarGeneracionHorario(object sender, RoutedEventArgs e)
        {

            TextBlock comboItem = (TextBlock)gruposSinHorarioComboBox.SelectedItem;
                       
            if (comboItem != null)
            {
                string[] grupoInformacion = comboItem.Text.Split('-');
                int semestre = Int32.Parse(grupoInformacion[0]);
                string letra = grupoInformacion[1];

                Grupo grupo = horarioDAO.GetGrupo(letra, semestre);
                SetGrupo(grupo);
                GenerarHorarioRegistroProfesores gui = new GenerarHorarioRegistroProfesores();
                Application.Current.MainWindow = gui;
                Application.Current.MainWindow.Show();

                foreach (Window window in Application.Current.Windows.OfType<GenerarHorario>())
                {
                    ((GenerarHorario)window).Close();
                }
            }
            else
            {
                Console.WriteLine("Sin seleccion");
            }
        }

        public void SetGrupo(Grupo grupo)
        {
            grupoSinHorario.ID = grupo.ID;
            grupoSinHorario.Semestre = grupo.Semestre;
            grupoSinHorario.Letra = grupo.Letra;            
        }

        public Grupo GetGrupo()
        {
            return grupoSinHorario;
        }

        private void Salir(object sender, MouseButtonEventArgs e)
        {
            MenuPrincipalSGH menuPrincipalSGH = new MenuPrincipalSGH();
            Application.Current.MainWindow = menuPrincipalSGH;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<GenerarHorario>())
                ((GenerarHorario)window).Close();
        }

        private void GruposSinHorarioComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock comboItem = (TextBlock)gruposSinHorarioComboBox.SelectedItem;
            if (comboItem != null)
            {
                botonContinuarGeneracion.IsEnabled = true;
            }
        }
    }
}

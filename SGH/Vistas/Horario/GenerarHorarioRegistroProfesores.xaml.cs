using SGH.DAOs;
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
using SGH.Modelos;

namespace SGH.Vistas.Horario
{
    /// <summary>
    /// Lógica de interacción para GenerarHorarioRegistroProfesores.xaml
    /// </summary>
    public partial class GenerarHorarioRegistroProfesores : Window
    {
        HorarioDAO horarioDAO = new HorarioDAO();
        List<Profesor> listaProfesoresComboBox = new List<Profesor>();

        public GenerarHorarioRegistroProfesores()
        {
            InitializeComponent();
            //CargarMateriasProfes();
            CargarMaterias();
        }

        private void Salir(object sender, MouseButtonEventArgs e)
        {
            GenerarHorario gui = new GenerarHorario();
            Application.Current.MainWindow = gui;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<GenerarHorarioRegistroProfesores>())
                ((GenerarHorarioRegistroProfesores)window).Close();
        }

        public void CargarMateriasProfes()
        {            
            List<Materia> listaMateriasBySemestre = horarioDAO.GetMateriasBySemestre(2);

            foreach (Materia materia in listaMateriasBySemestre)
            {
                Console.WriteLine("\n");
                Console.WriteLine(materia.Nombre);
                List<Profesor> listaProfesores = horarioDAO.GetProfesoresByMateria(materia.NRC);
                foreach (Profesor profesor in listaProfesores)
                {
                    Console.WriteLine(profesor.RFC);
                }

            }

        }

        public void CargarMaterias()
        {            
            List<Materia> listaMateriasConProfesorAsignado = horarioDAO.GetMateriasConProfesorAsignado();
            List<Materia> listaMateriasBySemestre = (from mat in listaMateriasConProfesorAsignado
                                                    where mat.Semestre == 2
                                                    select mat).ToList();

            if (listaMateriasBySemestre.Count > 0)
            {
                foreach (Materia materia in listaMateriasBySemestre)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = materia.Nombre + "-" + materia.Semestre;
                    textBlock.FontSize = 20;
                    materiasComboBox.Items.Add(textBlock);
                }
                
            }

            

        }
    }
}

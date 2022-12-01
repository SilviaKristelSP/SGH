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
using System.Text.RegularExpressions;
using System.Windows.Media.Media3D;

namespace SGH.Vistas.Horario
{
    /// <summary>
    /// Lógica de interacción para GenerarHorarioRegistroProfesores.xaml
    /// </summary>
    public partial class GenerarHorarioRegistroProfesores : Window
    {
        HorarioDAO horarioDAO = new HorarioDAO();
        List<Profesor> listaProfesoresComboBox = new List<Profesor>();
        private static Grupo grupo = new Grupo();        

        public GenerarHorarioRegistroProfesores()
        {
            InitializeComponent();
            SetGrupo();
            CargarMaterias();
        }
        public void SetGrupo()
        {
            GenerarHorario generarHorario = new GenerarHorario();
            grupo = generarHorario.GetGrupo();
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
            if (listaMateriasConProfesorAsignado.Count > 0)
            {
                List<Materia> listaMateriasBySemestre = (from mat in listaMateriasConProfesorAsignado
                                                         where mat.Semestre == grupo.Semestre
                                                         select mat).Distinct().ToList();
                if (listaMateriasBySemestre.Count > 0)
                {
                    foreach (Materia materia in listaMateriasBySemestre)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = materia.NRC + "-" + materia.Nombre;
                        textBlock.FontSize = 15;
                        materiasComboBox.Items.Add(textBlock);
                    }
                }
            }                        
        }

        private void MateriasComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            profesoresComboBox.Items.Clear();
            VerificarSeleccionComboBox();

            TextBlock comboMateriasItem = (TextBlock)materiasComboBox.SelectedItem;            
            string[] materiaInformacion = comboMateriasItem.Text.Split('-');
            string nrc = materiaInformacion[0];
            string nombre = materiaInformacion[1];
            List<Profesor> profesoresDisponibles = horarioDAO.GetProfesoresByMateria(nrc);

            if (profesoresDisponibles.Count > 0)
            {
                foreach (Profesor profesor in profesoresDisponibles)
                {
                    Persona persona = horarioDAO.GetPersonaByID(profesor.ID_Persona);
                    if (persona != null)
                    {
                        
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = profesor.RFC + "-" + persona.Nombre;
                        textBlock.FontSize = 15;
                        profesoresComboBox.Items.Add(textBlock);
                    }                    
                }
            }            
        }

        private void ProfesoresComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            VerificarSeleccionComboBox();

        }

        public void VerificarSeleccionComboBox()
        {
            TextBlock comboItemMateria = (TextBlock)materiasComboBox.SelectedItem;
            TextBlock comboItemProfesor = (TextBlock)profesoresComboBox.SelectedItem;            

            if (comboItemMateria != null && comboItemProfesor != null)
            {                
                botonAsignacionProfesor.IsEnabled = true;
            }
            else
            {
                botonAsignacionProfesor.IsEnabled = false;
            }
        }

        private void AsinarProfesor(object sender, RoutedEventArgs e)
        {

        }

        public class ProfesorMateria
        {
            public string Materia { get; set; }
            public string Profesor { get; set; }            
        }

    }
}

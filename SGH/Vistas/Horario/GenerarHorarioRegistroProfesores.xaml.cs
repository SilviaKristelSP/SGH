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
using SGH.Vistas.Alertas;
using static SGH.Vistas.Horario.GenerarHorarioRegistroProfesores;

namespace SGH.Vistas.Horario
{
    /// <summary>
    /// Lógica de interacción para GenerarHorarioRegistroProfesores.xaml
    /// </summary>
    public partial class GenerarHorarioRegistroProfesores : Window
    {
        private HorarioDAO horarioDAO = new HorarioDAO();
        private List<Profesor> listaProfesoresComboBox = new List<Profesor>();
        private List<ProfesorMateria> listaProfesorMateria = new List<ProfesorMateria>();
        public static List<ProfesorMateria> listaProfesorMateriaFinal = new List<ProfesorMateria>();
        private List<Materia> listaMateriasBySemestre = new List<Materia>();
        private static Grupo grupo = new Grupo();        

        public GenerarHorarioRegistroProfesores()
        {
            InitializeComponent();
            SetGrupo();
            CargarMaterias();
        }

        public void SetListaProfesorMateriaFinal(List<ProfesorMateria> listaProfesorMateria)
        {
            listaProfesorMateriaFinal = listaProfesorMateria;
        }

        public List<ProfesorMateria> GetListaProfesorMateriaFinal()
        {
            return listaProfesorMateriaFinal;
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

        public void CargarMaterias()
        {            
            List<Materia> listaMateriasConProfesorAsignado = horarioDAO.GetMateriasConProfesorAsignado();
            if (listaMateriasConProfesorAsignado.Count > 0)
            {
                listaMateriasBySemestre = (from mat in listaMateriasConProfesorAsignado
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
                        
            TextBlock comboItemMateria = (TextBlock)materiasComboBox.SelectedItem;
            TextBlock comboItemProfesor = (TextBlock)profesoresComboBox.SelectedItem;

            ProfesorMateria profesorMateria = new ProfesorMateria
            {
                Materia = comboItemMateria.Text,
                Profesor = comboItemProfesor.Text
            };

            ProfesorMateria existe = (from pm in listaProfesorMateria
                           where pm.Materia.Equals(profesorMateria.Materia)
                           select pm).FirstOrDefault();    

            if (existe == null)
            {
                listaProfesorMateria.Add(profesorMateria);
                listBoxProfesorMateria.Items.Add(profesorMateria);                
            }
            else
            {                
                MostrarAlertaShortOk("Ya se encuentra asignada esa materia");
            }
        }

        private void ClickBotonBorrarAsignacion(object sender, RoutedEventArgs e)
        {

            if (listBoxProfesorMateria.SelectedItem != null)
            {
                ProfesorMateria profesorMateria = (ProfesorMateria)listBoxProfesorMateria.SelectedItem;
                listaProfesorMateria.Remove(profesorMateria);
                listBoxProfesorMateria.Items.Remove(profesorMateria);                
            }
            else
            {
                MostrarAlertaShortOk("Ningún elemento seleccionado");                
            }
        }

        public void MostrarAlertaShortOk(string mensaje)
        {
            stackPanelBlack.Visibility = Visibility.Visible;
            Alerta alerta = new Alerta(mensaje,
                                    MessageType.Warning, MessageButtons.Ok, "short");
            new MessageBoxCustom(alerta).ShowDialog();
            stackPanelBlack.Visibility = Visibility.Collapsed;
        }
       
        private void ClickBotonContinuarHorario(object sender, RoutedEventArgs e)
        {

            bool materiasAsignadas = true;

            foreach (Materia materia in listaMateriasBySemestre)
            {
                string materiaInformacion = materia.NRC + "-" + materia.Nombre;                
                ProfesorMateria materiaAsignada = listaProfesorMateria.Where(pm => pm.Materia.Equals(materiaInformacion)).FirstOrDefault();

                if (materiaAsignada == null)
                {                    
                    materiasAsignadas = false;
                    break;
                }                
            }

            if (materiasAsignadas)
            {
                
                SetListaProfesorMateriaFinal(listaProfesorMateria);

                GenerarHorarioRegistro gui = new GenerarHorarioRegistro();
                Application.Current.MainWindow = gui;
                Application.Current.MainWindow.Show();

                foreach (Window window in Application.Current.Windows.OfType<GenerarHorarioRegistroProfesores>())
                {
                    ((GenerarHorarioRegistroProfesores)window).Close();
                }
            }
            else
            {                
                MostrarAlertaShortOk("Aun faltan materias por asignar");
            }
        }
    }
}

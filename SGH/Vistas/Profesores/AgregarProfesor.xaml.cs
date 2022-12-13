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
using SGH.Vistas.MenuPrincipal;
using SGH.Vistas.LogIn;
using SGH.Utiles;
using Microsoft.Win32;
using System.IO;
using SGH.Vistas.Alertas;
using System.Drawing;
//using System.Drawing.Imaging;
using SGH.DAOs;

namespace SGH.Vistas.Profesores
{
    /// <summary>
    /// Lógica de interacción para AgregarProfesor.xaml
    /// </summary>
    public partial class AgregarProfesor : Window
    {
        private Persona persona = new Persona();
        private Profesor profesor = new Profesor();
        private List<Profesor_Materia> materiasAsignadas = new List<Profesor_Materia>();
        public AgregarProfesor()
        {
            InitializeComponent();
            cargarMaterias();
        }

        private void clickAgregarProfesor(object sender, RoutedEventArgs e)
        {
            String id = "";
            if (verificarFormulario())
            {
                if (comprobarTamanioCadena())
                {
                    id = Util.generarID(50);
                    configurarObjetos(id);
                    if (verificarMateriasSeleccionadas())
                    {
                        if (ProfesorDAO.agregarProfesorConMaterias(persona, profesor, materiasAsignadas))
                            mostrarVentanaExito();
                        else
                            mostrarVentanaError();
                    }
                    else
                    {
                        if (mostrarVentanaConfirmacion())
                        {
                            if (ProfesorDAO.agregarProfesorSinMaterias(persona, profesor))
                                mostrarVentanaExito();
                            else
                                mostrarVentanaError();
                        }
                    }
                }
            }
            else
            {
                mostrarVentanaFormularioVacio();
            }
        }

        private bool comprobarTamanioCadena()
        {
            if (txbRFC.Text.Length <= 13 && txbCURP.Text.Length <= 18)
                return true;
            else
            {
                lbMaxRFC.Visibility = Visibility.Visible;
                lbMaxCURP.Visibility = Visibility.Visible;

                return false;
            }            
        }

        private void configurarObjetos(String idP)
        {
            llenarObjetoPersona(idP);
            llenarObjetoProfesor(idP);
            generarProfesor_Materia();
        }
        private bool verificarFormulario()
        {
            bool formularioAprobado = true;

            if (txbNombre.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbApellidoP.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbApellidoM.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbCarrera.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (txbCURP.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreActa.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreContrato.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreCURP.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreINE.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreTitulo.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (tbNombreFoto.Text.Equals(""))
            {
                formularioAprobado = false;
            }
            else if (cbTipoSangre.SelectedIndex < 0)
            {
                formularioAprobado = false;
            }
            else if (txbRFC.Text.Equals(""))
            {
                formularioAprobado = false;
            }

            return formularioAprobado;
        }

        private bool verificarMateriasSeleccionadas()
        {
            bool materiasSeleccionadas = true;

            int cantidadSeleccionada = 0;

            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();

            foreach (CheckBox check in listaCheckDinamicos)
            {
                if (check.IsChecked == true)
                {
                    cantidadSeleccionada++;
                }
            }
            if (cantidadSeleccionada == 0)
            {
                materiasSeleccionadas = false;
            }

            return materiasSeleccionadas;
        }

        private void generarProfesor_Materia()
        {
            List<string> materias = obtenerMateriasSeleccionadas();
            foreach(string materia in materias)
            {
                Profesor_Materia profesor_Materia = new Profesor_Materia();
                profesor_Materia.RFC_Profesor = profesor.RFC;
                profesor_Materia.NRC_Materia = materia;
                int aux = materia.Length;
                profesor_Materia.ID_Profesor_Materia = Util.generarID(50-aux) + materia;
                materiasAsignadas.Add(profesor_Materia);
            }
        }

        private List<string> obtenerMateriasSeleccionadas()
        {
            List<string> listaMaterias = new List<string>();

            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();

            foreach (CheckBox check in listaCheckDinamicos)
            {
                if (check.IsChecked == true)
                {
                    listaMaterias.Add(check.Name);
                }
            }

            return listaMaterias;
        }

        private void llenarObjetoPersona(string idPersona)
        {
            persona.Nombre = txbNombre.Text;
            persona.ApellidoPaterno = txbApellidoP.Text;
            persona.ApellidoMaterno = txbApellidoM.Text;
            persona.Curp = txbCURP.Text;
            persona.Estado = "Activo";
            persona.ID = idPersona;
            persona.Clave_Escuela = "escuela-1";

            ComboBoxItem item = (ComboBoxItem)cbTipoSangre.SelectedItem;
            persona.TipoSangre = "" + item.Content;
        }

        private void llenarObjetoProfesor(string idPersona)
        {
            profesor.RFC = txbRFC.Text;
            profesor.Carrera = txbCarrera.Text;
            profesor.ID_Persona = idPersona;
        }

        //Dialogos
        private bool mostrarVentanaConfirmacion()
        {
            Alerta alerta = new Alerta("El profesor no tiene materias asignadas, ¿está seguro de querer continuar?",
                        Alertas.MessageType.Warning,
                        MessageButtons.AcceptCancel, "medium");
            bool seleccion = false;

            MessageBoxCustom confirmation = new MessageBoxCustom(alerta);
            confirmation.ShowDialog();
            if (confirmation.GetDialog())
            {
                seleccion = true;
            }
            else
            {
                confirmation.Close();
            }
            return seleccion;
        }

        private bool mostrarVentanaConfirmacion2()
        {
            Alerta alerta = new Alerta("Cualquier registro en proceso será perdido, ¿está seguro de querer continuar?",
                        Alertas.MessageType.Warning,
                        MessageButtons.AcceptCancel, "medium");
            bool seleccion = false;

            MessageBoxCustom confirmation = new MessageBoxCustom(alerta);
            confirmation.ShowDialog();
            if (confirmation.GetDialog())
            {
                seleccion = true;
            }
            else
            {
                confirmation.Close();
            }
            return seleccion;
        }

        private void mostrarVentanaExito()
        {
            Alerta alerta = new Alerta("Profesor resgitrado exitosamente", Alertas.MessageType.Success,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
            cambiarAVentanaProfesores();
        }

        private void mostrarVentanaError()
        {
            Alerta alerta = new Alerta("Error con la base de datos, intente más tarde", Alertas.MessageType.Error,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
            cambiarAVentanaProfesores();
        }

        private void mostrarVentanaFormularioVacio()
        {
            Alerta alerta = new Alerta("Debe llenar el formulario para continuar", Alertas.MessageType.Warning,
                        MessageButtons.Ok, "medium");
            MessageBoxCustom mensaje = new MessageBoxCustom(alerta);
            mensaje.ShowDialog();
            if (mensaje.GetDialog())
            {
                mensaje.Close();
            }
        }

        private void cambiarAVentanaProfesores()
        {
            ListaProfesores listaProfesores = new ListaProfesores();
            Application.Current.MainWindow = listaProfesores;
            Application.Current.MainWindow.Show();

            foreach (Window window in Application.Current.Windows.OfType<AgregarProfesor>())
            {
                ((AgregarProfesor)window).Close();
            }
        }

        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            if (mostrarVentanaConfirmacion2())
            {
                cambiarAVentanaProfesores();
            }
        }

        //Funcionalidades Manejo Archivos e Imagen
        private void clickAgregarTitulo(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreTitulo.Text = archivo.NombreArchivo;
                profesor.Titulo = archivo.PDFEnByte;
            }
        }

        private void clickAgregarActaNacimiento(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreActa.Text = archivo.NombreArchivo;
                persona.ActaNacimiento = archivo.PDFEnByte;
            }
        }

        private void clickAgregarINE(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreINE.Text = archivo.NombreArchivo;
                profesor.INE = archivo.PDFEnByte;
            }

        }

        private void clickAgregarContrato(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreContrato.Text = archivo.NombreArchivo;
                profesor.DocContrato = archivo.PDFEnByte;
            }
        }

        private void clickAgregarCURPDoc(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivo = Util.convertirPDFABites();
            if (archivo != null)
            {
                tbNombreCURP.Text = archivo.NombreArchivo;
                persona.DocCurp = archivo.PDFEnByte;
            }
        }

        private void clickAgregarFoto(object sender, RoutedEventArgs e)
        {
            DatosArchivo archivoImg = Util.convertirImgABitesYBitMap();
            if (archivoImg != null)
            {
                Console.WriteLine(archivoImg.ImagenBitMap.ToString());
                imgFoto.Source = archivoImg.ImagenBitMap;
                tbNombreFoto.Text = archivoImg.NombreArchivo;
                persona.Foto = archivoImg.ImgEnByte;
            }
        }

        private void clickAbrirArchivoTitulo(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreTitulo.Text.Equals(""))
            {
                Util.abrirArchivoPDF(profesor.Titulo, tbNombreTitulo.Text);
            }
        }

        private void clickAbrirArchivoActaNacimiento(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreActa.Text.Equals(""))
            {
                Util.abrirArchivoPDF(persona.ActaNacimiento, tbNombreActa.Text);
            }
        }

        private void clickAbrirArchivoINE(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreINE.Text.Equals(""))
            {
                Util.abrirArchivoPDF(profesor.INE, tbNombreINE.Text);
            }
        }

        private void clickAbrirArchivoCURPDoc(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreCURP.Text.Equals(""))
            {
                Util.abrirArchivoPDF(persona.DocCurp, tbNombreCURP.Text);
            }
        }

        private void clickAbrirArchivoContrato(object sender, MouseButtonEventArgs e)
        {
            if (!tbNombreContrato.Text.Equals(""))
            {
                Util.abrirArchivoPDF(profesor.DocContrato, tbNombreContrato.Text);
            }
        }
       
        //Configuración Ventana
        private void cargarMaterias()
        {
            List<Materia> listaMaterias = MateriaDAO.recuperarMaterias();
            int semestreMax = MateriaDAO.obtenerUltimoSemestre();

            for (int i = 1; i <= semestreMax; i++)
            {
                Label labelDinamico = new Label();
                labelDinamico.FontSize = 14;
                labelDinamico.FontWeight = System.Windows.FontWeights.Bold;
                labelDinamico.Content = "Semestre " + i;
                wpMaterias.Children.Add(labelDinamico);
                foreach (Materia materia in listaMaterias)
                {
                    if (materia.Semestre == i)
                    {
                        CheckBox checkDinamico = new CheckBox();

                        labelDinamico.Margin = new Thickness(5);
                        checkDinamico.Content = materia.Nombre;
                        checkDinamico.Name = materia.NRC;
                        checkDinamico.Margin = new Thickness(0, 0, 15, 0);

                        wpMaterias.Children.Add(checkDinamico);
                    }
                }
            }
        }


    }
}

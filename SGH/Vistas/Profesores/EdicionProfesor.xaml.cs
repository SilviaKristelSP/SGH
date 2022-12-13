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
using SGH.Vistas.MenuPrincipal;
using SGH.Vistas.LogIn;
using SGH.Utiles;
using SGH.Modelos;
using SGH.DAOs;
using SGH.Vistas.Alertas;

namespace SGH.Vistas.Profesores
{
    /// <summary>
    /// Lógica de interacción para EdicionProfesor.xaml
    /// </summary>
    public partial class EdicionProfesor : Window
    {
        private Persona persona = new Persona();
        private Profesor profesor = new Profesor();
        private List<Profesor_Materia> impartidasPrevioEdicion = new List<Profesor_Materia>();
        private String id = "";

        public EdicionProfesor(String idPersona)
        {
            InitializeComponent();
            this.id = idPersona;

            recuperarPersonaYProfesor();
            cargarDatosProfesor();

        }

        private void recuperarPersonaYProfesor()
        {
            persona = PersonaDAO.recuperarPersonaID(id);
            profesor = ProfesorDAO.recuperarProfesorID(id);
        }

        private void cargarDatosProfesor()
        {
            txbNombre.Text = persona.Nombre;
            txbApellidoM.Text = persona.ApellidoMaterno;
            txbApellidoP.Text = persona.ApellidoPaterno;
            txbCarrera.Text = profesor.Carrera;
            txbCURP.Text = persona.Curp;
            txbRFC.Text = profesor.RFC;
            txbRFC.IsReadOnly = true;
            for(int i = 0; i < 8; i++)
            {
                cbTipoSangre.SelectedIndex = i;
                ComboBoxItem item = (ComboBoxItem)cbTipoSangre.SelectedItem;
                String ts = "" + item.Content;
                if (ts.Equals(persona.TipoSangre))
                {
                    cbTipoSangre.SelectedIndex = i;
                    break;
                }
            }
            inicializarNombreArchivos();
            Uri uri = new Uri(Util.generarRutaParaImagen(persona.Foto, tbNombreFoto.Text + Util.generarID(30)));
            imgFoto.Source = new BitmapImage(uri);
            cargarMaterias();
            marcarMateriasSeleccionadas();
        }

        private void cargarMaterias()
        {
            List<Materia> listaMaterias = MateriaDAO.recuperarMaterias();
            int semestreMax = MateriaDAO.obtenerUltimoSemestre();
            Console.WriteLine("" + listaMaterias.Count);
            if (listaMaterias.Count <= 0)
            {
                Label labelDinamico = new Label();
                labelDinamico.FontSize = 14;
                labelDinamico.FontWeight = System.Windows.FontWeights.Bold;
                labelDinamico.Content = "No tiene materias asiganadas";
                wpMaterias.Children.Add(labelDinamico);
            }
            else
            {
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

        private void marcarMateriasSeleccionadas()
        {
            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();
            impartidasPrevioEdicion = MateriaDAO.recuperarProfesorMaterias(profesor.RFC);
            foreach (CheckBox check in listaCheckDinamicos)
            {
                foreach(Profesor_Materia materia in impartidasPrevioEdicion)
                {
                    if (check.Name.Equals(materia.NRC_Materia))
                        check.IsChecked = true;
                }
            }
        }


        private void inicializarNombreArchivos()
        {
            string nombreCompleto = Util.obtenerNombreSinEspacios(persona);
            tbNombreTitulo.Text = "Titulo_" + nombreCompleto + ".pdf";
            tbNombreActa.Text = "ActaNacimiento_" + nombreCompleto + ".pdf";
            tbNombreINE.Text = "INE_" + nombreCompleto + ".pdf";
            tbNombreCURP.Text = "CURP_" + nombreCompleto + ".pdf";
            tbNombreContrato.Text = "Contrato_" + nombreCompleto + ".pdf";
            tbNombreFoto.Text = "Foto_" + nombreCompleto;
        }

        private void modificarDatosPersonaYProfesor()
        {
            persona.Nombre = txbNombre.Text;
            persona.ApellidoPaterno = txbApellidoP.Text;
            persona.ApellidoMaterno = txbApellidoM.Text;
            persona.Curp = txbCURP.Text;
            persona.Estado = "Activo";
            persona.ID = id;
            persona.Clave_Escuela = "escuela-1";

            ComboBoxItem item = (ComboBoxItem)cbTipoSangre.SelectedItem;
            persona.TipoSangre = "" + item.Content;

            profesor.RFC = txbRFC.Text;
            profesor.Carrera = txbCarrera.Text;
            profesor.ID_Persona = id;
        }

        private bool comprobarTamanioCadena()
        {
            if (txbRFC.Text.Length <= 13 && txbCURP.Text.Length <= 18)
                return true;
            else
            {
                lbMaxCURP.Visibility = Visibility.Visible;

                return false;
            }
        }

        private List<Profesor_Materia> encontrarMateriasAQuitar()
        {
            List<Profesor_Materia> aBorrar = new List<Profesor_Materia>();
            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();
            foreach (CheckBox check in listaCheckDinamicos)
            {
                if (encontrarNRC(check.Name) && check.IsChecked == false)
                {
                    int posicion = encontrarPosicionNRC(check.Name);
                    if(posicion > -1)
                        aBorrar.Add(impartidasPrevioEdicion[posicion]);
                }
            }

            if (aBorrar.Count <= 0)
                aBorrar = null;

            return aBorrar;
        }

        private List<Profesor_Materia> encontrarMateriasAAsignar()
        {
            UIElementCollection elementosWp = wpMaterias.Children;
            List<CheckBox> listaCheckDinamicos = elementosWp.OfType<CheckBox>().ToList();
            if (impartidasPrevioEdicion != null && impartidasPrevioEdicion.Count > 0)
                return asignarConBaseAExistentes(listaCheckDinamicos);
            else
                return asignarSinBaseAExistentes(listaCheckDinamicos);
        }

        private List<Profesor_Materia> asignarConBaseAExistentes(List<CheckBox> listaCheckDinamicos)
        {
            List<Profesor_Materia> aAsignar = new List<Profesor_Materia>();
            foreach (CheckBox check in listaCheckDinamicos)
            {
                if (check.IsChecked == true && (!encontrarNRC(check.Name)))
                {
                    Profesor_Materia profesor_Materia = new Profesor_Materia();
                    profesor_Materia.RFC_Profesor = profesor.RFC;
                    profesor_Materia.NRC_Materia = check.Name;
                    int aux = check.Name.Length;
                    profesor_Materia.ID_Profesor_Materia = Util.generarID(50 - aux) + check.Name;
                    aAsignar.Add(profesor_Materia);
                }
            }

            if (aAsignar.Count <= 0)
                aAsignar = null;

            return aAsignar;
        }

        private bool encontrarNRC(string nrc)
        {
            bool encontrado = false;
            foreach (Profesor_Materia materia in impartidasPrevioEdicion)
            {
                if (nrc.Equals(materia.NRC_Materia))
                {
                    encontrado = true;
                    break;
                }
            }
            return encontrado;
        }

        private int encontrarPosicionNRC(string nrc)
        {
            int posicion = -1;
            bool encontrado = false;
            foreach (Profesor_Materia materia in impartidasPrevioEdicion)
            {
                posicion++;
                if (nrc.Equals(materia.NRC_Materia))
                {
                    encontrado = true;
                    break;
                }   
            }

            if(!encontrado)
                posicion = -1;

            return posicion;
        }

        private List<Profesor_Materia> asignarSinBaseAExistentes(List<CheckBox> listaCheckDinamicos)
        {
            List<Profesor_Materia> aAsignar = new List<Profesor_Materia>();
            foreach (CheckBox check in listaCheckDinamicos)
            {
                if (check.IsChecked == true)
                {
                    Profesor_Materia profesor_Materia = new Profesor_Materia();
                    profesor_Materia.RFC_Profesor = profesor.RFC;
                    profesor_Materia.NRC_Materia = check.Name;
                    int aux = check.Name.Length;
                    profesor_Materia.ID_Profesor_Materia = Util.generarID(50 - aux) + check.Name;
                    aAsignar.Add(profesor_Materia);
                }
            }

            if (aAsignar.Count <= 0)
                aAsignar = null;

            return aAsignar;
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

        private List<Profesor_Materia>[] generarArregloMateriasProfesor()
        {
            List<Profesor_Materia>[] m = new List<Profesor_Materia>[2];
            m[0] = encontrarMateriasAAsignar();
            m[1] = encontrarMateriasAQuitar();
            return m;
        }

        //Funcionalidades Archivos

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

        //Funcionalidad botones

        private void clickGuardarCambios(object sender, RoutedEventArgs e)
        {
            if (verificarFormulario())
            {
                if (comprobarTamanioCadena())
                {
                    modificarDatosPersonaYProfesor();
                    if (verificarMateriasSeleccionadas())
                    {
                        if (ProfesorDAO.editarProfesor(encontrarMateriasAQuitar(), encontrarMateriasAAsignar(), profesor, persona))
                            mostrarVentanaExito();
                        else
                            mostrarVentanaError();
                    }
                    else
                    {
                        if (mostrarVentanaConfirmacion())
                        {
                            if (ProfesorDAO.editarProfesor(impartidasPrevioEdicion, new List<Profesor_Materia>(), profesor, persona))
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

        private void ClickRetroceder(object sender, RoutedEventArgs e)
        {
            if (mostrarVentanaConfirmacion2())
                cambiarAVentanaProfesores();
        }

        //dialogos
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
            Alerta alerta = new Alerta("Profesor editado exitosamente", Alertas.MessageType.Success,
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

            foreach (Window window in Application.Current.Windows.OfType<EdicionProfesor>())
            {
                ((EdicionProfesor)window).Close();
            }
        }


    }
}


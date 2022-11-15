using System.Linq;
using System.Windows;
using System.Data.Entity.Core;
using SGH.Vistas.Alertas;
using SGH.Vistas.MenuPrincipal;
using SGH.Vistas.Excepciones;
using SGH.ServiceReferenceSGH;
using System;

namespace SGH.Vistas.LogIn
{
    
    public partial class LogInSGH : Window
    {
        
        //private SGHContext sghContext = new SGHContext();
        private static Administrador administrador = new ServiceReferenceSGH.Administrador();        

        public LogInSGH()
        {
            InitializeComponent();
        }

        private void InicioSesion(object sender, RoutedEventArgs e)
        {

            ServiceSGHClient serviceSGHClient = new ServiceSGHClient();              

            try
            {

                //Administrador usuarioAdministrador = ExisteUsuario();
                Administrador usuarioAdministrador = serviceSGHClient.ExisteUsuario(textBoxLogInCorreo.Text);
                bool existeUsuario = usuarioAdministrador == null ? false : true;

                if (!existeUsuario)
                {
                    stackPanelBlack.Visibility = Visibility.Visible;

                    Alerta alerta = new Alerta("Correo y/o contraseña inválidos",
                                          MessageType.Error, MessageButtons.Ok, "short");
                    throw new ErrorAlertException(alerta);

                }

                EncryptPassword encryptPassword = new EncryptPassword();

                var contraseniaEncriptada = encryptPassword.GetSHA256(passwordBoxLogInContrasenia.Password);
                if (usuarioAdministrador.Contrasenia != contraseniaEncriptada)
                {
                    stackPanelBlack.Visibility = Visibility.Visible;
                    Alerta alerta = new Alerta("Correo y/o contraseña inválidos",
                                         MessageType.Error, MessageButtons.Ok, "short");
                    throw new ErrorAlertException(alerta);
                }

                SetUsuario(usuarioAdministrador);

                MenuPrincipalSGH menuPrincipal = new MenuPrincipalSGH();
                Application.Current.MainWindow = menuPrincipal;
                Application.Current.MainWindow.Show();

                foreach (Window window in Application.Current.Windows.OfType<LogInSGH>())
                {
                    ((LogInSGH)window).Close();
                }

            }
            catch (EntityException ex)
            {
                MessageBox.Show("Error en la Base de Datos");
                //log.Add(ex.ToString());
            }
            catch (ErrorAlertException)
            {
                stackPanelBlack.Visibility = Visibility.Collapsed;

            }


        }

        //public Administrador ExisteUsuario()
        //{
           
        //    Administrador administrador = 
        //        sghContext.Administradors.Where(
        //            admin => admin.Correo == textBoxLogInCorreo.Text).FirstOrDefault();

            
        //    return administrador;
        //}

        public void SetUsuario(Administrador administradorAutenticado)
        {
            administrador.RFC = administradorAutenticado.RFC;
            administrador.NombreCompleto = administradorAutenticado.NombreCompleto;
            administrador.Contrasenia = administradorAutenticado.Contrasenia;
            administrador.Telefono = administradorAutenticado.Telefono;
            administrador.Correo = administradorAutenticado.Correo;
            administrador.Rol = administradorAutenticado.Rol;
            administrador.Clave_Escuela = administradorAutenticado.Clave_Escuela;
        }        

        public Administrador GetUsuario()
        {
            return administrador;
        }

    }
}

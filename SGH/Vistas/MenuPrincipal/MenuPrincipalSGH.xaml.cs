using SGH.Vistas.LogIn;
using ShowMeTheXAML;
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

namespace SGH.Vistas.MenuPrincipal
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipalSGH : Window
    {

        private static Administrador administradorMenu;

        public MenuPrincipalSGH()
        {
            InitializeComponent();
            //LogInSGH logInSGH = new LogInSGH();
            //administradorMenu = logInSGH.GetUsuario();
            //SetInformacionAdministrador(administradorMenu);

  
        }   
        
        public void SetInformacionAdministrador(Administrador administrador)
        {

            toggleAdministrador.Content = administrador.NombreCompleto.ToUpper().First();
            textBlockAdministrador.Text = administrador.NombreCompleto;

        }
    }
}

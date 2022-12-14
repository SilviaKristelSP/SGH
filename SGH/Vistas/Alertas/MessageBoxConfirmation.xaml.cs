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

namespace SGH.Vistas.Alertas
{
    public partial class MessageBoxConfirmation : Window
    {

        private string buttonType="";

        public MessageBoxConfirmation() { }
        
        public MessageBoxConfirmation(string message, MessageButtons buttons)
        {
            InitializeComponent();

            buttonType = buttons.ToString();
            textBoxMessage.Text = message;

            switch (buttons)
            {
                case MessageButtons.Ok:
                    buttonOk.Visibility = Visibility.Visible;
                    buttonCancel.Visibility = Visibility.Collapsed;
                    buttonAccept.Visibility = Visibility.Collapsed;
                    break;

                case MessageButtons.AcceptCancel:
                    buttonAccept.Visibility = Visibility.Visible;
                    buttonCancel.Visibility = Visibility.Visible;
                    buttonOk.Visibility = Visibility.Collapsed;
                    break;

            }
        }
       
        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {           
            if (buttonType.Equals("Ok"))
            {               
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
           
            this.Close();
        }

        

    }   

}

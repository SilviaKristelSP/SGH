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

namespace SGH.Vistas.Alertas
{
    public partial class MessageBoxCustom : Window
    {
        private static string confirmationCode="";
        private bool dialogResult = false;

        public MessageBoxCustom() { }

        public string GetCode()
        {
            return confirmationCode;
        }

        public bool GetDialog()
        {
            return dialogResult;
        } 

        public MessageBoxCustom(Alerta alerta)
        {
            InitializeComponent();            
            if (alerta.messageLength.Equals("long"))
                textBoxMessage.FontSize = 16;
            else if (alerta.messageLength.Equals("medium"))
                textBoxMessage.FontSize = 20;

            textBoxMessage.Text = alerta.message;
            
            switch (alerta.type)
            {

                case MessageType.Info:
                    txtTitle.Text = "Info";                    
                    break;

                case MessageType.Confirmation:
                    txtTitle.Text = "Confirmación";                    
                    break;

                case MessageType.Success:                    
                    txtTitle.Text = "Éxito";                    
                    break;

                case MessageType.Warning:
                    txtTitle.Text = "Advertencia";                    
                    break;

                case MessageType.Error:                    
                    txtTitle.Text = "Error";                    
                    break;

                case MessageType.Submit:                    
                    break;

            }

            switch (alerta.buttons)
            {               
                case MessageButtons.Ok:                    
                    buttonOk.Visibility = Visibility.Visible;
                    buttonCancel.Visibility = Visibility.Collapsed;                                        
                    buttonAccept.Visibility = Visibility.Collapsed;
                    break;

                case MessageButtons.Submit:                                      
                    buttonOk.Visibility = Visibility.Collapsed;
                    buttonCancel.Visibility = Visibility.Collapsed;                    
                    buttonAccept.Visibility = Visibility.Collapsed;
                    break;

                case MessageButtons.AcceptCancel:
                    buttonAccept.Visibility = Visibility.Visible;
                    buttonOk.Visibility = Visibility.Collapsed;                                        
                    break;

            }            
        }                  

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            dialogResult = true;           
            this.Close();
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            dialogResult = true;
            //confirmationCode = textBoxConfirmationCode.Text;
            this.Close();
            
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            dialogResult = true;            
            this.Close();
        }
       
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            dialogResult = false;           
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            dialogResult = false;           
            this.Close();
        }              
    }

    public class Alerta
    {
        public string message { get; set; }
        public MessageType type { get; set; }
        public MessageButtons buttons { get; set; }
        public string messageLength { get; set; }

        public Alerta(string message, MessageType type, MessageButtons buttons, string messageLength)
        {
            this.message = message;
            this.type = type;
            this.buttons = buttons;
            this.messageLength = messageLength;
        }
    }

    public enum MessageType
    {
        Info,
        Confirmation,
        Success,
        Warning,
        Error,
        Submit,
    }
    public enum MessageButtons
    {                
        Ok,
        Submit,
        AcceptCancel,
    }
}

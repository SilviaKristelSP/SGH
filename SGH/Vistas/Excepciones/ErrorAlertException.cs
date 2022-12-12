using SGH.Vistas.Alertas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.Vistas.Excepciones
{
    public class ErrorAlertException : Exception
    {
        public ErrorAlertException(Alerta alerta) : base()
        {
            new MessageBoxCustom(alerta).ShowDialog();
        }

        public ErrorAlertException(string mensaje) : base()
        {
            Console.WriteLine(mensaje);
        }
      
    }
}

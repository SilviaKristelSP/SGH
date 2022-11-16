using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ServidorSGH.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using ServidorSGH.Modelos;

namespace ServidorSGH
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IServiceSGH
    {        

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public String GetRFCUsuario()
        {

            ServidorSGHContext servidorSGHContext = new ServidorSGHContext();            
            Administrador administrador = servidorSGHContext.Administradors.Where(admin => admin.RFC == "123").FirstOrDefault();

            return administrador.NombreCompleto;
        }

        public Administrador ExisteUsuario(string correo)
        {
            ServidorSGHContext sghContext = new ServidorSGHContext();
            Administrador administrador =
                    sghContext.Administradors.Where(
                    admin => admin.Correo == correo
                    ).FirstOrDefault();

            
            

            return administrador;
        }
    }
}

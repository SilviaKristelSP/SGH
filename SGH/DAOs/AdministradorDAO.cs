using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.DAOs
{
    public class AdministradorDAO
    {
        private SGHContext sghContext = new SGHContext();

        public Administrador ExisteUsuario(string correo)
        {

            Administrador administrador =
                sghContext.Administradors.Where(
                    admin => admin.Correo == correo).FirstOrDefault();


            return administrador;
        }



    }
}

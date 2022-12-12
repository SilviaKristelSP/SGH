using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;

namespace SGH.DAOs
{
    public class BajaDAO
    {
        public static Baja recuperarBaja(string idPersona)
        {
            Baja baja = new Baja();
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    baja = bd.Bajas.Where(i => i.ID_Persona == idPersona).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                baja = null;
            }
            return baja;
        }
    }
}

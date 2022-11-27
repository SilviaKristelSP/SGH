using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;

namespace SGH.DAOs
{
    public class ProfesorDAO
    {
        public static Profesor recuperarProfesorID(string idPersona)
        {
            Profesor lista = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    lista = bd.Profesors.Find(idPersona);
                }
            }
            catch (Exception ex)
            {
                lista = null;
            }
            return lista;
        }

        public static bool editarProfesor(Profesor profesor)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Entry(profesor).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }
    }
}

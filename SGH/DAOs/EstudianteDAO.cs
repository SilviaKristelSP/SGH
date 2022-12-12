using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;

namespace SGH.DAOs
{
    public class EstudianteDAO
    {
        public static Estudiante recuperarEstudianteID(string idPersona)
        {
            Estudiante lista = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    lista = bd.Estudiantes.Find(idPersona);
                }
            }
            catch (Exception ex)
            {
                lista = null;
            }
            return lista;
        }

        public static bool editarEstudiante(Estudiante estudiante)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Entry(estudiante).State = System.Data.Entity.EntityState.Modified;
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

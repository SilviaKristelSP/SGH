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

        public static List<String> recuperarIDsProfesoresActivos()
        {
            List<String> idsRecuperados = null;
            List<Profesor> profesores = null;
            IEnumerable<Profesor> lista = null;

            using (SGHContext bd = new SGHContext())
            {
                lista = bd.Profesors;
                profesores = lista.ToList();
            }
            idsRecuperados = new List<String>();
            foreach (Profesor profesor in profesores)
            {
                idsRecuperados.Add(profesor.ID_Persona);
            }

            return idsRecuperados;
        }

        public static List<String> recuperarCarreras()
        {
            List<String> carreras = null;
            List<Profesor> profesores = null;
            IEnumerable<Profesor> lista = null;

            using (SGHContext bd = new SGHContext())
            {
                lista = bd.Profesors;
                profesores = lista.ToList();
            }
            carreras = new List<string>();
            foreach (Profesor profesor in profesores)
            {
                carreras.Add(profesor.Carrera);
            }

            return carreras;
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

        public static bool registrarProfesor(Profesor profesor)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Profesors.Add(profesor);
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

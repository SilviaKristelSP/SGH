using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.DAOs
{
    public class EstudianteDAO
    {
        private SGHContext sghContext = new SGHContext();

        public Estudiante recuperarEstudiante(Estudiante estudiante)
        {
            Persona estudianteRecuperado = (from persona in sghContext.Personas
                                            where persona.Curp == estudiante.Persona.Curp
                                            select persona).FirstOrDefault();

            estudiante = (from alumno in sghContext.Estudiantes
                          where alumno.ID_Persona == estudianteRecuperado.ID
                          select alumno).FirstOrDefault();


            return estudiante;
        }
      
        public static Estudiante recuperarEstudianteID(string idPersona)
        {
            Estudiante lista = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    lista = bd.Estudiantes.Where(r => r.ID_Persona == idPersona).First();
                }
            }
            catch (Exception ex)
            {
                lista = null;
            }
            return lista;
        }

        public static List<String> recuperarIDsEstudiantesActivos()
        {
            List<String> idsRecuperados = null;
            List<Estudiante> estudiantes = null;
            IEnumerable<Estudiante> lista = null;

            using (SGHContext bd = new SGHContext())
            {
                lista = bd.Estudiantes;
                estudiantes = lista.ToList();
            }
            idsRecuperados = new List<String>();
            foreach (Estudiante estudiante in estudiantes)
            {
                idsRecuperados.Add(estudiante.ID_Persona);
            }

            return idsRecuperados;
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

        public static bool registrarEstudiante(Estudiante estudiante)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Estudiantes.Add(estudiante);
                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static string obtenerMatricula(string idPersona)
        {
            string matricula = "";
            Estudiante est = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    est = bd.Estudiantes.Find(idPersona);
                    matricula = est.Matricula;
                }
            }
            catch (Exception ex)
            {
                matricula = "";
            }
            return matricula;
        }

    }
}

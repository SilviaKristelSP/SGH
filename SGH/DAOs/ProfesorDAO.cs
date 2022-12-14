using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;
using SGH.Utiles;

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
                    lista = bd.Profesors.Where(r => r.ID_Persona == idPersona).First();
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

        public static string obtenerRFC(string idPersona)
        {
            string rfc = "";
            Profesor prof = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    prof = bd.Profesors.Find(idPersona);
                    rfc = prof.RFC;
                }
            }
            catch (Exception ex)
            {
                rfc = "";
            }
            return rfc;
        }

        public static bool darDeBajaProfesor(string idPersona, string rfc, Baja baja)
        {
            List<Profesor_Materia> profMat = MateriaDAO.recuperarProfesorMaterias(rfc);
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Personas.Find(idPersona).Estado = "Baja";
                    bd.Bajas.Add(baja);

                    if (profMat.Count > 0)
                    {
                        foreach(Profesor_Materia profMateria in profMat)
                        {
                            bd.Profesor_Materia.Attach(profMateria);
                            bd.Entry(profMateria).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }

                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }


        public static bool editarProfesor(List<Profesor_Materia> materiasABorrar, List<Profesor_Materia> materiasAgregar, Profesor profesor, Persona persona)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Entry(persona).State = System.Data.Entity.EntityState.Modified;
                    bd.Entry(profesor).State = System.Data.Entity.EntityState.Modified;

                    if (materiasABorrar != null && materiasABorrar.Count > 0)
                    {
                        foreach (Profesor_Materia profMateria in materiasABorrar)
                        {
                            bd.Profesor_Materia.Attach(profMateria);
                            bd.Entry(profMateria).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }

                    if(materiasAgregar != null && materiasAgregar.Count > 0)
                    {
                        foreach(Profesor_Materia pf in materiasAgregar)
                            bd.Profesor_Materia.Add(pf);
                    }

                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static bool agregarProfesorConMaterias(Persona persona, Profesor profesor, List<Profesor_Materia> materiasAImpartir)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Personas.Add(persona);
                    bd.Profesors.Add(profesor);

                    if(materiasAImpartir != null)
                    {
                        foreach (Profesor_Materia pf in materiasAImpartir)
                            bd.Profesor_Materia.Add(pf);
                    }

                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static bool agregarProfesorSinMaterias(Persona persona, Profesor profesor)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Personas.Add(persona);
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

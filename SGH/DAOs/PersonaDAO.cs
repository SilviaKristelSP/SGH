using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;

namespace SGH.DAOs
{
    public class PersonaDAO
    {
        public static bool registrarPersona(Persona persona)
        {
            bool ejecucionExitosa = true;
            try
            {
                using(SGHContext bd = new SGHContext())
                {
                    bd.Personas.Add(persona);
                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static bool darDeBajaPersona(string id, Baja baja)
        {
            bool ejecucionExitosa = true;
            try
            {
                using(SGHContext bd =new SGHContext())
                {
                    bd.Personas.Find(id).Estado = "Baja";

                    bd.Bajas.Add(baja);

                    bd.SaveChanges();
                }
            }catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static bool cancelarBajaPersona(string id)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Personas.Find(id).Estado = "Activo";

                    Baja baja = bd.Bajas.Where(b => b.ID_Persona == id).First();
                    bd.Bajas.Remove(baja);

                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static bool editarPersona(Persona persona)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    bd.Entry(persona).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static List<Persona> recuperarPersonas(List<String> ids, string estado)
        {
            List<Persona> lista = new List<Persona>();
            Persona personaAux = new Persona();
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    foreach (String id in ids)
                    {
                        personaAux = bd.Personas.Where(e => e.Estado == estado).Where(i => i.ID == id).FirstOrDefault();
                        if(personaAux != null)
                        {
                            lista.Add(personaAux);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                lista = null;
            }
            return lista;
        }

        public static Persona recuperarPersonaID(string idPersona)
        {
            Persona lista = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    lista = bd.Personas.Find(idPersona);
                }
            }
            catch (Exception ex)
            {
                lista = null;
            }
            return lista;
        }
    }
}

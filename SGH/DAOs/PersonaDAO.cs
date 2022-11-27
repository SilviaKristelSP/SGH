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

        public static bool darDeBajaPersona(string id)
        {
            bool ejecucionExitosa = true;
            try
            {
                using(SGHContext bd =new SGHContext())
                {
                    bd.Personas.Find(id).Estado = "Baja";
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

        public static IEnumerable<Persona> recuperarPersonas(string tipo)
        {
            IEnumerable<Persona> lista = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    lista = bd.Personas;
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

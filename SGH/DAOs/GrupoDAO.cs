using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;

namespace SGH.DAOs
{
    internal class GrupoDAO
    {
        public static List<Grupo> recuperarListadeGrupos()
        {
            List<Grupo> grupos = new List<Grupo>();
            IEnumerable<Grupo> listaGrupos = new List<Grupo>();

            using(SGHContext bd = new SGHContext())
            {
                listaGrupos = bd.Grupoes;
                grupos = listaGrupos.ToList();
            }

            return grupos;
        }

        public static List<Grupo> recuperarListadeGruposporSemestre(int semestre)
        {
            List<Grupo> grupos = new List<Grupo>();
            IEnumerable<Grupo> listaGrupos = new List<Grupo>();

            using (SGHContext bd = new SGHContext())
            {
                listaGrupos = bd.Grupoes.Where(r => r.Semestre == semestre);
                grupos = listaGrupos.ToList();
            }

            return grupos;
        }

        public static Grupo buscarGrupo(int semestre, String letra)
        {
            Grupo buscar = new Grupo();

            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    buscar = bd.Grupoes.Where(r => r.Semestre == semestre && r.Letra == letra).First();
                }
            }
            catch(Exception ex)
            { 

            }
            
            return buscar;
        }

        public static bool registrarGrupo(Grupo grupo)
        {
            bool ejecucionExitosa = true;
            try
            {
                using(SGHContext bd = new SGHContext())
                {
                    bd.Grupoes.Add(grupo);
                    bd.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                ejecucionExitosa = false;
                Console.WriteLine(ex.Message);
            }
            return ejecucionExitosa;
        }

        public static bool eliminarGrupo(Grupo grupo)
        {
            bool ejecucionExitosa = true;

            try
            {
                using(SGHContext bd = new SGHContext())
                {
                    bd.Grupoes.Remove(grupo);
                    bd.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                ejecucionExitosa=false;
            }
            return ejecucionExitosa;
        }

        public static List<Estudiante> recuperarEstudiantesInactivos()
        {
            List<Estudiante> estudiantesInactivosRecuperados = new List<Estudiante>();
            IEnumerable<Estudiante> lista = null;

            using (SGHContext bd = new SGHContext())
            {
                lista = bd.Estudiantes.Where(r => r.ID_Grupo == null);
                estudiantesInactivosRecuperados = lista.ToList();
            }

            return estudiantesInactivosRecuperados;
        }

        public static List<Persona>  recuperarNombresEstudiantes(List<Estudiante> estudiantes)
        {
            List<Persona> personas = null;
            Persona persona = null;
            foreach (Estudiante estudiante in estudiantes)
            {
                using (SGHContext bd = new SGHContext())
                {
                    persona = bd.Personas.Where(r => r.ID == estudiante.ID_Persona).First();
                    personas.Add(persona);
                }
            }

            return personas;
        }

        public static List<Estudiante> recuperarEstudiantesporGrupo(String idGrupo)
        {
            List<Estudiante> estudiantes = null;
            IEnumerable<Estudiante> lista = null;

            using(SGHContext bd = new SGHContext())
            {
                lista = bd.Estudiantes.Where(r => r.ID_Grupo == idGrupo);
                estudiantes = lista.ToList();
            }

            return estudiantes;
        }

        public static Estudiante recuperarEstudianteporIDPersona(String idPersona)
        {
            Estudiante estudiante = null;

            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    estudiante = bd.Estudiantes.Where(r => r.ID_Persona == idPersona).First();
                }
            }
            catch(Exception ex)
            {

            }
            return estudiante;
        }

        public static String recuperarultimoIDGrupo()
        {
            Grupo grupo = new Grupo();
            String idGrupo = "";
            try
            {
                using(SGHContext bd = new SGHContext())
                {
                    grupo = bd.Grupoes.Last();
                    idGrupo = grupo.ID;
                }

            }catch(Exception ex)
            {

            }
            return idGrupo;
        }
    }
}

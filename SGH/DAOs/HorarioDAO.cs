using SGH.Modelos;
using SGH.Vistas.Alertas;
using SGH.Vistas.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using SGH.Vistas;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Windows.Controls.Primitives;

namespace SGH.DAOs
{
    public class HorarioDAO
    {
        private SGHContext sghContext = new SGHContext();
        private Log log = new Log();

        public List<Grupo> GetGrupos()
        {
            List<Grupo> grupos = sghContext.Grupoes.ToList();
            return grupos;
        }

        public List<Grupo> GetGruposConHorario()
        {
            List<Grupo> gruposConHorario = new List<Grupo>();
            List<Grupo> grupos = sghContext.Grupoes.ToList();

            if (grupos.Count > 0)
            {
                foreach (Grupo grupo in grupos)
                {
                    Sesion existeSesion = sghContext.Sesions.Where(sesion => sesion.ID_Grupo == grupo.ID).FirstOrDefault();
                    if (existeSesion != null)
                    {
                        gruposConHorario.Add(grupo);
                    }

                }
            }
          
            return gruposConHorario;
        }

        public List<Grupo> GetGruposSinHorario()
        {
            List<Grupo> gruposSinHorario = new List<Grupo>();
            List<Grupo> grupos = sghContext.Grupoes.ToList();

            if (grupos.Count > 0)
            {
                foreach (Grupo grupo in grupos)
                {
                    Sesion existeSesion = sghContext.Sesions.Where(sesion => sesion.ID_Grupo == grupo.ID).FirstOrDefault();
                    if (existeSesion == null)
                    {
                        gruposSinHorario.Add(grupo);
                    }

                }
            }
               
            return gruposSinHorario;
        }

        public string GetGrupoId(string letra, int semestre)
        {
            Grupo grupo = sghContext.Grupoes.Where(gr => gr.Letra == letra && gr.Semestre == semestre).FirstOrDefault();
            return grupo.ID;
        }

        public Grupo GetGrupoByID(string id)
        {
            Grupo grupo = sghContext.Grupoes.Where(gr => gr.ID.Equals(id)).FirstOrDefault();
            return grupo;
        }

        public Grupo GetGrupo(string letra, int semestre)
        {
            Grupo grupo = sghContext.Grupoes.Where(gr => gr.Letra == letra && gr.Semestre == semestre).FirstOrDefault();
            return grupo;
        }

        public List<Sesion> GetSesionesByGrupo(string grupoId)
        {
            List<Sesion> sesiones = sghContext.Sesions.Where(sesion => sesion.ID_Grupo == grupoId).ToList();
            return sesiones;
        }

        public Profesor_Materia GetProfesorMateriaBySesion(string sesionID)
        {
            Profesor_Materia profesor_Materia = null;
                      
            Materia_Sesion materia_Sesion = sghContext.Materia_Sesion.Where(ms => ms.ID_Sesion == sesionID).FirstOrDefault();    
            if (materia_Sesion != null)
            {
                Profesor_Materia profesorMateria = sghContext.Profesor_Materia.Where(pm => pm.ID_Profesor_Materia == materia_Sesion.ID_Profesor_Materia).FirstOrDefault();
                if (profesorMateria != null)
                {
                    profesor_Materia = profesorMateria;
                }                
            }
                      

            return profesor_Materia;
        }

        public List<Materia> GetMateriasBySemestre(int semestre)
        {            
            List<Materia> materias = sghContext.Materias.Where(materia => materia.Semestre == semestre).ToList();
            return materias;
        }
    
        public List<Profesor> GetProfesoresByMateria(string nrc)
        {
            List<Profesor> listaProfesores = new List<Profesor>();
            List<Profesor_Materia> listaProfesorMateria = sghContext.Profesor_Materia.Where(pm => pm.NRC_Materia.Equals(nrc)).ToList();

            if (listaProfesorMateria.Count > 0)
            {
                foreach (Profesor_Materia profesor_Materia in listaProfesorMateria)
                {
                    Profesor profesor = sghContext.Profesors.Where(p => p.RFC.Equals(profesor_Materia.RFC_Profesor)).FirstOrDefault();
                    listaProfesores.Add(profesor);
                }               
            }

            return listaProfesores;
        }

        public List<Materia> GetMateriasConProfesorAsignado()
        {
            List<Materia> materias = new List<Materia>();
            List<Materia> materiasConProfesorAsignado = (from mat in sghContext.Materias
                                 join prof_mat in sghContext.Profesor_Materia
                                 on mat.NRC equals prof_mat.NRC_Materia
                                 select mat).ToList();

            return materiasConProfesorAsignado;
        }

        public List<Profesor> GetProfesoresByNRC(string nrc)
        {            
            List<Profesor> profesores = (from pm in (sghContext.Profesor_Materia.Where(pm => pm.NRC_Materia.Equals(nrc)).ToList())
                                        join prof in sghContext.Profesors
                                        on pm.RFC_Profesor equals prof.RFC
                                        select prof).ToList();
            return profesores;                                        
        }

        public Profesor GetProfesorByRFC(string rfc)
        {
            Profesor profesor = sghContext.Profesors.Where(prof => prof.RFC.Equals(rfc)).FirstOrDefault();
            return profesor;
        }

        public Materia GetMateriaByNRC(string nrc)
        {
            Materia materia = sghContext.Materias.Where(mt => mt.NRC.Equals(nrc)).FirstOrDefault();
            return materia;
        }
        
        public Persona GetPersonaByID(string idPersona)
        {
            Persona persona = sghContext.Personas.Where(pers => pers.ID.Equals(idPersona)).FirstOrDefault();
            return persona;
        }
    
        public void GuardarHorario()
        {

        }
    
        public Profesor_Materia GetProfesorMateria(string rfc, string nrc)
        {
            Profesor_Materia profesor_Materia = sghContext.Profesor_Materia.Where(pm => pm.RFC_Profesor.Equals(rfc) && pm.NRC_Materia.Equals(nrc)).FirstOrDefault();
            return profesor_Materia;
        }
    
        public Sesion ValidarSesion(Sesion sesion, string idProfesorMateria, int semestre)
        {

            List<Sesion> listaSesiones = (from se in sghContext.Sesions
                                          join grupo in sghContext.Grupoes
                                          on se.ID_Grupo equals grupo.ID
                                          where grupo.Semestre == semestre
                                          select se).ToList();

            Sesion sesionEncontrada = (from se in listaSesiones
                                       join ms in sghContext.Materia_Sesion
                                       on se.ID equals ms.ID_Sesion
                                       where 
                                       se.DiaSemana.Equals(sesion.DiaSemana) &&
                                       se.HoraFinal.Equals(sesion.HoraFinal) &&
                                       se.HoraInicio.Equals(sesion.HoraInicio) &&
                                       ms.ID_Profesor_Materia.Equals(idProfesorMateria)
                                       select se).FirstOrDefault();

            return sesionEncontrada;

        }
    
        public bool EncontrarSesionPorId(string sesionID)
        {
            bool existeSesion = false;
            Sesion sesion = sghContext.Sesions.Where(se => se.ID.Equals(sesionID)).FirstOrDefault();            
            if (sesion != null)
            {
                existeSesion = true;
            }
            return existeSesion;
        }        

        public bool EncontrarMateriaSesionPorId(string materiSesionID)
        {
            bool existeSesion = false;
            Materia_Sesion materia_Sesion = sghContext.Materia_Sesion.Where(se => se.ID_Materia_Sesion.Equals(materiSesionID)).FirstOrDefault();
            if (materia_Sesion != null)
            {
                existeSesion = true;
            }
            return existeSesion;
        }

        public bool GuardarSesion(Sesion sesion)
        {
            int cambiosHechos;
            bool sesionGuardada = false;
            try
            {
                sghContext.Sesions.Add(sesion);
                cambiosHechos = sghContext.SaveChanges();

                if (cambiosHechos == 1)
                {
                    sesionGuardada = true;
                }

            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show(ex.ToString());
                log.Add(ex.ToString());
            }

            return sesionGuardada;
        }      

        public bool GuardarMateriaSesion(Materia_Sesion materia_Sesion)
        {
            int cambiosHechos;
            bool registroGuardado = false;
            try
            {

                sghContext.Materia_Sesion.Add(materia_Sesion);
                cambiosHechos = sghContext.SaveChanges();

                if (cambiosHechos == 1)
                {
                    registroGuardado = true;
                }

            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show(ex.ToString());
                log.Add(ex.ToString());
            }

            return registroGuardado;

        }
    }
}

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

        public Materia GetMateriaBySesion(string sesionID)
        {
            Materia materia = null;
                      
            Materia_Sesion materia_Sesion = sghContext.Materia_Sesion.Where(ms => ms.ID_Sesion == sesionID).FirstOrDefault();    
            if (materia_Sesion != null)
            {
                Profesor_Materia profesorMateria = sghContext.Profesor_Materia.Where(pm => pm.ID_Profesor_Materia == materia_Sesion.ID_Profesor_Materia).FirstOrDefault();
                if (profesorMateria != null)
                {
                    materia = sghContext.Materias.Where(m => m.NRC == profesorMateria.NRC_Materia).FirstOrDefault();
                }                
            }
                      

            return materia;
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
            List<Materia> lista = (from mat in sghContext.Materias
                                 join prof_mat in sghContext.Profesor_Materia
                                 on mat.NRC equals prof_mat.NRC_Materia
                                 select mat).ToList();

            return lista;
        }
    
    
    }
}

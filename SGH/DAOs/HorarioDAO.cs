﻿using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SGH.DAOs
{
    public class HorarioDAO
    {
        private SGHContext sghContext = new SGHContext();

        public List<Grupo> GetGrupos()
        {
            List<Grupo> grupos = sghContext.Grupoes.ToList();

            return grupos;
        }

        public List<Grupo> GetGruposConHorario()
        {
            List<Grupo> gruposConHorario = new List<Grupo>();
            List<Grupo> grupos = sghContext.Grupoes.ToList();

            foreach (Grupo grupo in grupos)
            {
                Sesion existeSesion = sghContext.Sesions.Where(sesion => sesion.ID_Grupo == grupo.ID).FirstOrDefault();
                if (existeSesion != null)
                {
                    gruposConHorario.Add(grupo);
                }

            }

            return gruposConHorario;
        }

        public List<Grupo> GetGruposSinHorario()
        {
            List<Grupo> gruposSinHorario = new List<Grupo>();
            List<Grupo> grupos = sghContext.Grupoes.ToList();

            foreach (Grupo grupo in grupos)
            {
                Sesion existeSesion = sghContext.Sesions.Where(sesion => sesion.ID_Grupo == grupo.ID).FirstOrDefault();
                if (existeSesion == null)
                {
                    gruposSinHorario.Add(grupo);
                }

            }

            return gruposSinHorario;
        }

        public string GetGrupoId(string letra, int semestre)
        {

            Grupo grupo = sghContext.Grupoes.Where(gr => gr.Letra == letra && gr.Semestre == semestre).FirstOrDefault();

            return grupo.ID;
        }

        public List<Sesion> GetSesionesByGrupo(string grupoId)
        {
            List<Sesion> sesiones = sghContext.Sesions.Where(sesion => sesion.ID_Grupo == grupoId).ToList();
            return sesiones;
        }

        public Materia GetMateriaBySesion(string sesionID)
        {
            Materia_Sesion materia_Sesion = sghContext.Materia_Sesion.Where(ms => ms.ID_Sesion == sesionID).FirstOrDefault();
            Materia materia = sghContext.Materias.Where(m => m.NRC == materia_Sesion.NRC_Materia).FirstOrDefault();
            return materia;
        }
    }
}

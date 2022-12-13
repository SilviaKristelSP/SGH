using FluentValidation;
using SGH.DAOs;
using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.Validaciones
{
    internal class GrupoValidator
    {
        public static bool verificarExistencia(Grupo grupo)
        {
            bool verificacionExitosa = true;
            List<Grupo> grupos = GrupoDAO.recuperarListadeGrupos();
            Console.WriteLine(grupos.Count);

            if(grupos != null)
            {
                foreach (Grupo g in grupos)
                {
                    if (grupo.Semestre == g.Semestre && grupo.Letra.Equals(g.Letra))
                    {
                        verificacionExitosa = false;
                    }
                }
            }
            Console.WriteLine(grupos.Count);
            return verificacionExitosa;
        }

        public static bool verificarGrupoparaEliminar(String idGrupo)
        {
            bool verificacionExitosa = false;

            List<Estudiante> estudiantes = GrupoDAO.recuperarEstudiantesporGrupo(idGrupo);

            if(estudiantes.Count <= 0)
            {
                verificacionExitosa=true;
            }

            return verificacionExitosa;
        }

        
    }
}

using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SGH.DAOs
{
    public class GrupoDAO
    {
        private SGHContext sghContext = new SGHContext();
        public Grupo recuperarGrupo(Estudiante miembro)
        {
            Grupo grupo = sghContext.Grupoes.Where(gr => gr.ID == miembro.ID_Grupo).FirstOrDefault();
            return grupo;
        }

        public List<Materia> recuperarMateriasDeUnGrupo(Grupo grupo)
        {
            List<Materia> materiasDelGrupo = (from materia in sghContext.Materias
                                              where materia.Semestre == grupo.Semestre
                                              select materia).ToList();
            return materiasDelGrupo;
        }
    }
}

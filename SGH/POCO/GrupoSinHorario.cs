using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.POCO
{
    class GrupoSinHorario
    {
        private string semestre;
        private string grupo;

        public string Semestre { get => semestre; set => semestre = value; }
        public string Grupo { get => grupo; set => grupo = value; }
        public GrupoSinHorario(string semestre, string grupo)
        {
            this.semestre = semestre;
            this.grupo = grupo;
        }
    }
}

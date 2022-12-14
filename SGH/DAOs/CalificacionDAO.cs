using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.DAOs
{
    public class CalificacionDAO
    {
        private SGHContext sghContext = new SGHContext();

        public Calificacion recuperarCalificacion(string nrcMateria, string matriculaEstudiante)
        {
            Calificacion calificacion = (from clf in sghContext.Calificacions
                                         where clf.NRC_Materia == nrcMateria && clf.Matricula_Estudiante == matriculaEstudiante
                                         select clf).FirstOrDefault();
            return calificacion;
        }
    }
}

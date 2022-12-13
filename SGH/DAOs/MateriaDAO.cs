using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.DAOs
{
    public class MateriaDAO
    {
        private SGHContext sghContext = new SGHContext();

        public Materia recuperarMateria(string nombreMateria)
        {
            Materia materia = (from mt in sghContext.Materias
                               where mt.Nombre == nombreMateria
                               select mt).FirstOrDefault();
            return materia;
        }
    }
}

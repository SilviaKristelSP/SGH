using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


    }
}

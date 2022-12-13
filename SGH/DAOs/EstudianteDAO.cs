using SGH.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGH.DAOs
{
    public class EstudianteDAO
    {
        private SGHContext sghContext = new SGHContext();

        public Estudiante recuperarEstudiante(Estudiante estudiante)
        {
            Persona estudianteRecuperado = (from persona in sghContext.Personas
                                            where persona.Curp == estudiante.Persona.Curp
                                            select persona).FirstOrDefault();

            estudiante = (from alumno in sghContext.Estudiantes
                          where alumno.ID_Persona == estudianteRecuperado.ID
                          select alumno).FirstOrDefault();


            return estudiante;
        }
    }
}

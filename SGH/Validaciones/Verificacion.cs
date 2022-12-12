using SGH.Calificaciones;
using SGH.Modelos;
using System;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace SGH.Validaciones
{

    public static class Verificacion
    {
        public static bool VerificarNombre(String nombre)
        {
            Regex regex = new Regex("[a-zA-Z]+('[a-zA-Z])?[a-zA-Z]*");
            
            foreach (string word in nombre.Split(' '))
            {
                if(!regex.IsMatch(word))
                    return false;
            }

            return true;
        }

        public static bool VerificarExistencia(Estudiante estudiante)
        {
            return true;
        }

        public static int VerificarDatos(Estudiante estudiante)
        {
            if (estudiante == null) 
            {
                return -3;
            }

            if (!VerificarNombre(estudiante.Persona.Nombre))
            {
                return -1;
            }
            if (!VerificarExistencia(estudiante)) 
            {
                return -2;
            }

            return 0;
        }
    }
}

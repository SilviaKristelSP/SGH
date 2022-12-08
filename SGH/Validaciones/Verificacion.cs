using SGH.Calificaciones;
using System;
using System.Text.RegularExpressions;

namespace SGH.Validaciones
{

    public static class Verificacion
    {
        public static bool VerificarNombre(String nombre)
        {
            Regex regex = new Regex("^[A-Za-z]+$");

            if (regex.IsMatch(nombre)){
                return true; }
            return false;
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

            if (!VerificarNombre(estudiante.nombre))
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

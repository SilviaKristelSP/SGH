using SGH.Calificaciones;
using System;
using System.Text.RegularExpressions;

namespace SGH.Util
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

        public static bool VerificarDatos(Estudiante estudiante)
        {
            if (VerificarNombre(estudiante.nombre) && VerificarExistencia(estudiante))
            {
                return true;
            }
            return false;
        }
    }
}

using SGH.Calificaciones;
using SGH.Modelos;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace SGH.Validaciones
{

    public  class Verificacion
    {
        private SGHContext sghContext = new SGHContext();
        public static bool VerificarNombre(Persona persona)
        {
            Regex regex = new Regex("[a-zA-Z]+('[a-zA-Z])?[a-zA-Z]*");

            if (!regex.IsMatch(persona.Nombre))
            {
                return false;
            }

            if (!regex.IsMatch(persona.ApellidoPaterno))
            {
                return false;
            }

            if (!regex.IsMatch(persona.ApellidoMaterno))
            {
                return false;
            }

            return true;
        }

        public static bool VerificarCURP(string curp)
        {
            Regex regex = new Regex("^[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}$");

            if (regex.IsMatch(curp))
            {
                return true;
            }

            return false;
        }

        public  bool VerificarExistencia(Estudiante estudiante)
        {
            bool existe = sghContext.Personas.Any(persona => persona.Curp == estudiante.Persona.Curp);
            return existe;
        }

        public int VerificarDatos(Estudiante estudiante)
        {
            if (estudiante == null) 
            {
                return -3;
            }

            if (!VerificarCURP(estudiante.Persona.Curp))
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

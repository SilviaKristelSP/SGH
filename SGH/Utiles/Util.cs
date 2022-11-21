using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;

namespace SGH.Utiles
{
    public class Util
    {
        public static DatosArchivo convertirPDFABites()
        {
            DatosArchivo datos = new DatosArchivo();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Archivos PDF (.pdf)|*.pdf";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == true)
            {
                byte[] archivoEnByte = null;
                datos.NombreArchivo = System.IO.Path.GetFileName(openFileDialog1.FileName);

                Stream miStream = openFileDialog1.OpenFile();
                using (MemoryStream ms = new MemoryStream())
                {
                    miStream.CopyTo(ms);
                    archivoEnByte = ms.ToArray();
                    datos.PDFEnByte = archivoEnByte;
                    return datos;
                }
            }
            return null;
        }

        public static void abrirArchivoPDF(byte[] archivoByte, string nombreArchivo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(path);
            string folder = path + "temp\\";
            string complete = path + "temp";
            string fullFilePath = folder + nombreArchivo;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            if (File.Exists(fullFilePath))
            {
                Console.WriteLine(fullFilePath);
                File.Delete(fullFilePath);
            }

            File.WriteAllBytes(fullFilePath, archivoByte);
            Process.Start(fullFilePath);
        }

        public static string obtenerNombreSinEspacios(Persona xPersona)
        {
            return xPersona.Nombre + xPersona.ApellidoPaterno + xPersona.ApellidoMaterno;
        }
    }
}

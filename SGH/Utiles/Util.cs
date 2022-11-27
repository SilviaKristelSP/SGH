using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;
using System.Windows.Media.Imaging;

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

        public static DatosArchivo convertirImgABitesYBitMap()
        {
            OpenFileDialog abrir = new OpenFileDialog();
            DatosArchivo datos = new DatosArchivo();
            abrir.InitialDirectory = "c:\\";
            abrir.Filter = "Archivos jpg (.jpg)|*.jpg|Archivos png (.png)|*.png";
            abrir.FilterIndex = 1;
            abrir.RestoreDirectory = true;

            if (abrir.ShowDialog() == true)
            {
                datos.NombreArchivo = System.IO.Path.GetFileName(abrir.FileName);
                byte[] file = null;
                Stream myStream = abrir.OpenFile();
                using (MemoryStream ms = new MemoryStream())
                {
                    myStream.CopyTo(ms);
                    file = ms.ToArray();
                    datos.ImgEnByte = file;
                }
                Uri imgUri = new Uri(abrir.FileName);
                datos.ImagenBitMap = new BitmapImage(imgUri);
                return datos;
            }
            return null;
        }

        public static string generarID(int tamanioID)
        {
            string id = "";
            Random random = new Random();

            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            
            id = new string(Enumerable.Repeat(caracteres, tamanioID)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return id;
        }
    }
}


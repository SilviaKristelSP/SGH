using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SGH.Utiles
{
    public class DatosArchivo
    {
        public byte[] PDFEnByte  {set; get;}
        public byte[] ImgEnByte {set; get;}
        public string NombreArchivo { set; get;}
        public BitmapImage ImagenBitMap { set; get; }
    }
}

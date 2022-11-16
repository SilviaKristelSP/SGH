using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServidorSGH.Contratos
{
    [DataContract]
    public class AdministradorContract
    {
        [DataMember]
        public string RFC { get; set; }
        [DataMember]
        public string NombreCompleto { get; set; }
        [DataMember]
        public string Contrasenia { get; set; }
        [DataMember]
        public string Telefono { get; set; }
        [DataMember]
        public string Correo { get; set; }
        [DataMember]
        public string Rol { get; set; }
        [DataMember]
        public string Clave_Escuela { get; set; }


    }
}
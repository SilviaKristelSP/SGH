//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGH.Modelos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estudiante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Estudiante()
        {
            this.Calificacions = new HashSet<Calificacion>();
        }
    
        public string Matricula { get; set; }
        public string NumSeguroSocial { get; set; }
        public byte[] DocCurpTutor { get; set; }
        public byte[] DocCertificadoSecundaria { get; set; }
        public byte[] DocCartaBuenaConducta { get; set; }
        public string ID_Persona { get; set; }
        public string ID_Grupo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calificacion> Calificacions { get; set; }
        public virtual Grupo Grupo { get; set; }
        public virtual Persona Persona { get; set; }
    }
}

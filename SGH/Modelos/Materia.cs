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
    
    public partial class Materia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materia()
        {
            this.Calificacions = new HashSet<Calificacion>();
            this.Profesor_Materia = new HashSet<Profesor_Materia>();
        }
    
        public string NRC { get; set; }
        public string Nombre { get; set; }
        public int NumSesiones { get; set; }
        public int Semestre { get; set; }
        public string Color { get; set; }

    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calificacion> Calificacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profesor_Materia> Profesor_Materia { get; set; }
    }
}

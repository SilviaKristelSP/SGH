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
    
    public partial class Sesion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sesion()
        {
            this.Materia_Sesion = new HashSet<Materia_Sesion>();
        }
    
        public string ID { get; set; }
        public string DiaSemana { get; set; }
        public string HoraFinal { get; set; }
        public string HoraInicio { get; set; }
        public string ID_Grupo { get; set; }
    
        public virtual Grupo Grupo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Materia_Sesion> Materia_Sesion { get; set; }
    }
}

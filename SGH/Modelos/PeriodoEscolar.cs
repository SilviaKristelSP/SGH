//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGH.Modelos
{
    using System;
    using System.Collections.Generic;
    
    public partial class PeriodoEscolar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PeriodoEscolar()
        {
            this.PeriodoEscolar_Semestre = new HashSet<PeriodoEscolar_Semestre>();
        }
    
        public string ID { get; set; }
        public int AnioFinal { get; set; }
        public int AnioInicio { get; set; }
        public int MesFinal { get; set; }
        public int MesInicio { get; set; }
        public string Clave_Escuela { get; set; }
    
        public virtual Escuela Escuela { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PeriodoEscolar_Semestre> PeriodoEscolar_Semestre { get; set; }
    }
}

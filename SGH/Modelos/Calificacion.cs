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
    
    public partial class Calificacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Calificacion()
        {
            this.Calificacion_Parcial = new HashSet<Calificacion_Parcial>();
        }
    
        public string ID { get; set; }
        public int CantidadParciales { get; set; }
        public decimal EvalFinal { get; set; }
        public decimal PromedioParcial { get; set; }
        public decimal PromedioFinal { get; set; }
        public string NRC_Materia { get; set; }
        public string Matricula_Estudiante { get; set; }
    
        public virtual Estudiante Estudiante { get; set; }
        public virtual Materia Materia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calificacion_Parcial> Calificacion_Parcial { get; set; }
    }
}

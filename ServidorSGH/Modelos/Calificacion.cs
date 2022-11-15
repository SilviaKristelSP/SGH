//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServidorSGH.Modelos
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

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
    
    public partial class Calificacion_Parcial
    {
        public string ID_Calificacion_Parcial { get; set; }
        public string ID_Cali { get; set; }
        public decimal CaliParcial { get; set; }
    
        public virtual Calificacion Calificacion { get; set; }
    }
}

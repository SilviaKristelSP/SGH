using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGH.Modelos;
using SGH.Utiles;

namespace SGH.DAOs
{
    public class MateriaDAO
    {
        public static List<Materia> recuperarMaterias()
        {
            List<Materia> materias = null;
            IEnumerable<Materia> lista = null;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    lista = bd.Materias;
                    materias = lista.ToList();
                }
            }
            catch (Exception ex)
            {
                lista = null;
                materias = null;
            }
            return materias;
        }

        public static bool borrarMateriasProfesor(String idprofesor)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    List<Profesor_Materia> impartidas = (from mp in bd.Profesor_Materia
                                                 where mp.ID_Profesor_Materia == idprofesor
                                                 select mp).ToList();
                    int c = 0;
                    for (int i = 0; i < impartidas.Count; i++)
                    {
                        bd.Profesor_Materia.Remove(impartidas[i]);
                        c += bd.SaveChanges();
                    }

                    if(c <= 0)
                    {
                        ejecucionExitosa = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static bool asignarMateriasProfesor(List<Profesor_Materia> impartidas)
        {
            bool ejecucionExitosa = true;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    foreach (Profesor_Materia pf in impartidas)
                    {
                        var materia = new Profesor_Materia();
                        materia.NRC_Materia = pf.NRC_Materia;
                        materia.RFC_Profesor = pf.RFC_Profesor;
                        materia.ID_Profesor_Materia = Util.generarID(50);
                        bd.Profesor_Materia.Add(materia);
                    }
                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ejecucionExitosa = false;
            }
            return ejecucionExitosa;
        }

        public static int obtenerUltimoSemestre()
        {
            int sem = 0;
            try
            {
                using (SGHContext bd = new SGHContext())
                {
                    sem = bd.Materias.Max(x => x.Semestre);
                }
            }
            catch (Exception ex)
            {
                sem = 0;
            }
            return sem;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyushik_TournMan_Common.Models
{
    public class Technique
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name {get; set;}

        public double Weight { get; set; }

        //Exclusionary techniques have no weight
        public bool Exclusionary { get; set; }

        public virtual List<Technique> SubTechniques { get; set; }

    }
}

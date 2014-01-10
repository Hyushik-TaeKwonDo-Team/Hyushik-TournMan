using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyushik_TournMan_Common.Models
{
    //for LAZINESS! - I mean, "Efficiency"
    [Serializable]
    public class Technique
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name {get; set;}

        public double Weight { get; set; }

        //exists only for scoring purposes, DO NOT PERSIST
        [NotMapped]
        public bool Selected { get; set; } 

        //toggleable techs have no subtechniques
        public bool Toggleable { get; set; }

        //exists for querying
        public virtual Technique Parent { get; set; }

        //please no recursion . . .
        public virtual List<Technique> SubTechniques { get; set; }

        public Technique()
        {
            SubTechniques = new List<Technique>();
        }

        public bool IsLeaf()
        {
            return 0 == SubTechniques.Count;
        }

        public void AddSubTechnique(Technique technique)
        {   if(!SubTechniques.Contains(technique)){
                SubTechniques.Add(technique);
            }
            technique.Parent = this;
        }

    }
}

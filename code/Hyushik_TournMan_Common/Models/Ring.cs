using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Models
{
    public class Ring
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual List<Participant> SelectedParticipants { get; set; }

        public bool BreakingResultsPublic { get; set; }
        public bool WeaponResultsPublic { get; set; }
        public bool FormResultsPublic { get; set; }
        public bool SparringResultsPublic { get; set; }

        public Ring()
        {
            BreakingResultsPublic = false;
            WeaponResultsPublic = false;
            FormResultsPublic = false;
            SparringResultsPublic = false;
            SelectedParticipants = new List<Participant>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Models
{
    public class SparringResult
    {
        [Key]
        public long Id { get; set; }
        public Ring Ring { get; set; }

        public Participant Participant1 { get; set; }
        public Participant Participant2 { get; set; }
        public bool Partipant1IsVictor { get; set; }

        public int RoundNumber { get; set; }

        [NotMapped]
        public Participant Victor {
            get {
                return Partipant1IsVictor ? Participant1 : Participant2;
                }
        }

        [NotMapped]
        public Participant Defeated
        {
            get
            {
                return !Partipant1IsVictor ? Participant1 : Participant2;
            }
        }
    }
}

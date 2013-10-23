using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyushik_TournMan_Common.Models
{
    public class ParticipantBoardSizeCount
    {
        [Key]
        public string BoardSize { get; set; }
        [Key]
        public virtual Participant Participant { get; set; }

        public int Count { get; set; }
    }
}

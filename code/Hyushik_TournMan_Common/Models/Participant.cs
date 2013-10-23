using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyushik_TournMan_Common.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        public string Name { get; set; }
    }
}

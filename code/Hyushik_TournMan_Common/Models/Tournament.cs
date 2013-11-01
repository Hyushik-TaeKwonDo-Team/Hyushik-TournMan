using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Hyushik_TournMan_Common.Models
{
    public class Tournament
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DisplayName("Tournament Name")]
        public string Name { get; set; }
        [DisplayName("Participants")]
        public virtual List<Participant> Participants { get; set; }

        public Tournament()
        {
            Participants = new List<Participant>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Models
{
    public class ParticipantSelection
    {
        public List<Participant> Participants { get; set; }
        public long ParticipantId { get; set; }

        public bool CreateNewParticipant { get; set; }

        public long RingId { get; set; }
        public long TournId { get; set; }

        public string NewParticipantName { get; set; }

        public ParticipantSelection()
        {
            NewParticipantName = String.Empty;
            ParticipantId = -1;
            RingId = -1;
        }
    }
}

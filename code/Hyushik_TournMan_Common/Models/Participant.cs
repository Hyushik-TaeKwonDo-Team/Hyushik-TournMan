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
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }

        public string InstructorName { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolCity { get; set; }
        public string SchoolState { get; set; }
        public string SchoolZip { get; set; }
        public string SchoolPhone { get; set; }
        public string SchoolEmail { get; set; }

        public string Rank { get; set; }
        public string Age { get; set; }
        public string Weight { get; set; }

        public bool Weapons { get; set; }
        public bool Breaking { get; set; }
        public bool Forms { get; set; }
        public bool PointSparring { get; set; }
        public bool OlympicSparring { get; set; }

    }
}

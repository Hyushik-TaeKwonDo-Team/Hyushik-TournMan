using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyushik_TournMan_Common.Models
{
    public class BoardSizeCount
    {
        [Key, Column(Order=0) ]
        public string BoardSize { get; set; }
        [Key, Column(Order=1)]
        public long ParticipantId { get; set; }
        public int Count { get; set; }
    }
}
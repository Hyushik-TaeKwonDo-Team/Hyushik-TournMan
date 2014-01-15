using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Models
{
    public class BreakingResult
    {
        [Key]
        public long Id { get; set; }
        public Participant Participant { get; set; }
        public Tournament Tournament { get; set; }

        //the list is a 0 indexed list of stations
        //the techniqueValue has the full technique name and final value of the technique
        //adds more data to the db but is cleaner in code than making a weird reverse tree traversal
        //also allows for auditing?  I guess?
        public virtual List<TechniqueValue> Stations { get; set; }
        public BreakingJudgeScore JudgeScores { get; set; }

    }

    public class BreakingJudgeScore
    {
        [Key]
        public long Id { get; set; }
        //has to be hand set, because db split
        public long Judge_UserId { get; set; }
        public int SubjectiveScore { get; set; }
    }

    public class TechniqueValue{
        [Key]
        public long Id {get; set;}
        //i.e "Spinning Flip Kick"
        public string Name {get; set;}
        public double Value { get; set; }
    }
}

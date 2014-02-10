using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Models
{
    public abstract class WeaponOrFormResult{

        public virtual Tournament Tournament { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual List<WeaponAndFormJudgeScore> JudgeScores {get; set;}

        protected void Initialize()
        {
            JudgeScores = new List<WeaponAndFormJudgeScore>();
        }
    }

    public class WeaponAndFormJudgeScore{
        [Key]
        public long Id { get; set; }
        //has to be hand set, because db split
        public long Judge_UserId { get; set; }
        public int Score { get; set; }
    }

    public class WeaponResult : WeaponOrFormResult
    {
        [Key]
        public long Id { get; set; }
        public WeaponResult()
        {
            Initialize();
        }
    }

    public class FormResult : WeaponOrFormResult
    {
        [Key]
        public long Id { get; set; }
        public FormResult()
        {
            Initialize();
        }
    }
}

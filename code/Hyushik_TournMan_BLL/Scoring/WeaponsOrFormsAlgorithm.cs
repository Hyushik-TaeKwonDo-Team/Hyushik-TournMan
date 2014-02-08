using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Scoring
{
    public class WeaponsOrFormsAlgorithm
    {
        public double CalculateScore(WeaponOrFormResult result)
        {
            return result.JudgeScores.Sum(js=>js.Score);
        }
    }
}

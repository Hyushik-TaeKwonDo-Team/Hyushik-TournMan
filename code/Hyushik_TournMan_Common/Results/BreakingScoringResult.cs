using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Results
{
    public class BreakingScoringResult : OperationResult
    {
        public double Score { get; set; }
        public List<Station> Stations { get; set; }
    }
}

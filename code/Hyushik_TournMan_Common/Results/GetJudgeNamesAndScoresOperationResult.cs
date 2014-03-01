using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Results
{
    public class GetJudgeNamesAndScoresOperationResult : OperationResult
    {
        public Dictionary<long, string> JudgeIdToName { get; set; }
        public Dictionary<long, int> JudgeIdToScore { get; set; }

        public GetJudgeNamesAndScoresOperationResult()
        {
            JudgeIdToName = new Dictionary<long, string>();
            JudgeIdToScore = new Dictionary<long, int>();
        }
    }
}

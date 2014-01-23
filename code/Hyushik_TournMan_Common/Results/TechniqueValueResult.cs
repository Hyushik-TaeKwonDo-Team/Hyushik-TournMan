using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Results
{
    public class TechniqueValueResult : OperationResult
    {
        public TechniqueValue TechniqueValue { get; set; }
        public bool HasTechniqueValue { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Results
{
    public class OperationResult
    {
        public bool WasSuccessful {get; set;}
        public string Message { get; set; }

        public OperationResult(){
            WasSuccessful = false;
        }
    }
}

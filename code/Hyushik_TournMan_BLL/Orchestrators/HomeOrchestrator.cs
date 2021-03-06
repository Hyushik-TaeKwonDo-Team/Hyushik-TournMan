﻿using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class HomeOrchestrator: BaseOrchestrator, IHomeOrchestrator
    {
        public string getQrcode(long partId)
        {
            var gen = new QrCheckin.QrGen();
            return gen.getQrCodeFromLong(partId);
        }
    }
}

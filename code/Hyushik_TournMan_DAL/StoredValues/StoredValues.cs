using Hyushik_TournMan_Common.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_DAL.StoredValues
{
    public static class StoredValues
    {

        public static double StationFalloffProportion
        {
            get
            {

                return Double.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.StationFalloffProportion]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.StationFalloffProportion] = value.ToString();
            }
        }
    }
}

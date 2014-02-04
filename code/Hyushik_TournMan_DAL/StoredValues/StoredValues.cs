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

        public static int MaxBreakingStationCount
        {
            get
            {

                return Int32.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.MaxBreakingStationCount]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.MaxBreakingStationCount] = value.ToString();
            }
        }

        public static List<double> PossibleBoardDepths
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.PossibleBoardDepths].Split(',').Select(d=>double.Parse(d.Trim())).ToList();
            }
            set
            {
                var stringResult = String.Empty;
                foreach(var val in value){
                    if(stringResult!=string.Empty){
                        stringResult += ",";
                    }
                    stringResult += val.ToString();
                }
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.PossibleBoardDepths] = stringResult;
            }
        }

        public static List<double> PossibleBoardWidths
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.PossibleBoardWidths].Split(',').Select(d => double.Parse(d.Trim())).ToList();
            }
            set
            {
                var stringResult = String.Empty;
                foreach (var val in value)
                {
                    if (stringResult != string.Empty)
                    {
                        stringResult += ",";
                    }
                    stringResult += val.ToString();
                }
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.PossibleBoardWidths] = stringResult;
            }
        }
    }
}

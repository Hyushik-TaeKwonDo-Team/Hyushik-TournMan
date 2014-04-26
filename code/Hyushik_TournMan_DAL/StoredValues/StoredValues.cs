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
        public static double BreakingBoardExponent
        {
            get
            {
                return Double.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingBoardExponent]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingBoardExponent] = value.ToString();
            }
        }

        public static int BreakingMaxStationCount
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaxStationCount]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaxStationCount] = value.ToString();
            }
        }

        public static int BreakingMaximumBoards
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaximumBoards]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaximumBoards] = value.ToString();
            }
        }

        public static int BreakingMaximumAttempts
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaximumAttempts]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaximumAttempts] = value.ToString();
            }
        }

        public static double BreakingAttemptDecayRate
        {
            get
            {
                return Double.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingAttemptDecayRate]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingAttemptDecayRate] = value.ToString();
            }
        }

        public static double BreakingSpacerPenalty
        {
            get
            {
                return Double.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingSpacerPenalty]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingSpacerPenalty] = value.ToString();
            }
        }

        public static double BreakingPowerHoldPenalty
        {
            get
            {
                return Double.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingPowerHoldPenalty]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingPowerHoldPenalty] = value.ToString();
            }
        }

        public static double BreakingJudgeWeight
        {
            get
            {
                return Double.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingJudgeWeight]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingJudgeWeight] = value.ToString();
            }
        }

        public static int BreakingMaxScore
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaxScore]);
            }
            set
            {
                ConfigurationManager.AppSettings[Constants.AppSettingsKeys.BreakingMaxScore] = value.ToString();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_Common.Constants
{
    public static class Constants
    {

        public static class AppSettingsKeys
        {
            public const string BreakingBoardExponent = "breakingBoardExponent";
            public const string BreakingMaxStationCount = "breakingMaxStationCount";
            public const string BreakingMaximumBoards = "breakingMaximumBoards";
            public const string BreakingMaximumAttempts = "breakingMaximumAttempts";
            public const string BreakingAttemptDecayRate = "breakingAttemptDecayRate";
            public const string BreakingSpacerPenalty = "breakingSpacerPenalty";
            public const string BreakingPowerHoldPenalty = "breakingPowerHoldPenalty";

            public const string PossibleBoardWidths = "possibleBoardWidths";
            public const string PossibleBoardDepths = "possibleBoardDepths";
        }

        public static class CSV
        {
            public const int PRE_BOARD_SIZE_COLUMN_COUNT = 23;
            public const string YES_STRING = "Yes";
            public const string NO_STRING = "No";
        }

        public static class Roles
        {
            public const string ADMINISTRATOR_ROLE = "Administrator";
            public const string JUDGE_ROLE = "Judge";
        }

        public static class DefaultAdmin
        {
            public const string USERNAME = "admin";
            public const string PASSWORD = "password";
        }

        
    }
}

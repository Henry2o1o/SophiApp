﻿using SophiApp.Commons;
using SophiApp.Helpers;
using SophiApp.Interfaces;

namespace SophiApp.Conditions
{
    internal class OsBuildVersion : ICondition
    {
        private const uint MAX_SUPPORT_BUILD = 19044;
        private const uint MIN_SUPPORT_BUILD = 19041;
        public bool Result { get; set; }
        public string Tag { get; set; } = Tags.ConditionOsBuildVersion;

        public bool Invoke()
        {
            var build = OsHelper.GetBuild();
            return Result = build >= MIN_SUPPORT_BUILD && build <= MAX_SUPPORT_BUILD;
        }
    }
}
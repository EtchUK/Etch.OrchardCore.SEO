using System;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.RobotsTxt.Models
{
    public static class RobotsTxtModes
    {
        public const int NotDefined = 0;
        public const int AllowAllPages = 1;
        public const int DisallowAllPages = 2;
        public const int Custom = 3;
        public const int Recommended = 4;

        public readonly static string AllowAllPagesOutput = $"User-agent: *{Environment.NewLine}Disallow:";
        public readonly static string DisallowAllPagesOutput = $"User-agent: *{Environment.NewLine}Disallow: /";
        public readonly static string RecommendedOutput = $"User-agent: *{Environment.NewLine}Disallow: /login{Environment.NewLine}Disallow: /admin/";

        private static IDictionary<int, string> Outputs = new Dictionary<int, string>
        {
            { NotDefined, string.Empty },
            { AllowAllPages, AllowAllPagesOutput},
            { DisallowAllPages, DisallowAllPagesOutput},
            { Recommended, RecommendedOutput}
        };

        public static string GetOutput(RobotsTxtSettings settings)
        {
            if (settings.Mode == Custom)
            {
                return settings.CustomContent;
            }

            return Outputs[settings.Mode];
        }
    }
}

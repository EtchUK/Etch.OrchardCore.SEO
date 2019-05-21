using System;
using System.Collections.Generic;

namespace Etch.OrchardCore.SEO.RobotsTxt.Models
{
    public class RobotsTxtModes
    {
        public const int NotDefined = 0;
        public const int AllowAllPages = 1;
        public const int DisallowAllPages = 2;
        public const int Custom = 3;

        public static string AllowAllPagesOutput = $"User-agent: *{Environment.NewLine}Disallow:";
        public static string DisallowAllPagesOutput = $"User-agent: *{Environment.NewLine}Disallow: /";

        private static IDictionary<int, string> Outputs = new Dictionary<int, string>
        {
            { NotDefined, string.Empty },
            { AllowAllPages, AllowAllPagesOutput},
            { DisallowAllPages, DisallowAllPagesOutput}
        };

        public static string GetOutput(RobotsTxtSettings settings)
        {
            if (settings.Mode == Custom)
                return settings.CustomContent;

            return Outputs[settings.Mode];
        }
    }
}

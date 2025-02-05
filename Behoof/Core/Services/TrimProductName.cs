using System.Text.RegularExpressions;

namespace Behoof.Core.Services
{
    public class TrimProductName
    {
        public string TrimName(string name)
        {
            if (name == null || name == "")
                return "";
            var res = Regex.Match(name, @"^Смартфон\s[\w\s]+(?:\s\d+\/\d+[Gg]{1}[Bb]|\s\d+[Gg]{1}[Bb])");
            var res2 = res.Value;
            return res2;
        }
    }
}

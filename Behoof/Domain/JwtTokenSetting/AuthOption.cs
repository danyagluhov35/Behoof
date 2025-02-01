using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Behoof.Domain.JwtTokenSetting
{
    public class AuthOption
    {
        private const string Key = "1b>Z6kh~+O9[L3ic>>GCNxZ[X>B.71,])xJV2!rM?=8dwJj[W3(K~I,8=*8Cg/i5~TF+C\\L+b9r?_8Ey/o2#Zk$?Fa)M:<+JY~!w";
        public static string Issuer = "Behhof";
        public static string Audience = "UserForBehoof";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}

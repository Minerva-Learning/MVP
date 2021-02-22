using Lean.Security;

namespace Lean.Authorization.Users.Profile.Dto
{
    public class GetPasswordComplexitySettingOutput
    {
        public PasswordComplexitySetting Setting { get; set; }
    }
}

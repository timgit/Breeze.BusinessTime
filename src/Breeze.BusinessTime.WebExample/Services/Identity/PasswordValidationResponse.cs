using System.Collections.Generic;

namespace Breeze.BusinessTime.WebExample.Services.Identity
{
    public class PasswordValidationResponse
    {
        public bool IsValid { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
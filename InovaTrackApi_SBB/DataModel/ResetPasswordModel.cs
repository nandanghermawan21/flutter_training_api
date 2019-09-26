using System.ComponentModel.DataAnnotations;

namespace InovaTrackApi_SBB.DataModel
{
    public class ResetPasswordModel
    {
        [Required]
        public string ResetCode { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}

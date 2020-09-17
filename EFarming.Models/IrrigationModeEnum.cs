using System.ComponentModel.DataAnnotations;

namespace EFarming.Models
{
    public enum IrrigationModeEnum
    {
        [Display(Name = "Automatic")]
        Automatic = 1,
        [Display(Name = "Manual")]
        Manual = 2
    }
}

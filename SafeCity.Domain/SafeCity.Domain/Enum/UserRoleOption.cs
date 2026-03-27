using System.ComponentModel.DataAnnotations;

public enum UserRoleOption
{
    [Display(Name = "Citizen")]
    Citizen = 1,

    [Display(Name = "Police Officer")]
    Police = 2,

    [Display(Name = "Fire Fighter")]
    Fire_Fighter = 3,

    [Display(Name = "Emergency Dispatcher")]
    Emergency_Dispatcher = 4,

    [Display(Name = "Compliance Officer")]
    Compliance_Officer = 5,

    [Display(Name = "City Administrator")]
    City_Administrator = 6
}
using System;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models.Dtos;

public class UserLoginDto
{
    [Required(ErrorMessage = "The username field is required")]  
    public string? Username { get; set; }  

    [Required(ErrorMessage = "The password field is required")]  
    public string? Password { get; set; }
}

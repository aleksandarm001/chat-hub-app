namespace backend.Models.Auth;

public class RegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username {get; set; }
}
namespace backend.Models;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Role { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    long GenerateUniqueId()
    {
        byte[] guidBytes = Guid.NewGuid().ToByteArray();
        return BitConverter.ToInt64(guidBytes, 0);
    }
    public User(){}
    public User(string username, string email, string hashedPassword, string firstname, string lastname)
    {
        Id = GenerateUniqueId();  
        Username = username;
        Email = email;
        HashedPassword = hashedPassword;
        Role = "user";
        Firstname = firstname;
        Lastname = lastname;
    }
}
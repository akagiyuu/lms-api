namespace Identity.Service.Models;
public class User
{
    public int      UserId              { get; set; }
    public string   Username            { get; set; } = null!;
    public string   PasswordHash        { get; set; } = null!;
    public string   Role                { get; set; } = null!;
    public string?  RefreshToken        { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}

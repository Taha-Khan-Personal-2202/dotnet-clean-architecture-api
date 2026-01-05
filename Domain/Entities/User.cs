namespace Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; }

    private User() { }

    public User(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("User name is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        Name = name;
        Email = email;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}

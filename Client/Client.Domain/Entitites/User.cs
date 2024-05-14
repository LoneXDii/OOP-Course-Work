namespace Client.Domain.Entitites;

public class User : Entity, ICloneable
{
    public string AuthorizationToken { get; set; } = "";
    public string Name { get; set; } = "";
    public string Login { get; set; } = "";

    public object Clone()
    {
        return MemberwiseClone();
    }
}

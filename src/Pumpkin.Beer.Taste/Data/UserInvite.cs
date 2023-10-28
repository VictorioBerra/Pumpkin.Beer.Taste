namespace Pumpkin.Beer.Taste.Data;

public class UserInvite : AuditableEntity
{
    public int Id { get; set; }

    public int BlindId { get; set; }

    public Blind Blind { get; set; } = null!;
}

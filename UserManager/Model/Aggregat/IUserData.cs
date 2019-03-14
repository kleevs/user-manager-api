namespace UserManager.Model
{
    public interface IUserData : IUser, IBirthData, INameable, IActivable, IHerarchy<IUser>, IEmailable
    {
    }
}

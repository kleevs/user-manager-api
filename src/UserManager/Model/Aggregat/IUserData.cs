namespace UserManager.Model
{
    public interface IUserData : IUserEmailable, IBirthData, INameable, IActivable, IHerarchy<IUser>
    {
    }
}

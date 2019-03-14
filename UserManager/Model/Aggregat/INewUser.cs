namespace UserManager.Model
{
    public interface INewUser : IUser, IBirthData, INewAuthenticable, INameable, IActivable, IHerarchy<IUser>
    {
    }
}

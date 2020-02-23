namespace UserManager.Model
{
    public interface IUserFull : IUserLoginFilterable, INewAuthenticable, IUserData, IUserEmailable, IBirthData, INameable, IActivable, IHerarchy<IUser>
    {
    }
}

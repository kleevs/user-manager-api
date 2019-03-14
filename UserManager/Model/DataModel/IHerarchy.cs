namespace UserManager.Model
{
    public interface IHerarchy<T>
    {
        T ParentUser { get; }
    }
}

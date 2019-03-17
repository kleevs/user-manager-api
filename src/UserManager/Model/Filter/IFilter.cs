namespace UserManager.Model
{
    public interface IFilterable : IEmailable, IIdentifiable
    {
    }

    public interface IFilter : IFilterable
    {
        bool? IsActive { get; }
    }
}

namespace Infrastructure.DomainBaseClasses
{
    public interface IQueryRunner
    {
        TResult ExecuteQuery<TResult>(Query<TResult> query) where TResult : new();
    }
}
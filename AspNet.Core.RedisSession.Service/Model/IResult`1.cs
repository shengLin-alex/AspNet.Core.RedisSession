namespace AspNet.Core.RedisSession.Service.Model
{
    public interface IResult<TData> : IResult
    {
        TData Data { get; set; }
    }
}
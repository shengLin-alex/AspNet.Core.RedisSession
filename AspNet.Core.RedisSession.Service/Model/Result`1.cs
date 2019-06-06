namespace AspNet.Core.RedisSession.Service.Model
{
    public class Result<TData> : Result, IResult<TData>
    {
        public TData Data { get; set; }
    }
}
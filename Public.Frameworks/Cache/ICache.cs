namespace Public.Frameworks.Cache
{
    public interface ICache<T, U, O>
        where T : notnull
        where U : notnull
        where O : CacheOptions
    {
        O Options { get; }
        int Count { get; }

        bool Contains(T key);

        Task<U> GetOrAdd(T key, Func<T, Task<U>> factory, int semaphoreWaitItemOutMilli);
    }
}
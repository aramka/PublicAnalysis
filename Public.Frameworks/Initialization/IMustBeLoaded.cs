namespace Public.Frameworks.Initialization
{
    public interface IMustBeLoaded
    {
        Task Load(bool reload = false);
    }
}

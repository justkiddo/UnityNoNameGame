namespace root
{
    public interface IUnityLocalization
    {
        string Translate(string key, params object[] args);
    }
}
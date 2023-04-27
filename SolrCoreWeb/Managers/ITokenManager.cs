namespace SolrCoreWeb.Managers
{
    public interface ITokenManager
    {
        string CreateToken(string userId);
    }
}
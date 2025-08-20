namespace esok.api.Interfaces.Services
{
    public interface IFacebookService 
    { 
        Task<string> AddPost(string message, string address);
    }
}

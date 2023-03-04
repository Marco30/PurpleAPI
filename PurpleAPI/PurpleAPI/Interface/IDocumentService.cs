using PurpleAPI.Model;

namespace PurpleAPI.Interface
{
    public interface IDocumentService
    {
        Task<UserDocument> GetDocumentAsync(string customerNumber);
    }
}

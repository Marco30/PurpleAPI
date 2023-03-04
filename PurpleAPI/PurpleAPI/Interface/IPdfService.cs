namespace PurpleAPI.Interface
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfAsync(string documentText);
    }
}

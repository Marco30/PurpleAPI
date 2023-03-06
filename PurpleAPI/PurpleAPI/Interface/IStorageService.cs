namespace PurpleAPI.Interface
{
    public interface IStorageService
    {
        Task<string> SavePdfAsync(string userId, byte[] pdfBytes);
    }
}

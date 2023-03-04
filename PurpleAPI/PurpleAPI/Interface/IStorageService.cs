namespace PurpleAPI.Interface
{
    public interface IStorageService
    {
        Task<string> SavePdfAsync(byte[] pdfBytes);
    }
}

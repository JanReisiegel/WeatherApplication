namespace Weather.Services
{
    public interface IJsonFileService<T> where T : class
    {
        Task<List<T>> ReadFromFileAsync();
        Task WriteToFileAsync(List<T> data);
    }
}

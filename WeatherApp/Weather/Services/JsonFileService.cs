using Newtonsoft.Json;
using Weather.Models;

namespace Weather.Services
{
    public class JsonFileService<T> : IJsonFileService<T> where T : class
    {
        private readonly string _path;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public JsonFileService(string path)
        {
            _path = path;
        }
        public async Task<List<T>> ReadFromFileAsync()
        {
            if (!File.Exists(_path))
            {
                return new List<T>();
            }
            await _lock.WaitAsync();
            try
            {
                using (var reader = new StreamReader(_path))
                {
                    string json = await reader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task WriteToFileAsync(List<T> data)
        {
            await _lock.WaitAsync();
            try
            {
                string json = JsonConvert.SerializeObject(data);
                using (var writer = new StreamWriter(_path))
                {
                    await writer.WriteAsync(json);
                }
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}

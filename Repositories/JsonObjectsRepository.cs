
using System.Text.Json;

namespace backend.Repositories
{
    internal class JsonObjectsRepository<T>(string jsonPath) where T : class
    {
        private readonly string _jsonPath = jsonPath;

        private static readonly Mutex _semaphore = new();

        public void Acquire() => _semaphore.WaitOne();
        public void Release() => _semaphore.ReleaseMutex();

        public List<T> Load()
        {
            return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(_jsonPath))
                   ?? throw new JsonException($"Archivo con valor null '{_jsonPath}' cuando no se esperaba ");
        }

        public void Save(List<T> objects)
        {
            File.WriteAllText(_jsonPath, JsonSerializer.Serialize(objects));
        }
        protected bool RemoveByPredicate(Predicate<T> pred)
        {
            List<T> objects = Load();
            if (objects.RemoveAll(pred) != 0)
            {
                Save(objects);
                return true;
            }
            return false;
        }
    }
}
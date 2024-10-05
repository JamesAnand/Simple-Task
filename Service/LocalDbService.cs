using SQLite;
using Task.Management.Entities;

namespace Task.Management.Service
{
    public class LocalDbService : ILocalDbService
    {
        private const string DbName = @"taskslocal.db3";
        private readonly SQLiteAsyncConnection _connection;
        public LocalDbService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
            _connection = new SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<TaskEntity>();
        }

        public async System.Threading.Tasks.Task DeleteAsync(TaskEntity entity)
        {
            if(entity == null || await GetAsync(entity.TaskId) == null)
            {
                return;
            }
            await _connection.DeleteAsync(entity);
        }

        public async Task<List<TaskEntity>> GetAllAsync()
        {
            var tableInfo = await _connection.GetTableInfoAsync("Tasks");
            if(tableInfo.Count == 0)
            {
                return new();
            }
            return await _connection?.Table<TaskEntity>()?.ToListAsync() ?? new();
        }

        private async Task<TaskEntity> GetAsync(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            return await _connection.Table<TaskEntity>().Where(x => x.TaskId == id)?.FirstOrDefaultAsync();
        }

        public async System.Threading.Tasks.Task SaveAsync(TaskEntity entity)
        {
            if(entity == null || string.IsNullOrWhiteSpace(entity.TaskId))
            {
                return;
            }
            var task = await GetAsync(entity.TaskId);
            if(task == null)
            {
                await _connection.InsertAsync(entity);
            }
            else
            {
                await _connection.UpdateAsync(entity);
            } 
        }
    }
}

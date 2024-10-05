using Task.Management.Entities;

namespace Task.Management.Service
{
    public interface ILocalDbService
    {
        Task<List<TaskEntity>> GetAllAsync();  
        System.Threading.Tasks.Task SaveAsync(TaskEntity entity);
        System.Threading.Tasks.Task DeleteAsync(TaskEntity entity);
    }
}

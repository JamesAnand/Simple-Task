using System.Collections.ObjectModel;
using Task.Management.Entities;
using Task.Management.Service;

namespace Task.Management.ViewModels
{
    public class TasksPageViewModel : ViewModelBase
    {
        private bool _showOptions;
        private ObservableCollection<TaskEntity> _tasks = new();
        private TaskEntity? _selectedTask;
        public DelegateCommand CommandAddNewTask { get; set; }
        public DelegateCommand CommandClose { get; set; }
        public DelegateCommand<TaskEntity> CommandSelected { get; set; }
        public DelegateCommand CommandEditTask { get; set; }
        public DelegateCommand CommandDeleteTask { get; set; }
        public TasksPageViewModel(INavigationService navigationService, ILocalDbService localDbService) : base(navigationService, localDbService)
        {
            CommandAddNewTask = new DelegateCommand(AddNewTask).ObservesCanExecute(() => CanNavigate);
            CommandEditTask = new DelegateCommand(EditTask).ObservesCanExecute(() => CanNavigate);
            CommandDeleteTask = new DelegateCommand(DeleteTask).ObservesCanExecute(() => CanNavigate);
            CommandSelected = new DelegateCommand<TaskEntity>(Selected).ObservesCanExecute(() => CanNavigate);
            CommandClose = new DelegateCommand(CloseOptions).ObservesCanExecute(() => CanNavigate);
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            await LoadTasks(); 
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _selectedTask = null;
            ShowOptions = false;
            var task = parameters.GetValue<TaskEntity>("Task");
            if(task != null)
            {
                if(Tasks.Any(t => t.TaskId == task.TaskId))
                {
                    var index = Tasks.IndexOf(Tasks.FirstOrDefault(t => t.TaskId == task.TaskId));
                    if(index != -1)
                    {
                        Tasks[index] = task;
                    }
                }
                else
                {
                    Tasks.Add(task);
                }
            }
            base.OnNavigatedTo(parameters);
        }

        private void Selected(TaskEntity entity)
        {
            CanNavigate = false;
            _selectedTask = entity;
            ShowOptions = true;
            CanNavigate = true;
        }

        private void CloseOptions()
        {
            CanNavigate = false;
            _selectedTask = null;
            ShowOptions = false;
            CanNavigate = true;
        }

        private async System.Threading.Tasks.Task LoadTasks()
        {
            Tasks.Clear();
            var tasks = await LocalDbService.GetAllAsync();
            foreach(var task in tasks)
            {
                Tasks.Add(task);    
            }
        }

        private async void DeleteTask()
        {
            CanNavigate = false;
            if(_selectedTask != null)
            {
                await LocalDbService.DeleteAsync(_selectedTask);
            }
            _selectedTask = null;
            await LoadTasks();
            _selectedTask = null;
            ShowOptions = false;
            CanNavigate = true;
        }

        private async void EditTask()
        {
            try
            {
                if(_selectedTask == null)
                {
                    return;
                }
                CanNavigate = false;
                await NavigationService.NavigateAsync("TaskPage", new NavigationParameters { { "Task", _selectedTask } });
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CanNavigate = true;
            }
        }

        private async void AddNewTask()
        {
            try
            {
                CanNavigate = false;
                await NavigationService.NavigateAsync("TaskPage");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CanNavigate = true;
            }
        }

        public ObservableCollection<TaskEntity> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }
        public bool ShowOptions
        {
            get { return _showOptions; }
            set { SetProperty(ref _showOptions, value); }
        }
    }
}

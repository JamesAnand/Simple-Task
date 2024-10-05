
using Task.Management.Entities;
using Task.Management.Service;

namespace Task.Management.ViewModels
{
    public class TaskPageViewModel : ViewModelBase
    {
        private bool _isValid,_isNew;
        private TaskEntity _taskEntity;
        private Color _borderColor = Colors.Red;
        private string _title, _description,_pageTitle;
        private DateTime _dueDate = DateTime.Now;
        public DateTime MinimumDate { get { return DateTime.Now; } }
        public DelegateCommand CommandSave { get; set; }
        public DelegateCommand CommandBack { get; set; }
        public TaskPageViewModel(INavigationService navigationService, ILocalDbService localDbService) : base(navigationService, localDbService)
        {
            CommandSave = new DelegateCommand(Save).ObservesCanExecute(() => CanNavigate);
            CommandBack = new DelegateCommand(BackNavigation).ObservesCanExecute(() => CanNavigate);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _taskEntity = parameters.GetValue<TaskEntity>("Task");
            PageTitle = "Add Task";
            if (_taskEntity != null)
            {
                PageTitle = "Edit Task";
                Description = _taskEntity.Description;
                Title = _taskEntity.Title;
                DueDate = _taskEntity.DueDate; 
            }
            base.OnNavigatedTo(parameters);
        }

        private async void BackNavigation()
        {
            try
            {
                CanNavigate = false;
                await NavigationService.GoBackAsync();
            }
            catch (Exception)
            {
            }
            finally
            {
                CanNavigate = true;
            }
        }

        private async void Save()
        {
            try
            {
                CanNavigate = false;
                var newTask = _taskEntity;
                if(_taskEntity == null)
                {
                    newTask = new TaskEntity
                    { 
                        TaskId = Guid.NewGuid().ToString()
                    };
                }
                newTask.Description = Description ?? string.Empty;
                newTask.Title = Title ?? string.Empty;
                newTask.DueDate = DueDate;
                await LocalDbService.SaveAsync(newTask);
                await NavigationService.GoBackAsync(new NavigationParameters { { "Task", newTask } });
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CanNavigate = true;
            }
        }

        private void CheckDetailsFilled()
        {
            IsValid = !string.IsNullOrWhiteSpace(Title) && DueDate.Date >= DateTime.Now.Date;
            if(string.IsNullOrWhiteSpace(Title))
            {
                BorderColor = Colors.Red;
            }
            else
            {
                BorderColor = Color.FromArgb("#512BD4");
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value, CheckDetailsFilled); }
        }
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { SetProperty(ref _dueDate, value, CheckDetailsFilled); }
        }
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }
        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetProperty(ref _pageTitle, value); }
        }
        public Color BorderColor
        {
            get { return _borderColor; }
            set { SetProperty(ref _borderColor, value); }
        }
    }
}

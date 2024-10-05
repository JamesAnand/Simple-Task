using Task.Management.Service;

namespace Task.Management.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IInitialize, IDestructible
    {
        public ILocalDbService LocalDbService { get; private set; } 
        public INavigationService NavigationService { get; private set; }
        private bool _canNavigate = true;
        public ViewModelBase(INavigationService navigationService, ILocalDbService localDbService)
        {
            NavigationService = navigationService;
            LocalDbService = localDbService;
        }
        public virtual void Destroy()
        {
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }
        public bool CanNavigate
        {
            get { return _canNavigate; }
            set { SetProperty(ref _canNavigate, value); }
        }
    }
}

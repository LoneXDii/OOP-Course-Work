using Client.Pages;
using Client.Persistence.Repositories;

namespace Client
{
    public partial class App : Application
    {
        public App(IUnitOfWork unitOfWork)
        {
            InitializeComponent();

            MainPage = new LogInPage(unitOfWork);
        }
    }
}

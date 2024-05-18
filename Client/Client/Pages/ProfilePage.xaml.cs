using Client.Persistence.Repositories;

namespace Client.Pages;

public partial class ProfilePage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;

	public ProfilePage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		usernameLabel.Text = _unitOfWork.User.GetUser().Name;
	}

	private void OnExitButtonClicked(object sender, EventArgs e)
	{
		Application.Current.MainPage = new NavigationPage(new LogInPage(_unitOfWork));
    }
}
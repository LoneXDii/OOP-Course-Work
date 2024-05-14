using Client.Persistence.Repositories;

namespace Client.Pages;

public partial class LogInPage : ContentPage
{
	private IUnitOfWork _unitOfWork;

	public LogInPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{
		try
		{
			_unitOfWork.User.Login(loginEntry.Text, passwordEntry.Text);
			Application.Current.MainPage = new AppShell();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Произошла ошибка", "Вы ввели некорректные данные", "OK");
		}
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegisterPage(_unitOfWork));
	}
}
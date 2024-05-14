using Client.Persistence.Repositories;

namespace Client.Pages;

public partial class RegisterPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;
	public RegisterPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		try
		{
			string username = usernameEntry.Text;
			string login = loginEntry.Text;
			string password = passwordEntry.Text;
			string password2 = passwordRepeatEntry.Text;
			if (password != password2)
			{
                await DisplayAlert("Произошла ошибка", "Пароли должны совпадать", "OK");
				return;
            }
			_unitOfWork.User.Register(username, login, password);
            Application.Current.MainPage = new AppShell();
        }
		catch (Exception)
		{
            await DisplayAlert("Произошла ошибка", "Вы ввели некорректные данные", "OK");
        }
	}
}
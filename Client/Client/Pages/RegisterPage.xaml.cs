using Client.Persistence.Repositories;
using System.Text.Json;
using System.Text.RegularExpressions;
using Client.Entitites;

namespace Client.Pages;

public partial class RegisterPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly string passwordPattern = @"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$";

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
			if (login.Contains(" "))
			{
                await DisplayAlert("Произошла ошибка", "Логин не может содержать пробелы", "OK");
                return;
            }
			if (!Regex.IsMatch(password, passwordPattern))
			{
                await DisplayAlert("Некорректный пароль", "Пароль должен удовлетворять следующим требованиям:\n" +
					"1. Длина не менее 8 символов\n" +
					"2. Пароль должен содержать не менее 1 цифры\n" +
					"3. Пароль должен содержать не менее 1 латинской буквы каждого регистра", "OK");
                return;
            }
            string file = Path.Combine(FileSystem.Current.AppDataDirectory, "credentials.json");
            string credentials = JsonSerializer.Serialize(
                new Credentials { Login = login, Password = password });
            File.WriteAllText(file, credentials);
            _unitOfWork.User.Register(username, login, password);
            Application.Current!.MainPage = new AppShell();
        }
		catch (Exception)
		{
            await DisplayAlert("Произошла ошибка", "Пользователь с таким логином уже существует", "OK");
        }
	}
}
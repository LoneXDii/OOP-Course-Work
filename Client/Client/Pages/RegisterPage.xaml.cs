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
                await DisplayAlert("��������� ������", "������ ������ ���������", "OK");
				return;
            }
			if (login.Contains(" "))
			{
                await DisplayAlert("��������� ������", "����� �� ����� ��������� �������", "OK");
                return;
            }
			if (!Regex.IsMatch(password, passwordPattern))
			{
                await DisplayAlert("������������ ������", "������ ������ ������������� ��������� �����������:\n" +
					"1. ����� �� ����� 8 ��������\n" +
					"2. ������ ������ ��������� �� ����� 1 �����\n" +
					"3. ������ ������ ��������� �� ����� 1 ��������� ����� ������� ��������", "OK");
                return;
            }
            string file = Path.Combine(FileSystem.Current.AppDataDirectory, "credentials.json");
            string credentials = JsonSerializer.Serialize(
                new Credentials { Login = login, Password = password });
            File.WriteAllText(file, credentials);
            _unitOfWork.User.Register(username, login, password);
            Application.Current!.MainPage = new AppShell();
        }
        catch (AggregateException)
        {
            await DisplayAlert("��������� ������", "����������� ����������� � �������", "OK");
        }
        catch (Exception)
		{
            await DisplayAlert("��������� ������", "������������ � ����� ������� ��� ����������", "OK");
        }
	}

    private void OnSeePasswordClicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        if (sender is Button button)
        {
            button.ImageSource = new FontImageSource
            {
                FontFamily = "FluentIcons",
                Glyph = passwordEntry.IsPassword ? Icons.Icons.EyeShow : Icons.Icons.EyeHide,
                Color = Color.FromArgb("#242424"),
                Size = 25
            };
        }
    }

    private void OnSeePasswordRepeatClicked(object sender, EventArgs e)
    {
        passwordRepeatEntry.IsPassword = !passwordRepeatEntry.IsPassword;
        if (sender is Button button)
        {
            button.ImageSource = new FontImageSource
            {
                FontFamily = "FluentIcons",
                Glyph = passwordRepeatEntry.IsPassword ? Icons.Icons.EyeShow : Icons.Icons.EyeHide,
                Color = Color.FromArgb("#242424"),
                Size = 25
            };
        }
    }
}
using Client.Persistence.Repositories;
using System.Text.Json;
using Client.Entitites;

namespace Client.Pages;

public partial class LogInPage : ContentPage
{
	private IUnitOfWork _unitOfWork;

	public LogInPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
	}

	private async void OnPageLoaded(object sender, EventArgs e)
	{
        var dir = FileSystem.Current.AppDataDirectory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        try
        {
            string file = Path.Combine(FileSystem.Current.AppDataDirectory, "credentials.json");
            string cerdentialsJson = File.ReadAllText(file);
            var credentials = JsonSerializer.Deserialize<Credentials>(cerdentialsJson);
            if (credentials is not null)
            {
                _unitOfWork.User.Login(credentials.Login!, credentials.Password!);
                Application.Current!.MainPage = new AppShell();
            }
        }
		catch (AggregateException)
		{
            await DisplayAlert("Произошла ошибка", "Отсутствует подключение к серверу", "OK");
        }
        catch {
			return;
		}
    }

    private async void OnLoginClicked(object sender, EventArgs e)
	{
		try
		{
			if (RememberMeCheckBox.IsChecked)
			{
                string file = Path.Combine(FileSystem.Current.AppDataDirectory, "credentials.json");
				string credentials = JsonSerializer.Serialize(
					new Credentials { Login = loginEntry.Text, Password = passwordEntry.Text });
				File.WriteAllText(file, credentials);
			}
            _unitOfWork.User.Login(loginEntry.Text, passwordEntry.Text);
			Application.Current!.MainPage = new AppShell();
		}
        catch (AggregateException)
        {
            await DisplayAlert("Произошла ошибка", "Отсутствует подключение к серверу", "OK");
        }
        catch
		{
			await DisplayAlert("Произошла ошибка", "Вы ввели некорректные данные", "OK");
		}
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegisterPage(_unitOfWork));
	}

	private void OnSeePasswordClicked(object sender, EventArgs e)
	{
		passwordEntry.IsPassword = !passwordEntry.IsPassword;
		if (sender is Button button)
		{
			button.ImageSource = new FontImageSource{
				FontFamily = "FluentIcons",
				Glyph = passwordEntry.IsPassword ? Icons.Icons.EyeShow : Icons.Icons.EyeHide,
				Color = Color.FromArgb("#242424"),
				Size = 25
			};
		}
	}
}

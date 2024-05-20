using Client.Persistence.Repositories;
using Client.Popups;
using CommunityToolkit.Maui.Views;

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
		Application.Current!.MainPage = new NavigationPage(new LogInPage(_unitOfWork));
    }

	private async void OnChangeNameButtonClicked(object sender, EventArgs e)
	{
		await this.ShowPopupAsync(new ChangeUsernamePopup(_unitOfWork));
        usernameLabel.Text = _unitOfWork.User.GetUser().Name;
    }

	private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
	{
		var result = await this.ShowPopupAsync(new ChangePasswordPopup(_unitOfWork));
		if (result is bool boolRes)
		{
			if (boolRes)
			{
                await DisplayAlert("Операция прошла успешно", "Пароль успешно изменен", "OK");
            }
			else
			{
                await DisplayAlert("Произошла ошибка", "Вы ввели некорректные данные", "OK");
            }
		}
	}
}
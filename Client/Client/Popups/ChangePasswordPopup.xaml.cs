using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class ChangePasswordPopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;

	public ChangePasswordPopup(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
	}

	private async void OnChangePasswordClicked(object sender, EventArgs e)
	{
		string oldPass = oldPasswordEntry.Text;
		string newPass = newPasswordEntry.Text;
		string newPassRepeat = newPasswordRepeatEntry.Text;

		if (newPass == oldPass)
		{
			errorLabel.Text = "Старый и новый пароль не должны совпадать";
			errorLabel.IsVisible = true;
			return;
		}
		if (newPass != newPassRepeat) {
            errorLabel.Text = "Пароли не совпадают";
            errorLabel.IsVisible = true;
            return;
        }
		if(newPass == "")
		{
            errorLabel.Text = "Некорректный пароль";
            errorLabel.IsVisible = true;
            return;
        }

		try
		{
			_unitOfWork.User.ChangePassword(oldPass, newPass);
			await CloseAsync(true);
		}
		catch
		{
            errorLabel.Text = "Неверный пароль";
            errorLabel.IsVisible = true;
            return;
        }
    }
}
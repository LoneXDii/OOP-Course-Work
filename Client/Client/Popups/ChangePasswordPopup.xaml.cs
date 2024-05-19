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
			errorLabel.Text = "������ � ����� ������ �� ������ ���������";
			errorLabel.IsVisible = true;
			return;
		}
		if (newPass != newPassRepeat) {
            errorLabel.Text = "������ �� ���������";
            errorLabel.IsVisible = true;
            return;
        }
		if(newPass == "")
		{
            errorLabel.Text = "������������ ������";
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
            errorLabel.Text = "�������� ������";
            errorLabel.IsVisible = true;
            return;
        }
    }
}
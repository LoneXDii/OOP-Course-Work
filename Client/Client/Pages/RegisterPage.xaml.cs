using Client.Persistence.Repositories;
using System.Text.RegularExpressions;

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
			_unitOfWork.User.Register(username, login, password);
            Application.Current!.MainPage = new AppShell();
        }
		catch (Exception)
		{
            await DisplayAlert("��������� ������", "������������ � ����� ������� ��� ����������", "OK");
        }
	}
}
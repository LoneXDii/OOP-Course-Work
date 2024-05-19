using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class AddChatPopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;

	public AddChatPopup(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
	}

	private async void OnCreateChatClicked(object sender, EventArgs e)
	{
		if (chatNameEntry.Text == "")
		{
			errorLabel.Text = "Некорректное название чата";
			errorLabel.IsVisible = true;
			return;
		}
		try
		{
			_unitOfWork.ChatRepository.Create(chatNameEntry.Text, _unitOfWork.User.GetUser());
			await CloseAsync();
		}
		catch
		{
            errorLabel.Text = "Что-то пошло не так";
            errorLabel.IsVisible = true;
        }
	}
}
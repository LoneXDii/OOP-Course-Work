using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class ChangeChatNamePopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;
	private Chat _currentChat;

	public ChangeChatNamePopup(IUnitOfWork unitOfWork, Chat chat)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		_currentChat = chat;
		chatNameEntry.Text = chat.Name;
	}

	private async void OnChangeNameClicked(object sender, EventArgs e)
	{
		string startName = _currentChat.Name;
		_currentChat.Name = chatNameEntry.Text;
		try
		{
			_unitOfWork.ChatRepository.Update(_currentChat);
		}
		catch
		{
            await CloseAsync(startName);
        }
		await CloseAsync(_currentChat.Name);
	}
}
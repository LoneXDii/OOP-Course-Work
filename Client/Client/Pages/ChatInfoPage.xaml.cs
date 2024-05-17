using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using Client.Popups;
using CommunityToolkit.Maui.Views;

namespace Client.Pages;
//rename chat, add and delete users in chat, exit from chat
public partial class ChatInfoPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;
	private Chat _currentChat;

	public ChatInfoPage(IUnitOfWork unitOfWork, Chat chat)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		_currentChat = chat;
		chatNameLabel.Text = chat.Name;
	}

	private void OnPageLoaded(object sender, EventArgs e)
	{
		_unitOfWork.ChatMembersRepository.GetFromServer(_currentChat);
		chatMembersView.ItemsSource = _unitOfWork.ChatMembersRepository.Members;
	}

	private void OnDeleteUserClicked(object sender, EventArgs e)
	{
		var button = sender as Button;
		var param = button?.CommandParameter as User;
		if(param is not null)
		{
			_unitOfWork.ChatMembersRepository.Delete(param);
		}
	}

	private async void OnEditChatEntyClicked(object sender, EventArgs e)
	{
		var result = await this.ShowPopupAsync(new ChangeChatNamePopup(_unitOfWork, _currentChat));
		if (result is string stringResult)
		{
			chatNameLabel.Text = stringResult;
		}
	}
}
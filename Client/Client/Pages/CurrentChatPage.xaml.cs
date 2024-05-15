using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using Client.Popups;
using CommunityToolkit.Maui.Views;
using System.Runtime.CompilerServices;

namespace Client.Pages;

public partial class CurrentChatPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;
	private Chat _currentChat;

	public CurrentChatPage(IUnitOfWork unitOfWork, Chat currentChat)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		_currentChat = currentChat;
		_unitOfWork.MessageRepository.GetFromServer(_currentChat);
		messageView.ItemsSource = _unitOfWork.MessageRepository.Messages;
	}

	private void OnSendButtonClicked(object sender, EventArgs e)
	{
		string text = messageEntry.Text;
		if (text == "")
			return;
		var message = new Message
		{
			Text = text,
			UserId = _unitOfWork.User.GetUser().Id,
			ChatId = _currentChat.Id
		};
		_unitOfWork.MessageRepository.SendToServer(message);
	}

	private void OnGetsureTapped(object sender, EventArgs e)
	{
		this.ShowPopup(new MessageClickPopup());
	}
}
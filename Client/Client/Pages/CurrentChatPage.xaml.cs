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
		_unitOfWork.MessageRepository.Add(message);
	}

	private async void OnGetsureTapped(object sender, TappedEventArgs e)
	{
		var vertStack = sender as VerticalStackLayout;
		var message = e.Parameter as Message;
		if (vertStack is null || message is null)
		{
			return;
		}
		if (message.UserId != _unitOfWork.User.GetUser().Id)
		{
			return;
		}
		await this.ShowPopupAsync(new MessageClickPopup(vertStack, _unitOfWork, message));
	}
}
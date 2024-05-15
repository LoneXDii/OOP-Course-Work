using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using Client.Popups;
using CommunityToolkit.Maui.Views;

namespace Client.Pages;

public partial class CurrentChatPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;
	private Chat _currentChat;
	private bool is_editing = false;
	private Message? _edited_message = null;

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
		if (is_editing)
		{
			is_editing = false;
			EditMessage(_edited_message);
			_edited_message = null;
            messageEntry.Text = String.Empty;
            return;
		}
		if (text == "")
			return;
		var message = new Message
		{
			Text = text,
			UserId = _unitOfWork.User.GetUser().Id,
			ChatId = _currentChat.Id
		};
		_unitOfWork.MessageRepository.Add(message);
		messageEntry.Text = String.Empty;
	}

	private void EditMessage(Message? message)
	{
		if (message is null)
		{
			return;
		}
		message.Text = messageEntry.Text;
		_unitOfWork.MessageRepository.Update(message);
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
		var result = await this.ShowPopupAsync(new MessageClickPopup(vertStack, _unitOfWork, message));
		if (result is bool BoolResult)
		{
			if (BoolResult)
			{
				is_editing = true;
				_edited_message = message;
			    messageEntry.Text = message.Text;
			}
		}
	}
}
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
		chatNameLabel.Text = _currentChat.Name;
		messageView.ItemsSource = _unitOfWork.MessageRepository.Messages;
	}

	private async void OnSendButtonClicked(object sender, EventArgs e)
	{
		string text = messageEntry.Text;
        if (text == "")
            return;
        if (is_editing)
		{
			try
			{
				EditMessage(_edited_message);
				OnCancelEditingButtonClicked(sender, e);
				return;
			}
			catch
			{
				await DisplayAlert("Произошла ошибка", "Ошибка редактирования сообщения", "OK");
				return;
			}
		}
		var message = new Message
		{
			Text = text,
			UserId = _unitOfWork.User.GetUser().Id,
			ChatId = _currentChat.Id
		};
		try
		{
			_unitOfWork.MessageRepository.Add(message);
		}
		catch
		{
            await DisplayAlert("Произошла ошибка", "Ошибка редактирования сообщения", "OK");
			return;
        }
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

	private async void OnMessageTapped(object sender, TappedEventArgs e)
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
		if (result is bool boolResult)
		{
			if (boolResult)
			{
                editingInfoBorder.IsVisible = true;
                updatingMessageTextLabel.Text = message.Text;
                is_editing = true;
				_edited_message = message;
			    messageEntry.Text = message.Text;
			}
		}
	}

	private async void OnChatNameTapped(object sender, EventArgs e)
	{
		if (!_currentChat.IsDialogue)
		{
			await Navigation.PushAsync(new ChatInfoPage(_unitOfWork, _currentChat));
		}
	}

	private void OnCancelEditingButtonClicked(object sender, EventArgs e)
	{
		editingInfoBorder.IsVisible = false;
		is_editing = false;
        _edited_message = null;
        messageEntry.Text = String.Empty;
    }
}
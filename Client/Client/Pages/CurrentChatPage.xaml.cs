using Client.Domain.Entitites;
using Client.Persistence.Repositories;

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
		MessageView.ItemsSource = _unitOfWork.MessageRepository.GetMessages();
	}
}
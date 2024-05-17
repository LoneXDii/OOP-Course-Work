using Client.Domain.Entitites;
using Client.Persistence.Repositories;

namespace Client.Pages;
//rename, add and delete users in chat, exit from chat
public partial class ChatInfoPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;
	private Chat _currentChat;

	public ChatInfoPage(IUnitOfWork unitOfWork, Chat chat)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		_currentChat = chat;
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
}
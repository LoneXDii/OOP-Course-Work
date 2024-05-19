using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using Client.Popups;
using CommunityToolkit.Maui.Views;

namespace Client.Pages;

public partial class ChatsPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;

	public ChatsPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
	}

	private void OnPageLoaded(object sender, EventArgs e)
	{
		_unitOfWork.ChatRepository.GetFromServer(_unitOfWork.User.GetUser());
		ChatView.ItemsSource = _unitOfWork.ChatRepository.Chats;
	}

	private async void OnGetsurePrimaryTapped(object sender, TappedEventArgs e)
	{
		var chat = (Chat?)e.Parameter;
		if (chat is not null)
		{
            await Navigation.PushAsync(new CurrentChatPage(_unitOfWork, chat));
        }
	}

	private async void OnAddChatClicked(object sender, EventArgs e)
	{
		await this.ShowPopupAsync(new AddChatPopup(_unitOfWork));
	}
}
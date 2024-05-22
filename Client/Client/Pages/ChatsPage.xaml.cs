using Client.Domain.Entitites;
using Client.Persistence.Exceptions;
using Client.Persistence.Repositories;
using Client.Persistence.Services;
using Client.Popups;
using CommunityToolkit.Maui.Views;
using System;

namespace Client.Pages;

public partial class ChatsPage : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;

	public ChatsPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
	}

	private void OnPageAppearing(object sender, EventArgs e)
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

	private async void OnMessageUserClicked(object sender, EventArgs e)
	{ 
		var user = await this.ShowPopupAsync(new FindUserPopup(_unitOfWork));
		if (user is User userRes)
		{
			Chat? dialogue = null;
			try
			{
				dialogue = _unitOfWork.ChatRepository.CreateDialogue(_unitOfWork.User.GetUser(), userRes);              
                try
                {
                    var ids = dialogue.Name.Split('&');
                    var u1Id = Convert.ToInt32(ids[0]);
                    var u2Id = Convert.ToInt32(ids[1]);
					_unitOfWork.ChatMembersRepository.GetFromServer(dialogue);
					var member = _unitOfWork.ChatMembersRepository.Members.FirstOrDefault(u => u.Id != _unitOfWork.User.GetUser().Id);
                    dialogue.Name = member is null ? "DELETED" : member.Name;
                }
                catch { }
            }
			catch (DialogueExistException ex)
			{
				try
				{
					var dialogueId = Convert.ToInt32(ex.Message);
					dialogue = _unitOfWork.ChatRepository.Chats.FirstOrDefault(c => c.Id == dialogueId);
				}
				catch
				{
                    await DisplayAlert("Произошла ошибка", "Что-то пошло не так", "OK");
					return;
                }
			}
			catch
			{
                await DisplayAlert("Произошла ошибка", "Что-то пошло не так", "OK");
                return;
			}
			if (dialogue is null)
			{
                await DisplayAlert("Произошла ошибка", "Что-то пошло не так", "OK");
                return;
            }
			await Navigation.PushAsync(new CurrentChatPage(_unitOfWork, dialogue));
		}
	}
}
using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class AddUserToChatPopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;
	private List<User> _users;
	public AddUserToChatPopup(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		_users = _unitOfWork.AllUsers();
	}

	private void OnSearchBarTextChanged(object sender, EventArgs e)
	{
		if (userSearchBar.Text != "")
		{
			usersView.IsVisible = true;
            usersView.ItemsSource = _users.Where(x => x.Name.StartsWith(userSearchBar.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        }
		else
		{
			usersView.IsVisible = false;
		}
	}

	private async void OnAddButtonCliched(object sender, EventArgs e)
	{
		var button = sender as Button;
		var parameter = button?.CommandParameter as User;
		if (parameter is not null)
		{
			try
			{
				_unitOfWork.ChatMembersRepository.Add(parameter);
				await CloseAsync(true);
			}
			catch
			{
				await CloseAsync(false);
			}
		}
	}
}
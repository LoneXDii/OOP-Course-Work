using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class FindUserPopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;
	private List<User> _users;
	public FindUserPopup(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		_users = _unitOfWork.AllUsers();
	}

	private void OnSearchButonTextChanged(object sender, EventArgs e)
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
}
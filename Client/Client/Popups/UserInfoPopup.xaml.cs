using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class UserInfoPopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly User _currentUser;

	public UserInfoPopup(IUnitOfWork unitOfWork, User currentUser)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		_currentUser = currentUser;
		usernameLabel.Text = _currentUser.Name;
	}

	private void OnMessageClicked(object sender, EventArgs e)
	{
		
	}
}
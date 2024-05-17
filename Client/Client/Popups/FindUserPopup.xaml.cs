using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class FindUserPopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;
	public FindUserPopup(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
		usersView.ItemsSource = _unitOfWork.AllUsers();
	}
}
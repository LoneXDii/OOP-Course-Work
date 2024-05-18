using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;


namespace Client.Popups;

public partial class ChangeUsernamePopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;

	public ChangeUsernamePopup(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
		_unitOfWork = unitOfWork;
        userNameEntry.Text = _unitOfWork.User.GetUser().Name;

    }

    private async void OnChangeNameClicked(object sender, EventArgs e)
    {
        if(userNameEntry.Text == "")
        {
            return;
        }
        _unitOfWork.User.Update(userNameEntry.Text);
        await CloseAsync();
    }
}
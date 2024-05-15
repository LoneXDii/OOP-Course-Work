using Client.Domain.Entitites;
using Client.Persistence.Repositories;
using CommunityToolkit.Maui.Views;

namespace Client.Popups;

public partial class MessageClickPopup : Popup
{
	private readonly IUnitOfWork _unitOfWork;
	private Message _message;
	public MessageClickPopup(View anchor, IUnitOfWork unitOfWork, Message message)
	{
		InitializeComponent();
		Anchor = anchor;
		_unitOfWork = unitOfWork;
		_message = message;
	}

	private void OnEditClicked(object sender, EventArgs e)
	{
		Close();
	}

	private void OnDeleteClicked(object sender, EventArgs e)
	{
		try
		{
			_unitOfWork.MessageRepository.Delete(_message);
		}
		finally
		{
			Close();
		}
	}
}
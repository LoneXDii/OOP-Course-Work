using Client.Domain.Abstractions;

namespace Client
{
    public partial class MainPage : ContentPage
    {
        private IUnitOfWork _unitOfWork;
        int count = 0;

        public MainPage(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);

            //test requests
            _unitOfWork.User.Login("UserClient1", "NewPass222");
            _unitOfWork.User.Delete();
        }
    }

}

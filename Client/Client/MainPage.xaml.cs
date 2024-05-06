using Client.Domain.Abstractions;

namespace Client
{
    public partial class MainPage : ContentPage
    {
        private IUnitOfWork _UnitOfWork;
        int count = 0;

        public MainPage(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _UnitOfWork = unitOfWork;
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
            _UnitOfWork.Login("User0", "password0");

        }
    }

}

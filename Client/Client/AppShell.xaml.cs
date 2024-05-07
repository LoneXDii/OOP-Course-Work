using Client.Pages;

namespace Client
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CurrentChatPage), typeof(CurrentChatPage));
        }
    }
}

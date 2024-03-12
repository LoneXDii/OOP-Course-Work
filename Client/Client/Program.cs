using Client;
using Client.ApplicationLayer;
using Client.DomainLayer;
using Client.InfrastructureLayer.Data;
using Client.InfrastructureLayer.Presentational;


ApplicationService app = new ApplicationService(new EntitiesHandler());
ServerService server = new ServerService("127.0.0.1", 8888, 8889, app);
Task.Run(() => server.ProcessP2PConnectionAsync());
ConsoleUI ui = new ConsoleUI(app);
ui.Start();

//ServerService server = new ServerService("127.0.0.1", 8888, 8889);
//ProgramHandler program = new ProgramHandler(server);
////Task.Run(() => program.ProcessAsync());
//Task.Run(() => program.ProcessP2PAsync());
////await program.Start();

//ConsoleUI ui = new ConsoleUI();
//program.OnMessageCreated += ui.PrintMessage;
//ui.registration += program.Registrate;
//ui.messagePrinted += program.MessagePrinted;
//ui.dialogueOpened += program.AskUserByLogin;

//ui.Start();
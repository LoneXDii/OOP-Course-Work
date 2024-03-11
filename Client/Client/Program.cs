using Client;
using Client.Entities.Presentational;
using Client.Services;

ServerService server = new ServerService("127.0.0.1", 8888, 8889);
ProgramHandler program = new ProgramHandler(server);
//Task.Run(() => program.ProcessAsync());
Task.Run(() => program.ProcessP2PAsync());
//await program.Start();

ConsoleUI ui = new ConsoleUI();
program.OnMessageCreated += ui.PrintMessage;
ui.registration += program.Registrate;
ui.messagePrinted += program.MessagePrinted;
ui.dialogueOpened += program.AskUserByLogin;

ui.Start();
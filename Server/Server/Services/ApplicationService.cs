using Server.Domain.Entities;
using MediatR;
using Server.Application.UserUseCases.Commands;
using Server.Application.UserUseCases.Queries;
using Server.Application.ChatUseCases.Commands;
using Server.Application.ChatUseCases.Queries;
using Server.Application.MessageUseCases.Commands;
using Server.Application.MessageUseCases.Queries;

namespace Server.Services;

internal interface IApplicationService
{
    Task Run();
}

internal class ApplicationService : IApplicationService
{
    private readonly IMediator _mediator;

    public ApplicationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Run()
    {
        while (true)
        {
            Console.WriteLine("Select request to check:" +
                              "\n1.Get all users" +
                              "\n2.Get all chats" +
                              "\n3.Get all messages" +
                              "\n4.Add user (name, login, password)" +
                              "\n5.Update user (id, name, login, password)" +
                              "\n6.Delete user (id)" +
                              "\n7.Add chat (name, creatorId)" +
                              "\n8.Update chat (id, name)" +
                              "\n9.Delete chat (id)" +
                              "\n10.Add message (text, senderId, chatId)" +
                              "\n11.Update message (id, text)" +
                              "\n12.Delete message (id)" +
                              "\n13.Get user's chats (userId)" +
                              "\n14.Get user's chat messages (userId, chatId)");

            string param = Console.ReadLine();
            if (param is null) continue;


            IEnumerable<User> users;
            IEnumerable<Chat> chats;
            IEnumerable<Message> messages;
            User user;
            Chat chat;
            Message message;
            string name;
            string login;
            string password;
            string text;
            int id;
            int userId;
            int chatId;

            Console.WriteLine("");
            switch (param)
            {
                
                case "1":
                    users = await _mediator.Send(new GetAllUsersRequest());
                    foreach (var u in users)
                    {
                        Console.WriteLine($"name: {u.Name}, login: {u.Login}, password: {u.Password}, id:{u.Id}");
                    }
                    break;

                case "2":
                    chats = await _mediator.Send(new GetAllChatsRequest());
                    foreach (var c in chats)
                    {
                        Console.WriteLine($"name: {c.Name}, creatorId = {c.CreatorId}, id = {c.Id}");
                    }
                    break;

                case "3":
                    messages = await _mediator.Send(new GetAllMessagesRequest());
                    foreach (var m in messages)
                    {
                        Console.WriteLine($"Text = {m.Text}, SendTime = {m.SendTime}, SenderId = {m.SenderId}, ChatId = {m.ChatId}, Id = {m.Id}");
                    }
                    break;

                case "4":
                    name = Console.ReadLine();
                    login = Console.ReadLine();
                    password = Console.ReadLine();
                    user = new User(name, login, password);
                    user = await _mediator.Send(new AddUserRequest(user));
                    Console.WriteLine($"Added user:" +
                                      $"\nname: {user.Name}, login: {user.Login} , password:  {user.Password}, id:{user.Id}");
                    break;

                case "5":
                    id = Convert.ToInt32(Console.ReadLine());
                    name = Console.ReadLine();
                    login = Console.ReadLine();
                    password = Console.ReadLine();

                    user = new User(name, login, password);
                    user.Id = id;
                    user = await _mediator.Send(new UpdateUserRequest(user));
                    Console.WriteLine($"Updated user:" +
                                      $"\nname: {user.Name}, login: {user.Login} , password:  {user.Password}, id:{user.Id}");
                    break;

                case "6":
                    id = Convert.ToInt32(Console.ReadLine());
                    await _mediator.Send(new DeleteUserRequest(id));
                    Console.WriteLine($"Deleted user with id = {id}");
                    break;

                case "7":
                    name = Console.ReadLine();
                    userId = Convert.ToInt32(Console.ReadLine());
                    chat = new Chat(name);
                    chat.CreatorId = userId;
                    chat = await _mediator.Send(new AddChatRequest(chat));
                    Console.WriteLine($"Added chat:" +
                                      $"\nname: {chat.Name}, creatorId = {chat.CreatorId}, id:{chat.Id}");
                    break;

                case "8":
                    id = Convert.ToInt32(Console.ReadLine());
                    name = Console.ReadLine();
                    chat = new Chat(name);
                    chat.Id = id;
                    chat = await _mediator.Send(new UpdateChatRequest(chat));
                    Console.WriteLine($"Updated chat with id = {id}");
                    break;

                case "9":
                    id = Convert.ToInt32(Console.ReadLine());
                    await _mediator.Send(new DeleteChatRequest(id));
                    Console.WriteLine($"Deleted chat with id = {id}");
                    break;

                case "10":
                    text = Console.ReadLine();
                    userId = Convert.ToInt32(Console.ReadLine());
                    chatId = Convert.ToInt32(Console.ReadLine());
                    message = new Message(text);
                    message.SenderId = userId;
                    message.ChatId = chatId;
                    message = await _mediator.Send(new AddMessageRequest(message));
                    Console.WriteLine($"Added message:" +
                                      $"\nText = {message.Text}, SendTime = {message.SendTime}, SenderId = {message.SenderId}, ChatId = {message.ChatId}, Id = {message.Id}");
                    break;

                case "11":
                    id = Convert.ToInt32(Console.ReadLine());
                    text = Console.ReadLine();
                    message = new Message(text);
                    message.Id = id;
                    message = await _mediator.Send(new UpdateMessageRequest(message));
                    Console.WriteLine($"Updated message with id = {id}");
                    break;

                case "12":
                    id = Convert.ToInt32(Console.ReadLine());
                    await _mediator.Send(new DeleteMessageRequest(id));
                    Console.WriteLine($"Deleted message with id = {id}");
                    break;

                case "13":
                    id = Convert.ToInt32(Console.ReadLine());
                    chats = await _mediator.Send(new GetUserChatsRequest(id));
                    foreach (var c in chats)
                    {
                        Console.WriteLine($"name: {c.Name}, creatorId = {c.CreatorId}, id = {c.Id}");
                    }
                    break;

                case "14":
                    userId = Convert.ToInt32(Console.ReadLine());
                    chatId = Convert.ToInt32(Console.ReadLine());
                    messages = await _mediator.Send(new GetUserChatMessagesRequest(userId, chatId));
                    foreach (var m in messages)
                    {
                        Console.WriteLine($"Text = {m.Text}, SendTime = {m.SendTime}, SenderId = {m.SenderId}, ChatId = {m.ChatId}, Id = {m.Id}");
                    }
                    break;

                default:
                    break;
            }
            Console.WriteLine("");
        }
    }

}

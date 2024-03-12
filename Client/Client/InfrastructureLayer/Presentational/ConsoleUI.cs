using Client.ApplicationLayer;
using Client.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Client.InfrastructureLayer.Presentational.ConsoleUI;

namespace Client.InfrastructureLayer.Presentational;

internal class ConsoleUI
{
    ApplicationService application;

    public ConsoleUI(ApplicationService application)
    {
        this.application = application;
        this.application.OnMessageAdded += PrintMessage;
    }

    public void Start()
    {
        Console.WriteLine("Enter login:");
        string? login = Console.ReadLine();
        if (login is not null)
            application.CreateUser(login);
        Console.WriteLine("Enter:\n1.Create chat\n2.Get chat list");
        int param = Convert.ToInt32(Console.ReadLine());
        
        if(param == 1)
        {
            Console.WriteLine("Enter chat name");
            string? chatName = Console.ReadLine();
            if (chatName is not null)
                application.CreateChat(chatName);
        }
        if (param == 2)
        {
            var chats = application.GetChats();
            int i = 1;
            foreach (var chat in chats)
            {
                Console.WriteLine($"{i}.{chat}");
                i++;
            }

            Console.WriteLine("Select chat:");
            param = Convert.ToInt32(Console.ReadLine());
            application.SelectChat(param);

            while (true)
            {
                string? message = Console.ReadLine();
                if (message is not null)
                    application.CreateMessage(message);
            }
        }

        Console.WriteLine("Enter:\n1.Create chat\n2.Get chat list");
        param = Convert.ToInt32(Console.ReadLine());
        if (param == 2)
        {
            var chats = application.GetChats();
            int i = 1;
            foreach (var chat in chats)
            {
                Console.WriteLine($"{i}.{chat}");
                i++;
            }
        }

        Console.WriteLine("Select chat:");
        param = Convert.ToInt32(Console.ReadLine());
        application.SelectChat(param);

        Console.WriteLine("Enter:\n1.Add member\n2.Print messages");
        param = Convert.ToInt32(Console.ReadLine());
        if(param == 1)
        {
            Console.WriteLine("Enter member name");
            string? name = Console.ReadLine();
            if(name is not null)
                application.AddUserToChat(name);
        }


        Console.WriteLine("Enter:\n1.Add member\n2.Print messages");
        param = Convert.ToInt32(Console.ReadLine());
        while (true)
        {
            string? message = Console.ReadLine();
            if(message is not null)
                application.CreateMessage(message);
        }
    }
    public void PrintMessage(string message)
    {
        if (OperatingSystem.IsWindows())
        {
            var position = Console.GetCursorPosition();
            int left = position.Left;
            int top = position.Top;

            Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
            Console.SetCursorPosition(0, top);
            Console.WriteLine(message);
            Console.SetCursorPosition(left, top + 1);
        }
        else Console.WriteLine(message);
    }
}

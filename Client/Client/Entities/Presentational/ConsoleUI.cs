using Client.Entities.Logick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities.Presentational;

internal class ConsoleUI
{
    public delegate void Registration(string login);
    public event Registration? registration;

    public delegate void DialogueOpened(string login);
    public event DialogueOpened? dialogueOpened;

    public delegate void MessagePrinted(string text);
    public event MessagePrinted? messagePrinted;

    public void Start()
    {
        Console.WriteLine("Enter login:");
        string? login = Console.ReadLine();
        if (login is not null)
            registration?.Invoke(login);
        Console.WriteLine("Enter login of user you want to chat");
        login = Console.ReadLine();
        if(login != "1" && login is not null)
        {
            dialogueOpened?.Invoke(login);
        }
        while (true)
        {
            string? messageText = Console.ReadLine();
            if (messageText != null)
            {
                messagePrinted?.Invoke(messageText);
            }
        }
    }
    public void PrintMessage(Message message, string senderName)
    {
        if (OperatingSystem.IsWindows())
        {
            var position = Console.GetCursorPosition();
            int left = position.Left;
            int top = position.Top;

            Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
            Console.SetCursorPosition(0, top);
            Console.WriteLine($"{senderName}: {message.Text}");
            Console.SetCursorPosition(left, top + 1);
        }
        else Console.WriteLine($"{senderName}: {message.Text}");
    }
}

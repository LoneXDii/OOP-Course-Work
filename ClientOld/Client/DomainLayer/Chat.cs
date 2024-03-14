using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DomainLayer;

internal class Chat
{
    public string Name { get; set; }
    public int Id { get; set; }

    public Chat(string name)
    {
        Name = name;
    }
}

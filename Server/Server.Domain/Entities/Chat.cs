using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities;

public class Chat : Entity
{
    public Chat(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public int CreatorId { get; set; }
}

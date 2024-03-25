using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities;

internal class ChatMember : Entity
{
    public int UserId { get; set; }
    public int ChatId { get; set; }
}

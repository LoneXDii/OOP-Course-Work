using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities.Logick;

internal class Dialogue : IChat
{
    public int Id { get; }
    public int User1Id { get; }
    public int User2Id { get; }

    public void AddMessage(Message message)
    {

    }
}

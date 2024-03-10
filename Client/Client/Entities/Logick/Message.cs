using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities.Logick;

internal class Message
{
    public int Id { get; }
    public int SenderId { get; set; }
    public int ChatId { get; set; }
    public DateTime SendTime { get; set; }
    public string Text { get; set; }
}

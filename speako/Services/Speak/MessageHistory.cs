using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.Speak
{
  internal class MessageHistory: JsonSerializable<MessageHistory>, IMessageHistory
  {
    public MessageHistory() {
    }
  }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.Speak
{
  public class PText : IPText
  {
    public string voice { get; set; }
    public string message { get; set; }
  }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Messaging
{
    public interface IMessagePublishServer
    {
        int SubscribePort { get; set; }
        int PublishPort { get; set; }
        void Start();
        void Stop();
    }
}

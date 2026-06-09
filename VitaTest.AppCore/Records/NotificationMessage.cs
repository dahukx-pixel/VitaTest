using System;
using System.Collections.Generic;
using System.Text;
using VitaTest.AppCore.Enums;

namespace VitaTest.AppCore.Records
{
    public record NotificationMessage(string Text, NotifyType Type);
}

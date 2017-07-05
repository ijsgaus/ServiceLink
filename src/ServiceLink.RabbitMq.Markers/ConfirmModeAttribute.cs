using System;

namespace ServiceLink.RabbitMq.Markers
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property)]
    public class ConfirmModeAttribute : Attribute
    {
        public ConfirmModeAttribute(bool confirmMode)
        {
            ConfirmMode = confirmMode;
        }

        public bool ConfirmMode { get; }
    }
}
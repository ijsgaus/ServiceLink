using RabbitLink.Topology;

namespace ServiceLink.RabbitMq.Topology
{
    public abstract class Exchange 
    {
        private Exchange(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public sealed class Passive : Exchange
        {
            public Passive(string name) : base(name)
            {
            }
        }

        public sealed class Declare : Exchange
        {
            public Declare(string name, LinkExchangeType type) : base(name)
            {
                Type = type;
            }
            
            public LinkExchangeType Type { get; }
        }
        
    }
}
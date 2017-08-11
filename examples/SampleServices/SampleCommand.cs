using ServiceLink.Markers;

namespace SampleServices
{
    [Contract("0.5","sample.command")]
    public class SampleCommand
    {
        public int OrderId { get; set; }
        public Operation Operation { get; set; }
        public Good Good { get; set; }
        public SampleEnum Sample { get; set; }
    }
}
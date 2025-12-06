using Paftax.Pafta.Shared.Models.Element;

namespace Paftax.Pafta.Shared.Models
{
    public class GarbageElementModel : ElementModel
    {
        public required int Count { get; set; }
        public required bool IsUsed { get; set; }
    }
}

namespace Paftax.Pafta.Shared.Interfaces
{
    public interface ICloseable
    {
        event Action? CloseAction;
    }
}

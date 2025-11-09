namespace Paftax.Pafta.Shared.Interfaces
{
    public interface ICleanGarbageViewModel
    {
        Action? RequestLoadMaterials { get; set; }
        Action? RequestLoadFilters { get; set; }
        Action? RequestLoadLines { get; set; }

        event Action? CancelAction;
        event Action? CleanAction;
    }
}

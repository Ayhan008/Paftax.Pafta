using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class ViewModelFactory(Document document)
    {
        private readonly Document? _document = document;
        private CancellationToken _token = CancellationToken.None;
        public ViewModelFactory Cancellable(CancellationToken token)
        {
            _token = token;
            return this;
        }

        public List<ViewModel> CreateModels()
        {
            List<ViewModel> viewModels = [];

            IEnumerable<View> views = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>();

            foreach (View view in views)
            {
                _token.ThrowIfCancellationRequested();

                bool isPlaced = true;
                if (view.GetPlacementOnSheetStatus() == ViewPlacementOnSheetStatus.NotPlaced)
                {
                    isPlaced = false;
                }

                ViewModel viewModel = new()
                {
                    Id = view.Id.Value,
                    Name = view.Name,
                    IsPlaced = isPlaced,
                    ViewType = view.ViewType.ToString(),
                };

                viewModels.Add(viewModel);
            }
            return viewModels;
        }
    }
}

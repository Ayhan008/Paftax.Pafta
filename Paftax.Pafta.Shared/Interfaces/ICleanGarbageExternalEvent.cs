using Paftax.Pafta.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paftax.Pafta.Shared.Interfaces
{
    public interface ICleanGarbageExternalEvent
    {
        void RequestElements(string categoryName, ObservableCollection<string> target, Action onCompleted);
        void RequestLineModels(ObservableCollection<LineModel> target, Action onCompleted);
        void RequestFilterModels(ObservableCollection<FilterModel> target, Action onCompleted);
    }
}

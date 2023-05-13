using OrchardCore.DisplayManagement.Views;

namespace nibss_orchad_azure.ViewModels
{
    public class SearchFormViewModel : ShapeViewModel
    {
        public SearchFormViewModel(string shapeType) : base(shapeType)
        {
        }

        public string Terms { get; set; }
    }
}

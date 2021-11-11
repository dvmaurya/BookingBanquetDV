namespace ViewModelsAndValidation.Views
{
    public interface IAppFrame
    {
        void ShowLoadingIndicator(string contentText);

        void ChangeLoadingIndicatorText(string contentText);

        void HideLoadingIndicator();
    }
}

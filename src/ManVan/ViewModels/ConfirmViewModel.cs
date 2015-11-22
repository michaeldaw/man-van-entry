using System.Windows;

namespace ManVan
{
    public class ConfirmViewModel
    {
        public string AffirmativeLabel { get; set; } = "Yes";

        public string NegativeLabel { get; set; } = "No";

        public ConfirmResult Result { get; set; } = ConfirmResult.Negative;

        public string Caption { get; set; }

        public string Message { get; set; }

        public Visibility NegativeVisibility { get; set; } =
            Visibility.Visible;

        public Visibility AffirmativeVisibility { get; set; } =
            Visibility.Visible;
    }

    public enum ConfirmResult
    {
        Affirmative,
        Negative,
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace lab7.Views
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            this.FindControl<Button>("OKBtn").Click += delegate
            {
                Close();
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

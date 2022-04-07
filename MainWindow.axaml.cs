using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaDatabase.Model;
using System;
using Client = Supabase.Client;
namespace chat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var messageControl = this.FindControl<TextBox>("MessageTextBox");
            var nameControl = this.FindControl<TextBox>("NameTextBox").Text;
            var reference = Client.Instance.From<Messages>();
            reference.Insert(new Messages { 
                Name = nameControl, 
                Message  = messageControl.Text, 
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd h:mm:tt") 
            });
            messageControl.Text = "";
        }
    }
}
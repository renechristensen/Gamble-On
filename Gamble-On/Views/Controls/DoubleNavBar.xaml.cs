using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Gamble_On.Views.Controls
{
    public partial class DoubleTopBar : ContentView
    {
        // Rufus Command
        public ICommand RufusCommand
        {
            get => (ICommand)GetValue(RufusCommandProperty);
            set => SetValue(RufusCommandProperty, value);
        }

        public static readonly BindableProperty RufusCommandProperty =
            BindableProperty.Create(nameof(RufusCommand), typeof(ICommand), typeof(DoubleTopBar));

        // DR.dk Command
        public ICommand DrDkCommand
        {
            get => (ICommand)GetValue(DrDkCommandProperty);
            set => SetValue(DrDkCommandProperty, value);
        }

        public static readonly BindableProperty DrDkCommandProperty =
            BindableProperty.Create(nameof(DrDkCommand), typeof(ICommand), typeof(DoubleTopBar));

        public DoubleTopBar()
        {
            InitializeComponent();
        }
    }
}

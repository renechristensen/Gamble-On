using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Gamble_On.Helpers
{
    public class ItemSelectedToCommandBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemSelectedToCommandBehavior), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemSelected += OnItemSelected;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemSelected -= OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Command == null)
                return;

            if (Command.CanExecute(e.SelectedItem))
                Command.Execute(e.SelectedItem);
        }
    }
}


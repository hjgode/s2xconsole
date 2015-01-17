using System;
using System.Windows;
using System.Windows.Input;
namespace S2XConsole.Commands
{
	public class CommandReference : Freezable, ICommand
	{
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandReference), new PropertyMetadata(new PropertyChangedCallback(CommandReference.OnCommandChanged)));
		public event EventHandler CanExecuteChanged;
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(CommandReference.CommandProperty);
			}
			set
			{
				base.SetValue(CommandReference.CommandProperty, value);
			}
		}
		public bool CanExecute(object parameter)
		{
			return this.Command != null && this.Command.CanExecute(parameter);
		}
		public void Execute(object parameter)
		{
			this.Command.Execute(parameter);
		}
		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CommandReference commandReference = d as CommandReference;
			ICommand command = e.OldValue as ICommand;
			ICommand command2 = e.NewValue as ICommand;
			if (command != null)
			{
				command.CanExecuteChanged -= commandReference.CanExecuteChanged;
			}
			if (command2 != null)
			{
				command2.CanExecuteChanged += commandReference.CanExecuteChanged;
			}
		}
		protected override Freezable CreateInstanceCore()
		{
			throw new NotImplementedException();
		}
	}
}

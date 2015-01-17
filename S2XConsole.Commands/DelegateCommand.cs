using System;
using System.Collections.Generic;
using System.Windows.Input;
namespace S2XConsole.Commands
{
	public class DelegateCommand : ICommand
	{
		private readonly Action _executeMethod;
		private readonly Func<bool> _canExecuteMethod;
		private bool _isAutomaticRequeryDisabled;
		private List<WeakReference> _canExecuteChangedHandlers;
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (!this._isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested += value;
				}
				CommandManagerHelper.AddWeakReferenceHandler(ref this._canExecuteChangedHandlers, value, 2);
			}
			remove
			{
				if (!this._isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested -= value;
				}
				CommandManagerHelper.RemoveWeakReferenceHandler(this._canExecuteChangedHandlers, value);
			}
		}
		public bool IsAutomaticRequeryDisabled
		{
			get
			{
				return this._isAutomaticRequeryDisabled;
			}
			set
			{
				if (this._isAutomaticRequeryDisabled != value)
				{
					if (value)
					{
						CommandManagerHelper.RemoveHandlersFromRequerySuggested(this._canExecuteChangedHandlers);
					}
					else
					{
						CommandManagerHelper.AddHandlersToRequerySuggested(this._canExecuteChangedHandlers);
					}
					this._isAutomaticRequeryDisabled = value;
				}
			}
		}
		public DelegateCommand(Action executeMethod) : this(executeMethod, null, false)
		{
		}
		public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : this(executeMethod, canExecuteMethod, false)
		{
		}
		public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
		{
			if (executeMethod == null)
			{
				throw new ArgumentNullException("executeMethod");
			}
			this._executeMethod = executeMethod;
			this._canExecuteMethod = canExecuteMethod;
			this._isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
		}
		public bool CanExecute()
		{
			return this._canExecuteMethod == null || this._canExecuteMethod();
		}
		public void Execute()
		{
			if (this._executeMethod != null)
			{
				this._executeMethod();
			}
		}
		public void RaiseCanExecuteChanged()
		{
			this.OnCanExecuteChanged();
		}
		protected virtual void OnCanExecuteChanged()
		{
			CommandManagerHelper.CallWeakReferenceHandlers(this._canExecuteChangedHandlers);
		}
		bool ICommand.CanExecute(object parameter)
		{
			return this.CanExecute();
		}
		void ICommand.Execute(object parameter)
		{
			this.Execute();
		}
	}
	public class DelegateCommand<T> : ICommand
	{
		private readonly Action<T> _executeMethod;
		private readonly Func<T, bool> _canExecuteMethod;
		private bool _isAutomaticRequeryDisabled;
		private List<WeakReference> _canExecuteChangedHandlers;
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (!this._isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested += value;
				}
				CommandManagerHelper.AddWeakReferenceHandler(ref this._canExecuteChangedHandlers, value, 2);
			}
			remove
			{
				if (!this._isAutomaticRequeryDisabled)
				{
					CommandManager.RequerySuggested -= value;
				}
				CommandManagerHelper.RemoveWeakReferenceHandler(this._canExecuteChangedHandlers, value);
			}
		}
		public bool IsAutomaticRequeryDisabled
		{
			get
			{
				return this._isAutomaticRequeryDisabled;
			}
			set
			{
				if (this._isAutomaticRequeryDisabled != value)
				{
					if (value)
					{
						CommandManagerHelper.RemoveHandlersFromRequerySuggested(this._canExecuteChangedHandlers);
					}
					else
					{
						CommandManagerHelper.AddHandlersToRequerySuggested(this._canExecuteChangedHandlers);
					}
					this._isAutomaticRequeryDisabled = value;
				}
			}
		}
		public DelegateCommand(Action<T> executeMethod) : this(executeMethod, null, false)
		{
		}
		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : this(executeMethod, canExecuteMethod, false)
		{
		}
		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
		{
			if (executeMethod == null)
			{
				throw new ArgumentNullException("executeMethod");
			}
			this._executeMethod = executeMethod;
			this._canExecuteMethod = canExecuteMethod;
			this._isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
		}
		public bool CanExecute(T parameter)
		{
			return this._canExecuteMethod == null || this._canExecuteMethod(parameter);
		}
		public void Execute(T parameter)
		{
			if (this._executeMethod != null)
			{
				this._executeMethod(parameter);
			}
		}
		public void RaiseCanExecuteChanged()
		{
			this.OnCanExecuteChanged();
		}
		protected virtual void OnCanExecuteChanged()
		{
			CommandManagerHelper.CallWeakReferenceHandlers(this._canExecuteChangedHandlers);
		}
		bool ICommand.CanExecute(object parameter)
		{
			if (parameter == null && typeof(T).IsValueType)
			{
				return this._canExecuteMethod == null;
			}
			return this.CanExecute((T)((object)parameter));
		}
		void ICommand.Execute(object parameter)
		{
			this.Execute((T)((object)parameter));
		}
	}
}

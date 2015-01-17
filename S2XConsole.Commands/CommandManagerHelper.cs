using System;
using System.Collections.Generic;
using System.Windows.Input;
namespace S2XConsole.Commands
{
	internal class CommandManagerHelper
	{
		internal static void CallWeakReferenceHandlers(List<WeakReference> handlers)
		{
			if (handlers != null)
			{
				EventHandler[] array = new EventHandler[handlers.Count];
				int num = 0;
				for (int i = handlers.Count - 1; i >= 0; i--)
				{
					WeakReference weakReference = handlers[i];
					EventHandler eventHandler = weakReference.Target as EventHandler;
					if (eventHandler == null)
					{
						handlers.RemoveAt(i);
					}
					else
					{
						array[num] = eventHandler;
						num++;
					}
				}
				for (int j = 0; j < num; j++)
				{
					EventHandler eventHandler2 = array[j];
					eventHandler2(null, EventArgs.Empty);
				}
			}
		}
		internal static void AddHandlersToRequerySuggested(List<WeakReference> handlers)
		{
			if (handlers != null)
			{
				foreach (WeakReference current in handlers)
				{
					EventHandler eventHandler = current.Target as EventHandler;
					if (eventHandler != null)
					{
						CommandManager.RequerySuggested += eventHandler;
					}
				}
			}
		}
		internal static void RemoveHandlersFromRequerySuggested(List<WeakReference> handlers)
		{
			if (handlers != null)
			{
				foreach (WeakReference current in handlers)
				{
					EventHandler eventHandler = current.Target as EventHandler;
					if (eventHandler != null)
					{
						CommandManager.RequerySuggested -= eventHandler;
					}
				}
			}
		}
		internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
		{
			CommandManagerHelper.AddWeakReferenceHandler(ref handlers, handler, -1);
		}
		internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
		{
			if (handlers == null)
			{
				handlers = ((defaultListSize > 0) ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
			}
			handlers.Add(new WeakReference(handler));
		}
		internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
		{
			if (handlers != null)
			{
				for (int i = handlers.Count - 1; i >= 0; i--)
				{
					WeakReference weakReference = handlers[i];
					EventHandler eventHandler = weakReference.Target as EventHandler;
					if (eventHandler == null || eventHandler == handler)
					{
						handlers.RemoveAt(i);
					}
				}
			}
		}
	}
}

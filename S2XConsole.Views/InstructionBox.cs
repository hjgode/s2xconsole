using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class InstructionBox : ComboBox, IComponentConnector
	{
		private TextBox textBox;
		private bool _contentLoaded;
		public InstructionBox()
		{
			this.InitializeComponent();
		}
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Down || e.Key == Key.Up)
			{
				bool isDown = e.Key == Key.Down;
				this.SetTextBoxCursor("PART_EditableTextBox", isDown);
				e.Handled = true;
				return;
			}
			base.OnPreviewKeyDown(e);
		}
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
			bool isDown = e.Delta < 0;
			this.SetTextBoxCursor("PART_EditableTextBox", isDown);
			e.Handled = true;
		}
		private void SetTextBoxCursor(string control, bool isDown)
		{
			try
			{
				if (this.textBox == null)
				{
					this.textBox = (TextBox)base.Template.FindName(control, this);
				}
				if (this.textBox != null)
				{
					string text = "\r\n";
					int num = this.textBox.Text.LastIndexOf(text, this.textBox.SelectionStart);
					int num2;
					if (num == -1)
					{
						num2 = this.textBox.SelectionStart;
						num = 0;
					}
					else
					{
						num2 = this.textBox.SelectionStart - num - text.Length;
					}
					int num3;
					int num4;
					if (!isDown)
					{
						num3 = this.textBox.Text.LastIndexOf(text, num);
						if (num3 == -1)
						{
							num3 = 0;
						}
						else
						{
							num3 += text.Length;
						}
						num4 = num;
					}
					else
					{
						num3 = this.textBox.Text.IndexOf(text, this.textBox.SelectionStart);
						if (num3 == -1)
						{
							return;
						}
						num3 += text.Length;
						num4 = this.textBox.Text.IndexOf(text, num3);
						if (num4 == -1)
						{
							num4 = this.textBox.Text.Length;
						}
					}
					int num5 = num4 - num3;
					if (num5 >= 0 && num5 < num2)
					{
						num2 = num5;
					}
					this.textBox.SelectionStart = num3 + num2;
				}
			}
			catch (Exception)
			{
			}
		}
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/S2XConsole;component/views/instructionbox.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}
	}
}

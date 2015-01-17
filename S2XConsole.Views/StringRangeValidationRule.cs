using System;
using System.Globalization;
using System.Windows.Controls;
namespace S2XConsole.Views
{
	public class StringRangeValidationRule : ValidationRule
	{
		private int _minimumLength = -1;
		private int _maximumLength = -1;
		private string _errorMessage;
		public int MinimumLength
		{
			get
			{
				return this._minimumLength;
			}
			set
			{
				this._minimumLength = value;
			}
		}
		public int MaximumLength
		{
			get
			{
				return this._maximumLength;
			}
			set
			{
				this._maximumLength = value;
			}
		}
		public string ErrorMessage
		{
			get
			{
				return this._errorMessage;
			}
			set
			{
				this._errorMessage = value;
			}
		}
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			ValidationResult result = new ValidationResult(true, null);
			string text = (value ?? string.Empty).ToString();
			if (text.Length < this.MinimumLength || (this.MaximumLength > 0 && text.Length > this.MaximumLength))
			{
				result = new ValidationResult(false, this.ErrorMessage);
			}
			return result;
		}
	}
}

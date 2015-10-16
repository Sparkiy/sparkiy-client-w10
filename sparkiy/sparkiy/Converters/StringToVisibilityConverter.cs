using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace sparkiy.Converters
{
	/// <summary>
	/// <see cref="String"/> to <see cref="Visibility"/> converter.
	/// Sets visibility to <see cref="Visibility.Collapsed"/> is string is empty.
	/// </summary>
	public sealed class StringToVisibilityConverter : IValueConverter
	{
		/// <summary>
		/// Converts the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="language">The language.</param>
		/// <returns>Returns <see cref="Visibility.Collapsed"/> if given value is an empty string or null; <see cref="Visibility.Visible"/> otherwise.</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!(value is string))
				return Visibility.Collapsed;
			return !string.IsNullOrWhiteSpace(value.ToString()) ? Visibility.Visible : Visibility.Collapsed;
		}

		/// <summary>
		/// Converts the back.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="language">The language.</param>
		/// <returns></returns>
		/// <exception cref="System.NotSupportedException">Not supported operation.</exception>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
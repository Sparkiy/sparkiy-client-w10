using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace sparkiy.Converters
{
	/// <summary>
	/// <see cref="Boolean"/> to <see cref="Visibility"/> converter.
	/// </summary>
	public sealed class BooleanToVisibilityConverter : IValueConverter
	{
		/// <summary>
		/// Converts boolean value to visibility enum value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="language">The language.</param>
		/// <returns>Returns <see cref="Visibility.Collapsed"/> if provided value is not of type <see cref="Boolean"/> or is <c>False</c>; <see cref="Visibility.Visible"/> otherwise.</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!(value is bool))
				return Visibility.Collapsed;
			return (bool) value ? Visibility.Visible : Visibility.Collapsed;
		}

		/// <summary>
		/// Converts visibility enum value to boolean value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="language">The language.</param>
		/// <returns>Returns <c>False</c> if provided value is not of type <see cref="Visibility"/> or is <see cref="Visibility.Collapsed"/>; <c>True</c> otherwise.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value as Visibility? == Visibility.Visible;
		}
	}
}

using System;
using Windows.UI.Xaml;

namespace sparkiy.Extensions
{
	/// <summary>
	/// <see cref="ResourceDictionary"/> extensions.
	/// </summary>
	public static class ResourceDictionaryExtensions
	{
		/// <summary>
		/// Tries the get value.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="resourceDictionary">The resource dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>Returns <c>True</c> if value was retrieved and casted to requested value type successfully; <c>False</c> otherwise.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// resourceDictionary
		/// or
		/// key
		/// </exception>
		public static bool TryGetValue<T>(this ResourceDictionary resourceDictionary, object key, out T value) 
		{
			// Check arguments
			if (resourceDictionary == null) throw new ArgumentNullException(nameof(resourceDictionary));
			if (key == null) throw new ArgumentNullException(nameof(key));
			
			// Retrieve object with given key
			object obj;
			if (resourceDictionary.TryGetValue(key, out obj))
			{
				try
				{
					// Cast object to desired type
					value = (T) obj;

					return true;
				}
				catch
				{
					// Failed to cast
					// This will continue to execute and set default value and return false
				}
			}

			// Set default value if unable to cast
			value = default(T);

			return false;
		}
	}
}

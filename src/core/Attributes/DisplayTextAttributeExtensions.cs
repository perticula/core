// perticula - core - DisplayTextAttributeExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Attributes
{
	/// <summary>
	///   Class DisplayTextAttributeExtensions.
	///   Methods for selecting and operating on text values attached to enumerations
	/// </summary>
	public static class DisplayTextAttributeExtensions
	{
		/// <summary>
		///   Returns the display text associated with this enum (must be decorated
		///   with [DisplayText( ... )] )
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.ArgumentNullException">value</exception>
		public static string DisplayText(this Enum? value)
		{
			if (value == null) return "";
			return value
			       .GetType()
			       .GetField(value.ToString())
			       ?.GetCustomAttributes(typeof(DisplayTextAttribute), false)
			       .Cast<DisplayTextAttribute>()
			       .Take(1)
			       .Select(attr => attr.DisplayText)
			       .FirstOrDefault() ?? "";
		}

		/// <summary>
		///   Returns the display text associated with this enum (must be decorated
		///   with [DisplayText( ... )] )
		///   and formats the found value with the supplied params
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="arguments">The format arguments.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.ArgumentNullException">value</exception>
		/// <exception cref="FormatException">value cannot be null</exception>
		public static string DisplayText(this Enum? value, params object[] arguments)
		{
			if (value == null) return "";
			var attribute = value
			                .GetType()
			                .GetField(value.ToString())
			                ?.GetCustomAttributes(typeof(DisplayTextAttribute), false)
			                .Cast<DisplayTextAttribute>()
			                .Take(1)
			                .Select(attr => attr)
			                .FirstOrDefault();

			if (string.IsNullOrEmpty(attribute?.DisplayText)) return attribute?.DefaultText ?? "";

			var formatted = string.Format(attribute.DisplayText, arguments);
			return string.IsNullOrEmpty(formatted) ? attribute.DefaultText : formatted;
		}

		/// <summary>
		///   Finds the (first) enum value matching the display text.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns>T.</returns>
		/// <exception cref="System.InvalidCastException"></exception>
		public static T FromDisplayText<T>(this string value)
			where T : struct, IConvertible
		{
			foreach (Enum e in Enum.GetValues(typeof(T)))
			{
				//check to see if entire string matches
				var dt = e.DisplayText();
				if (string.Equals(dt, value, StringComparison.OrdinalIgnoreCase)) return (T) Enum.Parse(typeof(T), e.ToString(), true);

				//check to see if multiple values match (e.g. 'Hotels/Lodging' matches either 'hotels' or 'lodging')
				if (!dt.Contains('/')) continue;
				var splitValues = dt.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
				if (splitValues.Any(split => string.Equals(split, value, StringComparison.OrdinalIgnoreCase))) return (T) Enum.Parse(typeof(T), e.ToString(), true);
			}

			//check to see if we can parse to enum directly
			if (Enum.TryParse(value, true, out T test)) return test;

			//nothing matches
			throw new InvalidCastException();
		}
	}
}

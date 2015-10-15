using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sparkiy.Logging
{
	/// <summary>
	/// Debug text writer for SeriLog logging.
	/// </summary>
	public class DebugTextWriter : TextWriter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DebugTextWriter"/> class.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		public DebugTextWriter(Encoding encoding)
		{
			Encoding = encoding;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DebugTextWriter"/> class.
		/// </summary>
		public DebugTextWriter()
		{
		}

		/// <summary>
		/// Writes a character to the text string or stream.
		/// </summary>
		/// <param name="value">The character to write to the text stream.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void Write(char value)
		{
			Debug.Write(value);
		}

		/// <summary>
		/// Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
		/// </summary>
		public override void Flush()
		{
		}

		/// <summary>
		/// Asynchronously clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
		/// </summary>
		/// <returns>
		/// A task that represents the asynchronous flush operation.
		/// </returns>
#pragma warning disable 1998 // Method has empty implementation
#pragma warning disable 1998 // Method has empty implementation
		public override async Task FlushAsync()
#pragma warning restore 1998
#pragma warning restore 1998
		{
		}

		/// <summary>
		/// Writes the specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		public override void Write(char[] buffer)
		{
			Debug.Write(buffer);
		}

		/// <summary>
		/// Writes the text representation of a Boolean value to the text string or stream.
		/// </summary>
		/// <param name="value">The Boolean value to write.</param>
		public override void Write(bool value)
		{
			Debug.Write(value);
		}

		/// <summary>
		/// Writes the specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		public override void Write(char[] buffer, int index, int count)
		{
			Debug.Write(buffer.Skip(index).Take(count));
		}

		/// <summary>
		/// Writes a string to the text string or stream.
		/// </summary>
		/// <param name="value">The string to write.</param>
		public override void Write(string value)
		{
			Debug.Write(value);
		}

		/// <summary>
		/// Writes the text representation of an object to the text string or stream by calling the ToString method on that object.
		/// </summary>
		/// <param name="value">The object to write.</param>
		public override void Write(object value)
		{
			Debug.Write(value);
		}

		/// <summary>
		/// Writes the specified format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg">The argument.</param>
		public override void Write(string format, params object[] arg)
		{
			Debug.Write(string.Format(this.FormatProvider, format, arg));
		}

		/// <summary>
		/// Writes a line terminator to the text string or stream.
		/// </summary>
		public override void WriteLine()
		{
			Debug.WriteLine(string.Empty);
		}

		/// <summary>
		/// Writes a string followed by a line terminator to the text string or stream.
		/// </summary>
		/// <param name="value">The string to write. If <paramref name="value" /> is null, only the line terminator is written.</param>
		public override void WriteLine(string value)
		{
			Debug.WriteLine(value);
		}

		/// <summary>
		/// Writes the text representation of an object by calling the ToString method on that object, followed by a line terminator to the text string or stream.
		/// </summary>
		/// <param name="value">The object to write. If <paramref name="value" /> is null, only the line terminator is written.</param>
		public override void WriteLine(object value)
		{
			Debug.WriteLine(value);
		}


		/// <summary>
		/// When overridden in a derived class, returns the character encoding in which the output is written.
		/// </summary>
		public override Encoding Encoding { get; }
	}
}
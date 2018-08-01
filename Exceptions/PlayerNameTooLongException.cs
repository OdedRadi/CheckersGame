using System;
using System.Runtime.Serialization;

namespace CheckersGame
{
	[Serializable]
	internal class PlayerNameTooLongException : Exception
	{
		public PlayerNameTooLongException()
		{
		}

		public PlayerNameTooLongException(string message) : base(message)
		{
		}

		public PlayerNameTooLongException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected PlayerNameTooLongException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
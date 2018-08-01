using System;
using System.Runtime.Serialization;

namespace CheckersGame
{
	[Serializable]
	internal class PlayerNameWithSpacesException : Exception
	{
		public PlayerNameWithSpacesException()
		{
		}

		public PlayerNameWithSpacesException(string message) : base(message)
		{
		}

		public PlayerNameWithSpacesException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected PlayerNameWithSpacesException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
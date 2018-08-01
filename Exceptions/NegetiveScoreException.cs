using System;
using System.Runtime.Serialization;

namespace CheckersGame
{
	[Serializable]
	internal class NegetiveScoreException : Exception
	{
		public NegetiveScoreException()
		{
		}

		public NegetiveScoreException(string message) : base(message)
		{
		}

		public NegetiveScoreException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NegetiveScoreException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
using System;
using System.Text;

namespace CheckersGame
{
	public struct Move
	{
		public enum eMoveResult
		{
			Skipping,
			Moving,
			Invalid
		}

		private Coordinate m_FromCoordinate;
		private Coordinate m_ToCoordinate;

		public Move(Coordinate i_FromCoordinate, Coordinate i_ToCoordinate)
		{
			m_FromCoordinate = i_FromCoordinate;
			m_ToCoordinate = i_ToCoordinate;
		}

		public static bool TryParse(string i_InputString, out Move o_Move)
		{
			bool validInput = false;

			o_Move = new Move();
			if (i_InputString.Contains(">"))
			{
				string[] inputSplitedStrings = i_InputString.Split('>');

				if (inputSplitedStrings.Length == 2)
				{
					string fromCoordianteString = inputSplitedStrings[0];
					string toCoordianteString = inputSplitedStrings[1];

					if (fromCoordianteString.Length == 2 && toCoordianteString.Length == 2)
					{
						char fromColCharatcter = fromCoordianteString[0];
						char fromRowCharatcter = fromCoordianteString[1];
						char toColCharatcter = toCoordianteString[0];
						char toRowCharatcter = toCoordianteString[1];

						if (char.IsUpper(fromColCharatcter) &&
							char.IsLower(fromRowCharatcter) &&
							char.IsUpper(toColCharatcter) &&
							char.IsLower(toRowCharatcter))
						{
							int fromColNum = fromColCharatcter - 'A';
							int fromRowNum = fromRowCharatcter - 'a';
							int toColNum = toColCharatcter - 'A';
							int toRowNum = toRowCharatcter - 'a';

							validInput = true;
							o_Move = new Move(new Coordinate(fromRowNum, fromColNum), new Coordinate(toRowNum, toColNum));
						}
					}
				}
			}

			return validInput;
		}

		public Coordinate FromCoordinate
		{
			get
			{
				return m_FromCoordinate;
			}
		}

		public Coordinate ToCoordinate
		{
			get
			{
				return m_ToCoordinate;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append(char.ConvertFromUtf32(FromCoordinate.Column + 'A'));
			stringBuilder.Append(char.ConvertFromUtf32(FromCoordinate.Row + 'a'));
			stringBuilder.Append('>');
			stringBuilder.Append(char.ConvertFromUtf32(ToCoordinate.Column + 'A'));
			stringBuilder.Append(char.ConvertFromUtf32(ToCoordinate.Row + 'a'));

			return stringBuilder.ToString();
		}
	}
}
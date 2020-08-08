using System;


namespace teleport
{
	public class Progressbar
	{
		public int Max { get; set; }
		public int progress = 0;
		private int position;
		public String Title = String.Empty;

		public Progressbar(string title,int max)
		{
			Max = max;
			Title = title;
		}


		public void Start()
		{
			
			position = Console.CursorTop;
			ChangePercent(0);
		}
	
	
		public void Change(int absoute)
        {
			int percent = absoute / (Max / 100);
			ChangePercent(percent);
        }

		public void ChangePercent(int percent)
		{
			percent = (percent > 100 ? 100 : percent);
			int oldPosition = Console.CursorTop;
			Console.CursorTop = position;
			Console.CursorLeft = 0;
			Console.WriteLine($"{Title}[{new String('=', percent)}{new String(' ', 100 - percent)}]{percent}%");
			Console.CursorTop = oldPosition;

		}
	}
}

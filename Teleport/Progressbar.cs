using System;


namespace Teleport
{
	public class Progressbar
	{
		public long Max { get; set; }
		public int progress = 0;
		private int position;
		public String Title = String.Empty;

		public Progressbar(string title,long max)
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
			float percent = 0;
			if (absoute > 0)
            {
				percent = absoute / (Max / 100f);
				ChangePercent((int)percent);
				
            }
        }

		public void ChangePercent(int percent)
		{
			percent = (percent > 100 ? 100 : percent);
			int oldPosition = Console.CursorTop;
			Console.CursorTop = position;
			Console.Write(new String(' ', Console.WindowLeft));
			Console.CursorLeft = 0;
			Console.WriteLine($"{Title}[{new String('=', percent)}{new String(' ', 100 - percent)}]{percent}%");
			Console.CursorTop = oldPosition;

		}
	}
}

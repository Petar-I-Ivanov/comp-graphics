using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
	[Serializable]
	public class StarShape : Shape
	{

		#region Constructor

		public StarShape(Rectangle rect) : base(rect)
		{
		}

		public StarShape(StarShape polygon) : base(polygon)
		{
			this.StartingWidth = 200;
			this.StartingHeight = 200;
		}

		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{
			PointF[] points = starPoints();
			this.Transform.TransformPoints(points);

			bool result = false;
			int j = points.Count() - 1;
			for (int i = 0; i < points.Count(); i++)
			{
				if (points[i].Y < point.Y && points[j].Y >= point.Y || points[j].Y < point.Y && points[i].Y >= point.Y)
				{
					if (points[i].X + (point.Y - points[i].Y) / (points[j].Y - points[i].Y) * (points[j].X - points[i].X) < point.X)
					{
						result = !result;
					}
				}
				j = i;
			}
			return result;
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			PointF[] points = starPoints();

			Pen pen = new Pen(BorderColor);
			pen.Width = BorderWidth;
			pen.Color = Color.FromArgb(TransparencyLevel, pen.Color.R, pen.Color.G, pen.Color.B);

			SolidBrush brush = new SolidBrush(FillColor);
			brush.Color = Color.FromArgb(TransparencyLevel, brush.Color.R, brush.Color.G, brush.Color.B);

			grfx.FillPolygon(brush, points);
			grfx.DrawPolygon(pen, points);

		}

		private PointF[] starPoints()
        {
			PointF[] points = new PointF[8];

			//points[0] = new PointF(0, 100);
			points[0] = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height / 2);

			//points[1] = new PointF(80, 80);
			points[1] = new PointF(Rectangle.X + Rectangle.Width / 2 - 20, Rectangle.Y + Rectangle.Height / 2 - 20);

			//points[2] = new PointF(100, 0);
			points[2] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y);

			//points[3] = new PointF(120, 80);
			points[3] = new PointF(Rectangle.X + Rectangle.Width / 2 + 20, Rectangle.Y + Rectangle.Height / 2 - 20);

			//points[4] = new PointF(200, 100);
			points[4] = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height / 2);

			//points[5] = new PointF(120, 120);
			points[5] = new PointF(Rectangle.X + Rectangle.Width / 2 + 20, Rectangle.Y + Rectangle.Height / 2 + 20);

			//points[6] = new PointF(100, 200);
			points[6] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height);

			//points[7] = new PointF(80, 120);
			points[7] = new PointF(Rectangle.X + Rectangle.Width / 2 - 20, Rectangle.Y + Rectangle.Height / 2 + 20);

			return points;
		}
	}
}

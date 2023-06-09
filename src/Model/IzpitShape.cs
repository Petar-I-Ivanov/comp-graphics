using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
    [Serializable]
    class IzpitShape : Shape
    {
		public IzpitShape(Rectangle rect) : base(rect)
		{
		}

		public IzpitShape(IzpitShape polygon) : base(polygon)
		{
			this.StartingWidth = 200;
			this.StartingHeight = 200;
		}

		public override bool Contains(PointF point)
		{
			PointF[] points = izpitPoints();
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

		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			PointF[] points = izpitPoints();

			Pen pen = new Pen(BorderColor);
			pen.Width = BorderWidth;
			pen.Color = Color.FromArgb(TransparencyLevel, pen.Color.R, pen.Color.G, pen.Color.B);

			SolidBrush brush = new SolidBrush(FillColor);
			brush.Color = Color.FromArgb(TransparencyLevel, brush.Color.R, brush.Color.G, brush.Color.B);

			grfx.FillPolygon(brush, points);
			grfx.DrawPolygon(pen, points);

		}

		private PointF[] izpitPoints()
        {
			PointF[] points = new PointF[10];

			//points[0] = new PointF(100, 0);
			points[0] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y);

			//points[1] = new PointF(100, 100);
			points[1] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);

			//points[0] = new PointF(100, 0);
			points[2] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y);


			//points[1] = new PointF(200, 100);
			points[3] = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height / 2);

			//points[1] = new PointF(100, 100);
			points[4] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);

			//points[1] = new PointF(200, 100);
			points[5] = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height / 2);


			//points[2] = new PointF(100, 200);
			points[6] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height);

			//points[3] = new PointF(0, 100);
			points[7] = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height / 2);

			//points[1] = new PointF(100, 100);
			points[8] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);

			//points[3] = new PointF(0, 100);
			points[9] = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height / 2);

			return points;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
	/// <summary>
	/// Класът елипса е основен примитив, който е наследник на базовия Shape.
	/// </summary>
	[Serializable]
	public class EllipseShape : Shape
	{
		#region Constructor

		public EllipseShape(Rectangle rect) : base(rect)
		{
		}

		public EllipseShape(EllipseShape ellipse) : base(ellipse)
		{
			this.StartingWidth = 100;
			this.StartingHeight = 200;
		}

		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към елипсата.
		/// В случая на елипса този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{
			PointF[] pointToConvert = new PointF[] { point };
			this.Transform.Invert();
			this.Transform.TransformPoints(pointToConvert);
			this.Transform.Invert();

			Point center = new Point((int)((int) base.Rectangle.X + (base.Width / 2)),
                (int)(base.Rectangle.Y + base.Height / 2));

			double _xRadius = base.Width / 2;
			double _yRadius = base.Height / 2;

			if (_xRadius <= 0.0 || _yRadius <= 0.0)
				return false;

			Point normalized = new Point((int)(pointToConvert[0].X - center.X),
                                         (int)(pointToConvert[0].Y - center.Y));

			return ((double)(normalized.X * normalized.X)
				/ (_xRadius * _xRadius)) + ((double)(normalized.Y * normalized.Y) / (_yRadius * _yRadius))
				<= 1.0;
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			Pen pen = new Pen(BorderColor);
			pen.Width = BorderWidth;
			pen.Color = Color.FromArgb(TransparencyLevel, pen.Color.R, pen.Color.G, pen.Color.B);

			SolidBrush brush = new SolidBrush(FillColor);
			brush.Color = Color.FromArgb(TransparencyLevel, brush.Color.R, brush.Color.G, brush.Color.B);

			grfx.FillEllipse(brush, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.DrawEllipse(pen, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
		}
	}
}

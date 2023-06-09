using Draw.src.Model;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
	/// <summary>
	/// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
	/// </summary>
	[Serializable]
	public abstract class Shape
	{
		#region Constructors
		
		public Shape()
		{
		}
		
		public Shape(RectangleF rect)
		{
			rectangle = rect;

			if (this.GetType() != typeof(GroupShape))
            {
				this.FillColor = Color.White;
				this.BorderWidth = 1;
				this.BorderColor = Color.Black;
				this.StartingWidth = (int)rect.Width;
				this.StartingHeight = (int)rect.Height;
			}
		}
		
		public Shape(Shape shape)
		{
			this.Height = shape.Height;
			this.Width = shape.Width;
			this.SizePercentage = shape.SizePercentage * 100;

			this.Location = new PointF(5, 5);
			this.Rotate(shape.Rotation);
			this.rectangle = new Rectangle(5, 5, (int) shape.Rectangle.Width, (int) shape.Rectangle.Height);
			
			this.FillColor =  shape.FillColor;
			this.BorderWidth = shape.BorderWidth;
			this.BorderColor = shape.BorderColor;
			this.TransparencyLevel = shape.TransparencyLevel;
		}
		#endregion

		#region Properties

		private int startingWidth;
		public int StartingWidth
        {
			get { return this.startingWidth; }
			set { startingWidth = value; }
        }

		private int startingHeight;
		public int StartingHeight
		{
			get { return this.startingHeight; }
			set { startingHeight = value; }
		}

		/// <summary>
		/// Обхващащ правоъгълник на елемента.
		/// </summary>
		private RectangleF rectangle;
		public virtual RectangleF Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}

        /// <summary>
        /// Широчина на елемента.
        /// </summary>
        public virtual float Width {
			get { return Rectangle.Width; }
			set { rectangle.Width = value; }
		}
		
		/// <summary>
		/// Височина на елемента.
		/// </summary>
		public virtual float Height {
			get { return Rectangle.Height; }
			set { rectangle.Height = value; }
		}
		
		/// <summary>
		/// Горен ляв ъгъл на елемента.
		/// </summary>
		public virtual PointF Location {
			get { return Rectangle.Location; }
			set { rectangle.Location = value; }
		}
		
		/// <summary>
		/// Цвят на елемента.
		/// </summary>
		private Color fillColor;		
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}

		/// <summary>
		/// Цвят на bordera на елемента.
		/// </summary>
		private Color borderColor;
		public virtual Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; }
		}

		/// <summary>
		/// Големина на bordera на елемента.
		/// </summary>
		private int borderWidth;
		public virtual int BorderWidth
		{
			get { return borderWidth; }
			set { borderWidth = value; }
		}

		private int transparencyLevel = 255;
		public virtual int TransparencyLevel
        {
			get { return transparencyLevel; }
			set { transparencyLevel = value; }
        }

		public virtual PointF Center
		{
			get { return new PointF(this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2); }
		}

		private CustomMatrix transform = new CustomMatrix();
		public virtual Matrix Transform
		{
			get { return transform.matrix; }
			set { transform.matrix = value; }
		}

		private int rotation = 0;
		public int Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}

		private float sizePercentage = 1;
		public virtual float SizePercentage
        {
			get { return sizePercentage; }
			set { sizePercentage = value / 100; }
        }

		#endregion

		public virtual void Rotate(int rotationAngle)
		{
			int differenceRotation = rotationAngle - rotation;
			this.Transform.RotateAt(differenceRotation, Center);
			rotation += differenceRotation;
		}

		public void GroupRotate(int differenceRotation, PointF center)
        {
			this.Transform.RotateAt(differenceRotation, center);
			rotation += differenceRotation;
		}

		public virtual void Translate(PointF point)
        {
			int backRotation = rotation;
			Transform.RotateAt(-backRotation, Center);

			this.Location = new PointF(this.Location.X + point.X, this.Location.Y + point.Y);

			Transform.RotateAt(backRotation, Center);
		}

		public virtual void Resize()
		{
			Width = (float) startingWidth * sizePercentage;
			Height = (float) startingHeight * sizePercentage;
		}

		/// <summary>
		/// Проверка дали точка point принадлежи на елемента.
		/// </summary>
		/// <param name="point">Точка</param>
		/// <returns>Връща true, ако точката принадлежи на елемента и
		/// false, ако не пренадлежи</returns>
		public virtual bool Contains(PointF point)
		{
			PointF[] pointsToConvert = { point };
			this.Transform.Invert();
			this.Transform.TransformPoints(pointsToConvert);
			this.Transform.Invert();

			return Rectangle.Contains(pointsToConvert[0].X, pointsToConvert[0].Y);
		}
		
		/// <summary>
		/// Визуализира елемента.
		/// </summary>
		/// <param name="grfx">Къде да бъде визуализиран елемента.</param>
		public virtual void DrawSelf(Graphics grfx)
		{
			grfx.Transform = this.Transform;
		}
	}
}

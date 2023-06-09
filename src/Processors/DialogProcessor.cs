using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection {
			get { return selection; }
			set { selection = value; }
		}

		private Shape buffer;
		public Shape Buffer
		{
			get { return buffer; }
			set { buffer = value; }
		}

		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}

		/// <summary>
		/// Големината на екрана на потребителя.
		/// Използва се за определяне на граници за генериране на Shape.
		/// </summary>
		private Size screenSize;
		public Size SceneSize
		{
			get { return screenSize; }
			set { screenSize = value; }

		}

		#endregion

		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(screenSize.Width - 200);
			int y = rnd.Next(screenSize.Height - 200);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
			ShapeList.Add(rect);
		}

		/// <summary>
		/// Добавя примитив - елипса на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomEllipse()
		{
			Random rnd = new Random();
			int x = rnd.Next(screenSize.Width - 200);
			int y = rnd.Next(screenSize.Height - 200);

			EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
			ShapeList.Add(ellipse);
		}

		/// <summary>
		/// Добавя примитив - звезда на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomStar()
		{
			Random rnd = new Random();
			int x = rnd.Next(screenSize.Width - 200);
			int y = rnd.Next(screenSize.Height - 200);

			StarShape star = new StarShape(new Rectangle(x, y, 200, 200));
			ShapeList.Add(star);
		}

		public void AddRandomIzpitShape()
		{
			Random rnd = new Random();
			int x = rnd.Next(screenSize.Width - 200);
			int y = rnd.Next(screenSize.Height - 150);

			IzpitShape shape = new IzpitShape(new Rectangle(x, y, 200, 150));
			ShapeList.Add(shape);
		}

		public void AddShape(Shape shape)
        {
			Shape newShape = null;

			if (shape.GetType() == typeof(RectangleShape))
				newShape = new RectangleShape((RectangleShape) shape);

			if (shape.GetType() == typeof(EllipseShape))
				newShape = new EllipseShape((EllipseShape)shape);

			if (shape.GetType() == typeof(StarShape))
				newShape = new StarShape((StarShape)shape);

			if (shape.GetType() == typeof(GroupShape))
				newShape = new GroupShape((GroupShape)shape);

			ShapeList.Add(newShape);
		}

		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					//ShapeList[i].FillColor = Color.Red;
						
					return ShapeList[i];
				}	
			}
			return null;
		}

		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			if (selection.Count > 0)
			{
				PointF offset = new PointF(p.X - lastLocation.X, p.Y - lastLocation.Y);

				foreach (Shape selected in selection)
				{
					selected.Translate(offset);
				}

				lastLocation = p;
			}
		}

		public void SetBorderColor(Color color)
		{
			if (selection.Count > 0)
            {
				foreach (Shape selected in selection)
				{
					selected.BorderColor = color;
				}
			}
		}

		public void SetFillColor(Color color)
		{
			if (selection.Count > 0)
			{
				foreach (Shape selected in selection)
				{
					selected.FillColor = color;
				}
			}
		}

		public void SetBorderWidth(int borderWidth)
		{
			if (selection.Count > 0)
			{
				foreach (Shape selected in selection)
				{
					selected.BorderWidth = borderWidth;
				}
			}
		}

		public void SetTransparencyLevel(int transparencyLevel)
        {
			if (selection.Count > 0)
			{
				foreach (Shape selected in selection)
				{
					selected.TransparencyLevel = transparencyLevel;
				}
			}
		}

		public void RotateAt(int rotationAngle)
		{
			if (selection.Count > 0)
			{
				foreach (Shape selected in selection)
				{
					selected.Rotate(rotationAngle);
				}
			}
		}

		public void Resize(int persentage)
		{
			if (selection.Count > 0)
			{
				foreach (Shape selected in selection)
				{
					selected.SizePercentage = persentage;
					selected.Resize();
				}
			}
		}

		public void GroupSelected()
        {
			if (Selection.Count < 2) return;

			float minX = float.MaxValue;
			float minY = float.MaxValue;
			float maxX = float.MinValue;
			float maxY = float.MinValue;

			foreach (Shape shape in Selection)
            {
				if (minX > shape.Location.X) minX = shape.Location.X;
				if (minY > shape.Location.Y) minY = shape.Location.Y;
				if (maxX < shape.Location.X + shape.Width) maxX = shape.Location.X + shape.Width;
				if (maxY < shape.Location.Y + shape.Height) maxY = shape.Location.Y + shape.Height;
			}

			GroupShape group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
			group.StartingWidth = (int) (maxX - minX);
			group.StartingHeight = (int) (maxY - minY);

			group.SubShapes = Selection;
			Selection = new List<Shape>();
			Selection.Add(group);

			foreach (Shape shape in group.SubShapes)
				ShapeList.Remove(shape);

			ShapeList.Add(group);
        }

		public void UngroupSelected()
        {
			if (Selection.Count > 0)
			{
				foreach (Shape shape in selection)
				{
					if (shape.GetType() == typeof(GroupShape))
					{
						foreach (Shape groupShape in ((GroupShape)shape).SubShapes)
						{
							ShapeList.Add(groupShape);
						}

						((GroupShape)shape).SubShapes.Clear();
						ShapeList.Remove(shape);

					}
				}

				selection.Clear();
			}
		}

		public void Delete()
        {
			foreach (Shape sh in selection)
				ShapeList.Remove(sh);

			selection.Clear();
		}

		public void SelectAll()
        {
			Selection.Clear();

			foreach (Shape sh in ShapeList)
				Selection.Add(sh);
        }

		public void InvertedSelect()
		{
			List<Shape> temp = selection.ConvertAll(shape => shape);
			selection.Clear();

			foreach (Shape shape in ShapeList)
            {
				if (!temp.Contains(shape))
                {
					selection.Add(shape);
                }
            }
		}

		internal void Save(string filename)
		{
			using (FileStream filestream = new FileStream(filename, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(filestream, ShapeList);
			}
		}

		internal void Open(string filename)
		{
			using (FileStream filestream = new FileStream(filename, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				ShapeList = (List<Shape>)binaryFormatter.Deserialize(filestream);
			}
		}

		public override void Draw(Graphics grfx)
        {
            base.Draw(grfx);

			float[] dashValues = { 4, 2 };
			Pen dashPen = new Pen(Color.Black, 3);
			dashPen.DashPattern = dashValues;

			if (Selection.Count > 0)
			{
				foreach (Shape selected in selection)
				{
					grfx.Transform = selected.Transform;

					grfx.DrawRectangle(
					dashPen,
					selected.Location.X - 3,
					selected.Location.Y - 3,
					selected.Width + 6,
					selected.Height + 6
					);
				}
			}
        }
    }
}

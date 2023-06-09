using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Draw.src.Model
{
    [Serializable]
    class GroupShape : Shape
    {
		#region Constructor

		public GroupShape(RectangleF rect) : base(rect)
		{
		}

        public GroupShape(GroupShape shape)
        {
            foreach (Shape itemShape in shape.SubShapes)
            {
                Shape addShape = null;

                if (itemShape.GetType() == typeof(RectangleShape))
                    addShape = new RectangleShape((RectangleShape)itemShape);

                if (itemShape.GetType() == typeof(EllipseShape))
                    addShape = new EllipseShape((EllipseShape)itemShape);

                if (itemShape.GetType() == typeof(StarShape))
                    addShape = new StarShape((StarShape)itemShape);

                this.subShapes.Add(addShape);
            }

            this.Location = new PointF(5, 5);
            this.Rotate(shape.Rotation);
            this.Rectangle = new Rectangle(5, 5, (int)shape.Rectangle.Width, (int)shape.Rectangle.Height);
        }

        #endregion

        private List<Shape> subShapes = new List<Shape>();
		public List<Shape> SubShapes
        {
			get { return this.subShapes; }
			set { this.subShapes = value; }
        }

        public override Color FillColor
		{ 
			get => base.FillColor;
			set
			{
				base.FillColor = value;
				foreach (Shape shape in SubShapes)
					shape.FillColor = value;
			}
		}

        public override Color BorderColor
        {
            get => base.BorderColor;
            set
            {
                base.BorderColor = value;
                foreach (Shape shape in SubShapes)
                    shape.BorderColor = value;
            }
        }

        public override int BorderWidth
        {
            get => base.BorderWidth;
            set
            {
                base.BorderWidth = value;
                foreach (Shape shape in SubShapes)
                    shape.BorderWidth = value;
            }
        }

        public override int TransparencyLevel
        {
            get => base.TransparencyLevel;
            set
            {
                base.TransparencyLevel = value;
                foreach (Shape shape in SubShapes)
                    shape.TransparencyLevel = value;
            }
        }

        public override Matrix Transform
        {
            get => base.Transform;
			set
			{
				base.Transform.Multiply(value);
				foreach (Shape shape in SubShapes)
					shape.Transform.Multiply(value);
			}
        }

        public override float SizePercentage
        {
            get => base.SizePercentage;
            set
            {
                base.SizePercentage = value;
                foreach (Shape shape in SubShapes)
                    shape.SizePercentage = value;
            }
        }

        public override void Rotate(int rotationAngle)
        {
            foreach (Shape shape in SubShapes)
            {
                shape.GroupRotate(rotationAngle - this.Rotation, Center);
            }

            base.Rotate(rotationAngle);
        }
        public override void Resize()
        {
            base.Resize();
            foreach (Shape shape in SubShapes)
                shape.Resize();
        }

        public override void Translate(PointF point)
        {
            base.Translate(point);

            foreach (Shape shape in SubShapes)
            {
                shape.Translate(point);
            }
        }

		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			foreach (Shape shape in SubShapes) {
				shape.DrawSelf(grfx);
            }
		}
	}
}

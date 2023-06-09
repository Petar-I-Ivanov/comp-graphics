using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//

			dialogProcessor.SceneSize = this.Size;
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}

		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			viewPort.Invalidate();
		}

		/// <summary>
		/// Бутон, който поставя на произволно място елипса със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		private void DrawEllipseSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomEllipse();
			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";
			viewPort.Invalidate();
		}

		/// <summary>
		/// Бутон, който поставя на произволно място звезда със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		private void DrawStarSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomStar();
			statusBar.Items[0].Text = "Последно действие: Рисуване на звезда";
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked)
			{

				Shape currentShape = dialogProcessor.ContainsPoint(e.Location);

				if (currentShape != null && !dialogProcessor.Selection.Contains(currentShape))
				{

					dialogProcessor.Selection.Add(currentShape);
					statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
				}

				else if (currentShape != null && dialogProcessor.Selection.Contains(currentShape))
				{
					dialogProcessor.Selection.Remove(currentShape);
					statusBar.Items[0].Text = "Деселектиране на обект.";
				}

				else
				{
					dialogProcessor.Selection.Clear();
					statusBar.Items[0].Text = "Изчистване на селекцията.";
				}

				// Когато е натиснат примитив полетата за вход се сетват с данните на примитива
				if (dialogProcessor.Selection.Count == 1)
				{
					transparencyLevel.Value = dialogProcessor.Selection[0].TransparencyLevel;
					angle.Value = dialogProcessor.Selection[0].Rotation;
					borderWidth.Value = dialogProcessor.Selection[0].BorderWidth;
					sizePercentage.Value = (decimal)(dialogProcessor.Selection[0].SizePercentage * 100);
				}

				dialogProcessor.IsDragging = true;
				dialogProcessor.LastLocation = e.Location;
				statusBar.Items[0].Text = "Влачене на примитив.";
				viewPort.Invalidate();
			}
		}
		void KeyDownOnWorkspace(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				dialogProcessor.Delete();
				statusBar.Items[0].Text = "Изтриване на примитив/и.";
				viewPort.Invalidate();
			}

			if (e.KeyCode == Keys.D)
			{
				PointF offset = new PointF(dialogProcessor.LastLocation.X + 10, dialogProcessor.LastLocation.Y);
				dialogProcessor.TranslateTo(offset);
				statusBar.Items[0].Text = "Преместване надясно.";
				viewPort.Invalidate();
			}

			if (e.KeyCode == Keys.A)
			{
				PointF offset = new PointF(dialogProcessor.LastLocation.X - 10, dialogProcessor.LastLocation.Y);
				dialogProcessor.TranslateTo(offset);
				statusBar.Items[0].Text = "Преместване наляво";
				viewPort.Invalidate();
			}

			if (e.KeyCode == Keys.W)
			{
				PointF offset = new PointF(dialogProcessor.LastLocation.X, dialogProcessor.LastLocation.Y - 10);
				dialogProcessor.TranslateTo(offset);
				statusBar.Items[0].Text = "Преместване нагоре";
				viewPort.Invalidate();
			}

			if (e.KeyCode == Keys.S)
			{
				PointF offset = new PointF(dialogProcessor.LastLocation.X, dialogProcessor.LastLocation.Y + 10);
				dialogProcessor.TranslateTo(offset);
				statusBar.Items[0].Text = "Преместване надолу.";
				viewPort.Invalidate();
			}

			if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
			{
				if (dialogProcessor.Selection.Count == 1)
				{
					dialogProcessor.Buffer = dialogProcessor.Selection[0];
					statusBar.Items[0].Text = "Копиран примитив.";
				}
			}

			if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
			{
				if (dialogProcessor.Buffer != null)
				{
					dialogProcessor.AddShape(dialogProcessor.Buffer);
					statusBar.Items[0].Text = "Поставен примитив.";
					viewPort.Invalidate();
				}
			}

			if (e.KeyCode == Keys.G && e.Modifiers == Keys.Control)
			{
				if (dialogProcessor.Selection.Count >= 2)
				{
					dialogProcessor.GroupSelected();
					statusBar.Items[0].Text = "Групиране на примитиви.";
					viewPort.Invalidate();
				}
			}

			if (e.Control == true && e.KeyCode == Keys.G && e.Shift == true)
			{
				if (dialogProcessor.Selection.Count != 0)
				{
					dialogProcessor.UngroupSelected();
					statusBar.Items[0].Text = "Разгрупиране на примитиви.";
					viewPort.Invalidate();
				}
			}

			if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
			{
				dialogProcessor.SelectAll();
				statusBar.Items[0].Text = "Селектиране на всички примитиви.";
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging && dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

		/// <summary>
		/// Променя цвета на bordera.
		/// </summary>
		private void borderColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.SetBorderColor(colorDialog.Color);
				statusBar.Items[0].Text = "Последно действие: Избор на цвят на border.";
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Променя цвета на filla.
		/// </summary>
		private void fillColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.SetFillColor(colorDialog.Color);
				statusBar.Items[0].Text = "Последно действие: Избор на цвят на fill.";
				viewPort.Invalidate();
			}
		}

		private void transparencyLevel_ValueChanged(object sender, EventArgs e)
		{
			dialogProcessor.SetTransparencyLevel((int)transparencyLevel.Value);
			statusBar.Items[0].Text = "Последно действие: Избор на transparency.";
			viewPort.Invalidate();
		}

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			dialogProcessor.RotateAt((int)angle.Value);
			statusBar.Items[0].Text = "Последно действие: Избор на angle.";
			viewPort.Invalidate();
		}

		private void sizeChange_ValueChanged(object sender, EventArgs e)
		{
			int input = (int)sizePercentage.Value;
			dialogProcessor.Resize(input);
			statusBar.Items[0].Text = "Последно действие: Избор на мащабриране.";
			viewPort.Invalidate();
		}

		private void borderWidth_ValueChanged(object sender, EventArgs e)
		{
			dialogProcessor.SetBorderWidth((int)borderWidth.Value);
			statusBar.Items[0].Text = "Последно действие: Избор на големина на border.";
			viewPort.Invalidate();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			dialogProcessor.GroupSelected();
			viewPort.Invalidate();
		}

		private void toolStripButton1_Click_1(object sender, EventArgs e)
		{
			dialogProcessor.UngroupSelected();
			statusBar.Items[0].Text = "Групиране на примитиви.";
			viewPort.Invalidate();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.Delete();
			statusBar.Items[0].Text = "Изтриване на примитиви.";
			viewPort.Invalidate();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.SelectAll();
			statusBar.Items[0].Text = "Селектиране на всички примитиви.";
			viewPort.Invalidate();
		}

		private void toolStripButton1_Click_2(object sender, EventArgs e)
		{
			dialogProcessor.InvertedSelect();
			statusBar.Items[0].Text = "Обърната селекция.";
			viewPort.Invalidate();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.Save(saveFileDialog.FileName);
			}
		}

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.Open(openFileDialog.FileName);
				viewPort.Invalidate();
			}
		}

        private void AddIzpitShape_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomIzpitShape();
			statusBar.Items[0].Text = "Последно действие: Рисуване на izpit shape";
			viewPort.Invalidate();
		}
    }
}

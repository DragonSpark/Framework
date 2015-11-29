// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactions.Extensions
{
	/// <summary>
	/// Provides a trigger based on the user making a gesture with their mouse on the element
	/// which this is attached to. An example of this is when the user draws a chevron, then this can trigger.
	/// </summary>
	public class MouseGestureTrigger : TriggerBase<UIElement>
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty GesturePointListProperty = DependencyProperty.Register("GesturePointListProperty", typeof(string), typeof(MouseGestureTrigger), new PropertyMetadata(default(string), new PropertyChangedCallback(OnGesturePointListChanged)));

		private bool dragging;
		private List<Point> movements;
		private List<Point> gesture;


		/// <summary>
		/// Constructor
		/// </summary>
		public MouseGestureTrigger() {
			this.ErrorThreshold = 20;
		}

		/// <summary>
		/// Defines how tolerant this is to how close the users gesture is to this gesture.
		/// A lower number indicates lower tolerance.
		/// </summary>
		public double ErrorThreshold
		{
			get;
			set;
		}

		/// <summary>
		/// List of points which define the gesture which this is triggering off of.
		/// </summary>
		public string GesturePointList
		{
			get { return (string)this.GetValue(GesturePointListProperty);}
			set { this.SetValue(GesturePointListProperty, value);}
		}

		private static void OnGesturePointListChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			MouseGestureTrigger mouseGestureTrigger = (MouseGestureTrigger)obj;

			mouseGestureTrigger.gesture = new List<Point>();

			if (mouseGestureTrigger.GesturePointList != null) {
				string[] points = mouseGestureTrigger.GesturePointList.Split(';');
				foreach (string point in points) {
					string[] xy = point.Split(',');

					try {
						mouseGestureTrigger.gesture.Add(new Point(double.Parse(xy[0]), double.Parse(xy[1])));
					}
					catch {
					}
				}
				mouseGestureTrigger.NormalizePointList(mouseGestureTrigger.gesture);
			}
		}

		private FrameworkElement AssociatedElement
		{
			get { return (FrameworkElement)this.AssociatedObject; }
		}

		/// <summary>
		/// Attach the appropriate events.
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedElement.MouseLeftButtonDown += this.AssociatedElement_MouseLeftButtonDown;
		}

		/// <summary>
		/// Detach from the appropriate events.
		/// </summary>
		protected override void OnDetaching() {
			base.OnDetaching();
			this.AssociatedElement.MouseLeftButtonDown -= this.AssociatedElement_MouseLeftButtonDown;
			if (this.dragging) {
				this.StopDrag();
			}
		}

		void AssociatedElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.movements = new List<Point>();
			this.AddPointToMovements((e.GetPosition(this.AssociatedElement)));
			this.StartDrag();
		}

		void AssociatedElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.StopDrag();
		}

		void AssociatedElement_MouseMove(object sender, MouseEventArgs e)
		{
			this.AddPointToMovements(e.GetPosition(this.AssociatedElement));
		}

		void AssociatedElement_LostMouseCapture(object sender, EventArgs e) {
			this.dragging = false;
			this.AssociatedElement.MouseMove -= this.AssociatedElement_MouseMove;
			this.AssociatedElement.MouseLeftButtonUp -= this.AssociatedElement_MouseLeftButtonUp;
			this.AssociatedElement.LostMouseCapture -= this.AssociatedElement_LostMouseCapture;

			if (this.CheckForGestureMatch()) {
				this.InvokeActions(null);
			}
		}

		private void NormalizePointList(List<Point> points)
		{
			Point topLeft = new Point(points.Min(x => x.X), points.Min(y => y.Y));
			Point bottomRight = new Point(points.Max(x => x.X), points.Max(y => y.Y));
			Size size = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

			for(int i = 0; i < points.Count; i++)
			{
				Point point = points[i];
				point.X -= topLeft.X;
				point.Y -= topLeft.Y;
				point.X /= size.Width;
				point.Y /= size.Height;
				points[i] = point;
			}
		}

		private void AddPointToMovements(Point point)
		{
			// normalize point
			this.movements.Add(point);
		}

		private void StartDrag()
		{
			if (this.AssociatedElement.CaptureMouse()) {
				this.dragging = true;
				this.AssociatedElement.MouseMove += this.AssociatedElement_MouseMove;
				this.AssociatedElement.MouseLeftButtonUp += this.AssociatedElement_MouseLeftButtonUp;
				this.AssociatedElement.LostMouseCapture += this.AssociatedElement_LostMouseCapture;
			}
		}

		private void StopDrag()
		{
			this.AssociatedElement.ReleaseMouseCapture();
		}

		private double GetDistance(Point p1, Point p2)
		{
			double x = (p1.X - p2.X);
			double y = (p1.Y - p2.Y);

			return Math.Sqrt(x*x+y*y);
		}

		private Point GetParametricValue(List<Point> points, double t)
		{
			double[] lengths = new double[points.Count];
			double totalLength = 0;

			lengths[0] = 0;
			for (int i = 1; i < points.Count; i++)
			{
				lengths[i] = lengths[i - 1] + this.GetDistance(points[i], points[i - 1]);
			}
			totalLength = lengths[points.Count - 1];

			for (int i = 0; i < points.Count; i++)
			{
				lengths[i] /= totalLength;
			}

			for (int i = 0; i < points.Count - 1; i++)
			{
				if (t > lengths[i] && t <= lengths[i + 1])
				{
					double r = t - lengths[i];
					double x = (1 - r) * points[i].X + r * points[i + 1].X;
					double y = (1 - r) * points[i].Y + r * points[i + 1].X;
					return new Point(x, y);
				}
			}

			return new Point();
		}

		private bool CheckForGestureMatch()
		{
			this.NormalizePointList(this.movements);

			double totalError = 0;

			for (double t = 0; t < 1.0; t += 0.01)
			{
				Point gesturePoint = this.GetParametricValue(this.gesture, t);
				Point movementPoint = this.GetParametricValue(this.movements, t);
				totalError += this.GetDistance(gesturePoint, movementPoint);

				if (totalError > this.ErrorThreshold)
				{
					return false;
				}
			}

			return true;
		}

		
	}
}

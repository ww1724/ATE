using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Zoranof.Workflow.Common;

namespace Zoranof.Workflow
{
    public class OptionLink
    {
        // func
        public WorkflowEditor Attacher { get; set; }

        public WorkflowOption From { get; set; }

        public WorkflowOption To { get; set; }

        // draw
        public Point StartPoint { get => From.PointToViewer; }

        public Point EndPoint { get => To.PointToViewer; }

        PointCollection Points { get; set; }

        public Polyline Polyline { get; set; }

        public Polygon Arrow { get; set; }

        public bool IsDragging { get; set; }

        public bool IsHovered { get; set; }

        public bool IsSelected { get; set; }

        public OptionLink()
        {
            LinkUpdated += (o, e) =>
            {

            };
        }

        #region public slots
        public Point PointExtend(Point point, OptionLocation location, double distance) {
            Point ret;
            switch(location)
            {
                case OptionLocation.Left: ret = new Point(point.X - distance, point.Y);break;
                case OptionLocation.Right: ret = new Point(point.X + distance, point.Y); break;
                case OptionLocation.Top: ret = new Point(point.X, point.Y - distance); break;
                case OptionLocation.Bottom: ret = new Point(point.X, point.Y + distance); break;
                default: ret = point; break;
            }
            return ret;
        }

        public Polyline GenerateLinkLines()
        {
            Polyline a = new Polyline();
            List<Point> points1 = new() { StartPoint };
            List<Point> points2 = new() { EndPoint };
            
            points1.Add(PointExtend(StartPoint, From.Location, Attacher.ConnectLineDistance));
            points2.Add(PointExtend(EndPoint, To.Location, Attacher.ConnectLineDistance));

            Vector vector = new(points1.Last().X - points2.Last().X, points1.Last().Y - points2.Last().Y);
            
            
            
            return a;
        }
        #endregion

        #region Events
        public event EventHandler LinkUpdated;

        public virtual void OnLinkUpdated(EventArgs e) => LinkUpdated?.Invoke(this, e);
        #endregion



    }
}

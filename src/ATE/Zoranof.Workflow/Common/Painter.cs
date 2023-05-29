using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Zoranof.Workflow.Common
{
    public class Painter
    {
        private Brush m_brush { get; set; }

        private Pen m_pen { get; set; } 

        private double m_thickness { get; set; }

        private DrawingContext m_painter;

        Painter() {
            m_thickness = 2;
            m_pen = new Pen(Brushes.Black, m_thickness);
            m_brush = Brushes.White;
        }

        public void ReInitPainter(DrawingContext painter)
        {
            m_painter = painter;
        }

        public void SetBrush(Brush brush) => m_brush = brush;

        public void SetPen(Pen pen) => m_pen = pen;

        public void SetPainter(DrawingContext painter) => m_painter = painter;

        public void SetThickness(double thickness) => m_thickness = thickness;
    }
}

using ATE.Services.Entities;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ATE.Test
{
    public class TestingCodeEditor : Control
    {

        public TestingCodeEditor()
        {
            Background = Brushes.LightCyan;
        }



        #region public slots

        #endregion

        #region private functions


        #endregion

        #region private fields
        public List<string> m_items = new List<string>();
        #endregion

        #region override
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            OnDrawFramework(drawingContext);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            var a = e.Data as TestingProjectEntity;
            m_items.Add(a.Title);
            InvalidateVisual();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            
            e.Effects = DragDropEffects.Copy;
            base.OnDragEnter(e);
        }
        #endregion

        #region draw
        public virtual void OnDrawFramework(DrawingContext drawingContext)
        {
            drawingContext.DrawRoundedRectangle(Background, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height), 8, 8);
            foreach(var item in m_items)
            {
                drawingContext.PushTransform(new TranslateTransform(100, 100));
                FormattedText ft = new FormattedText(
                    item,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("YouYuan"),
                    90.0,
                    Brushes.Red,
                    90
                    );
                drawingContext.DrawText(ft, new Point(10, 10));
            }
        }

        public virtual void OnDrawHeader(DrawingContext drawingContext)
        {

        }

        public virtual void OnDrawInfo(DrawingContext drawingContext) { }
        #endregion

        #region custom events

        #endregion
    }
}

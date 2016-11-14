using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace IEDExplorer
{
    public class MyTabControl : TabControl
    {
        public MyTabControl()
        {            
            // Enable default double buffering processing (DoubleBuffered returns true)
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            // Disable default CommCtrl painting on non-Vista systems
            if (Environment.OSVersion.Version.Major < 6)
                SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);            
            DrawControl(e.Graphics);
        }

        internal void DrawTab(Graphics g, TabPage tabPage, int nIndex)
        {
          Rectangle recBounds = this.GetTabRect(nIndex);
          RectangleF tabTextArea = (RectangleF)this.GetTabRect(nIndex);

          bool bSelected = (this.SelectedIndex == nIndex);       

          Point[] pt = new Point[7];
          if (this.Alignment == TabAlignment.Top)
          {
            pt[0] = new Point(recBounds.Left, recBounds.Bottom);
            pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
            pt[2] = new Point(recBounds.Left + 3, recBounds.Top);
            pt[3] = new Point(recBounds.Right - 3, recBounds.Top);
            pt[4] = new Point(recBounds.Right, recBounds.Top + 3);
            pt[5] = new Point(recBounds.Right, recBounds.Bottom);
            pt[6] = new Point(recBounds.Left, recBounds.Bottom);
          }
          else
          {
            pt[0] = new Point(recBounds.Left, recBounds.Top);
            pt[1] = new Point(recBounds.Right, recBounds.Top);
            pt[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
            pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
            pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
            pt[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
            pt[6] = new Point(recBounds.Left, recBounds.Top);
          }

          //----------------------------
          // fill this tab with background color
          Brush br = new SolidBrush(tabPage.BackColor);
          LinearGradientBrush transparentGradientBrush = new LinearGradientBrush(recBounds, SystemColors.Control, SystemColors.ButtonFace, LinearGradientMode.Vertical);
//          LinearGradientBrush transparentGradientBrush = new LinearGradientBrush(recBounds, SystemColors.ButtonHighlight, Color.DarkOrange, LinearGradientMode.Vertical);

          g.FillPolygon(transparentGradientBrush, pt);
          br.Dispose();
          //----------------------------

          //----------------------------
          // draw border
          //g.DrawRectangle(SystemPens.ControlDark, recBounds);
          g.DrawPolygon(SystemPens.ControlDark, pt);

          if (bSelected)
          {
            // clear bottom lines
            Pen pen = new Pen(Color.Red);

            switch (this.Alignment)
            {
              case TabAlignment.Bottom:
                g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom, 
                                recBounds.Right - 1, recBounds.Bottom);
                g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom+1, 
                                recBounds.Right - 1, recBounds.Bottom+1);
                break;

              case TabAlignment.Top:
                g.DrawLine(pen, recBounds.Left + 1, recBounds.Top, 
                                   recBounds.Right - 1, recBounds.Top);
                g.DrawLine(pen, recBounds.Left + 1, recBounds.Top-1, 
                                   recBounds.Right - 1, recBounds.Top-1);
                g.DrawLine(pen, recBounds.Left + 1, recBounds.Top-2, 
                                   recBounds.Right - 1, recBounds.Top-2);
                break;
            }
            pen.Dispose();
          }

          // draw tab's icon
          if ((tabPage.ImageIndex >= 0) && (ImageList != null) && 
                     (ImageList.Images[tabPage.ImageIndex] != null))
          {
            int nLeftMargin = 8;
            int nRightMargin = 2;

            Image img = ImageList.Images[tabPage.ImageIndex];
    
            Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, 
                                recBounds.Y + 1, img.Width, img.Height);
    
            // adjust rectangles
            float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

            rimage.Y += (recBounds.Height - img.Height) / 2;
            tabTextArea.X += nAdj;
            tabTextArea.Width -= nAdj;

            // draw icon
            g.DrawImage(img, rimage);
          }          
          
          // draw string
          StringFormat stringFormat = new StringFormat();
          stringFormat.Alignment = StringAlignment.Center;  
          stringFormat.LineAlignment = StringAlignment.Center;

          br = new SolidBrush(tabPage.ForeColor);

          g.DrawString(tabPage.Text, Font, br, tabTextArea, 
                                               stringFormat);          
        }

        internal void DrawControl(Graphics g)
        {
            if (!Visible)
                return;

            Rectangle TabControlArea = this.ClientRectangle;
            Rectangle TabArea = this.DisplayRectangle;

            // fill client area
            Brush br = new SolidBrush(SystemColors.Control);
            g.FillRectangle(br, TabControlArea);
            br.Dispose();
            
            // draw border
            int nDelta = SystemInformation.Border3DSize.Width;

            Pen border = new Pen(SystemColors.ControlDark);
            TabArea.Inflate(nDelta, nDelta);
            g.DrawRectangle(border, TabArea);
            border.Dispose();
           
            // clip region for drawing tabs
            Region rsaved = g.Clip;
            Rectangle rreg;
            int nMargin = 2;

            int nWidth = TabArea.Width + nMargin;

            rreg = new Rectangle(TabArea.Left, TabControlArea.Top,
                            nWidth - nMargin, TabControlArea.Height);

            g.SetClip(rreg);

            // draw tabs
            for (int i = 0; i < this.TabCount; i++)
                DrawTab(g, this.TabPages[i], i);

            g.Clip = rsaved;            
            
            // draw background to cover flat border areas
            if (this.SelectedTab != null)
            {
                TabPage tabPage = this.SelectedTab;
                Color color = tabPage.BackColor;
                border = new Pen(color);

                TabArea.Offset(1, 1);
                TabArea.Width -= 2;
                TabArea.Height -= 2;

                g.DrawRectangle(border, TabArea);
                TabArea.Width -= 1;
                TabArea.Height -= 1;
                g.DrawRectangle(border, TabArea);

                border.Dispose();
            }            
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer.Views
{
    public class MyDataGridView : DataGridView
    {
        public MyDataGridView()
        {
            // Enable internal ListView double-buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
    }

}

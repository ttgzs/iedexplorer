using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEDExplorer.Dialogs
{
    public partial class ConnParamDialog : Form
    {
        IsoConnectionParameters par;

        public ConnParamDialog(IsoConnectionParameters param)
        {
            InitializeComponent();

            par = param;

        }

    }
}

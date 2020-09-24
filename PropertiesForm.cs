using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLtoXLSXcvt
{
    public partial class PropertiesForm : Form
    {
        private ConverterProperties Properties;

        public PropertiesForm(ConverterProperties properties)
        {
            InitializeComponent();
            Properties = properties;

            CRRNumeric.Value = (decimal)Properties.CRRValue;
            ICWRNumeric.Value = (decimal)Properties.ICWRValue;
        }

        private void CRRNumeric_ValueChanged(object sender, EventArgs e)
        {
            Properties.CRRValue = (float)CRRNumeric.Value;
        }
        private void ICWRNumeric_ValueChanged(object sender, EventArgs e)
        {
            Properties.CRRValue = (float)ICWRNumeric.Value;
        }
    }
}

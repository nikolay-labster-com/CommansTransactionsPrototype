using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandTransactionsPrototype.PropEditors
{
	public partial class ucMissionPropEditor : UserControl
	{
		public ucMissionPropEditor()
		{
			InitializeComponent();
		}

		public Mission Mission { get; set; }
	}
}

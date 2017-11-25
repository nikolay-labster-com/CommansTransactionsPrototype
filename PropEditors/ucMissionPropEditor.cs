using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Commands;

namespace CommandTransactionsPrototype.PropEditors
{
	public partial class ucMissionPropEditor : UserControl
	{
		private Mission _currentMission;
		private EditFieldCommand _editCommand;

		public ucMissionPropEditor()
		{
			InitializeComponent();
		}

		public Mission Mission {
			get { return _currentMission; }
			set
			{
				var oldMission = _currentMission;
				_currentMission = value;
				if (_currentMission != oldMission)
				{
					MissionChanged();
				}
			}
		}

		private void MissionChanged()
		{
			txtId.Enabled = false;
			txtName.Enabled = false;
			txtDescription.Enabled = false;

			if (_currentMission != null)
			{
				txtId.Text = _currentMission.Id.ToString();
				txtName.Text = _currentMission.Name;
				txtDescription.Text = _currentMission.Description;

				txtId.Enabled = true;
				txtName.Enabled = true;
				txtDescription.Enabled = true;
			}
		}

		public void InitCommands(UiCommandControl commandControl)
		{
			commandControl.AssociateCommand<CommandDeleteMission>(
				() => new object[] {_currentMission}, btnDeleteThisMission);

			commandControl.AssociateCommand<EditFieldCommand>(
				() => new object[]
				{
					_currentMission, ReflectionHelper.GetPropertyInfo<Mission>(m => m.Id), txtId.Text
				}, txtId);

			commandControl.AssociateCommand<EditFieldCommand>(
				() => new object[]
				{
					_currentMission, ReflectionHelper.GetPropertyInfo<Mission>(m => m.Name), txtName.Text
				}, txtName);

			commandControl.AssociateCommand<EditFieldCommand>(
				() => new object[]
				{
					_currentMission, ReflectionHelper.GetPropertyInfo<Mission>(m => m.Description), txtDescription.Text
				}, txtDescription);

			//_editCommand = commandControl.GetRegisteredCommand<EditFieldCommand>();
		}
	}
}

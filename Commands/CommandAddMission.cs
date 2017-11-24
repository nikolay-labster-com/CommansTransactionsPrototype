using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandAddMission : ICommand
	{
		private readonly TreeView _trv;

		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandAddMission(TreeView trv, Engine engine, ITransactionProvider transactionProvider)
		{
			_trv = trv;
			_engine = engine;
			_transactionProvider = transactionProvider;
		}

		public bool CanBeExecutedNow
		{
			get
			{
				var curNode = _trv.SelectedNode;
				if (curNode != null)
				{
					var engine = curNode.Tag as Engine;
					return engine != null;
				}
				return false;
			}
		}


		public void InitiateExecution()
		{
			if (!CanBeExecutedNow)
			{
				throw new InvalidOperationException("Cannot be executed!");
			}

			var tran = _transactionProvider.GetCurrentTransaction();

			tran.CaptureMacroState("CommandAddMission");

			string nameForMission = MyInputBox.GetText("Enter mission name", "A mission");
			var mission = new Mission();

			//var insert = new McrInsertElementToCollection(_engine.Missions, mission);
			tran.Helper.AddItem(_engine.Missions, mission);
			tran.Helper.SetValue(mission, m => m.Name, nameForMission);

			tran.Commit();
		}
	}
}

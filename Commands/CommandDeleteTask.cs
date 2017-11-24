using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandDeleteTask : ICommand
	{
		private readonly TreeView _trv;

		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandDeleteTask(TreeView trv, Engine engine, ITransactionProvider transactionProvider)
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
					var task = curNode.Tag as MissionTask;
					return task != null;
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

			tran.CaptureMacroState("CommandDeleteMission");


			var taskToDelete = (MissionTask) _trv.SelectedNode.Tag;
			var parentMission = (Mission) _trv.SelectedNode.Parent.Tag;

			tran.Helper.DeleteItem(parentMission.Tasks, taskToDelete);

			tran.Commit();
		}
	}
}

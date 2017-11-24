using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandAddTask : ICommand
	{
		private readonly TreeView _trv;

		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandAddTask(TreeView trv, Engine engine, ITransactionProvider transactionProvider)
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
					var mission = curNode.Tag as Mission;
					return mission != null;
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

			tran.CaptureMacroState("CommandAddTask");

			var mission = (Mission) _trv.SelectedNode.Tag;

			var task = new MissionTask();

			tran.Helper.AddItem(mission.Tasks, task);

			tran.Commit();
		}
	}
}

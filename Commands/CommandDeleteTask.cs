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
		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandDeleteTask(Engine engine, ITransactionProvider transactionProvider)
		{
			_engine = engine;
			_transactionProvider = transactionProvider;
		}

		public bool CanBeExecutedOn(params object[] obj)
		{
			var task = obj[0] as MissionTask;
			return task != null;
		}


		public void InitiateExecution(params object[] obj)
		{
			if (!CanBeExecutedOn(obj))
			{
				throw new InvalidOperationException("Cannot be executed!");
			}

			var tran = _transactionProvider.GetCurrentTransaction();

			tran.CaptureMacroState("CommandDeleteMission");


			var taskToDelete = (MissionTask)obj[0];
			var parentMission = (Mission)obj[1];

			tran.Helper.DeleteItem(parentMission.Tasks, taskToDelete);

			tran.Commit();
		}
	}
}

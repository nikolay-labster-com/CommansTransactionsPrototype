using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandDeleteMission : ICommand
	{
		//private readonly Func<TreeNode> _getCurrentNodeFunc;
		//private readonly TreeView _trv;

		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandDeleteMission(TreeView trv, Engine engine, ITransactionProvider transactionProvider)
		{
			//_getCurrentNodeFunc = getCurrentNodeFunc;
			//_trv = trv;
			_engine = engine;
			_transactionProvider = transactionProvider;
		}

		//public bool CanBeExecutedNow
		//{
		//	get
		//	{
		//		var curNode = _trv.SelectedNode;
		//		if (curNode != null)
		//		{
		//			var mission = curNode.Tag as Mission;
		//			return mission != null;
		//		}
		//		return false;
		//	}
		//}
		public bool CanBeExecutedOn(params object[] obj)
		{
			var mission = obj[0] as Mission;
			return mission != null;
		}


		public void InitiateExecution(params object[] obj)
		{
			if (!CanBeExecutedOn(obj))
			{
				throw new InvalidOperationException("Cannot be executed!");
			}

			var tran = _transactionProvider.GetCurrentTransaction();

			tran.CaptureMacroState("CommandDeleteMission");

			var missionToDelete = (Mission) obj[0];

			tran.Helper.DeleteItem(_engine.Missions, missionToDelete);

			tran.Commit();
		}
	}
}

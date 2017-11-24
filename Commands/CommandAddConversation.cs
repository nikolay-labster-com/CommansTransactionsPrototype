using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandAddConversation : ICommand
	{
		private readonly TreeView _trv;

		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandAddConversation(TreeView trv, Engine engine, ITransactionProvider transactionProvider)
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

		public bool CanBeExecutedOn(object obj)
		{
			var engine = obj as Engine;
			return engine != null;
		}


		public void InitiateExecution()
		{
			if (!CanBeExecutedNow)
			{
				throw new InvalidOperationException("Cannot be executed!");
			}

			var tran = _transactionProvider.GetCurrentTransaction();

			tran.CaptureMacroState("CommandAddConversation");

			var conversation = new Conversation();

			tran.Helper.AddItem(_engine.Conversations, conversation);

			tran.Commit();
		}
	}
}

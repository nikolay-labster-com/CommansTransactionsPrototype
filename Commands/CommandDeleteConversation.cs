using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandDeleteConversation : ICommand
	{
		private readonly TreeView _trv;

		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandDeleteConversation(TreeView trv, Engine engine, ITransactionProvider transactionProvider)
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
					var conversation = curNode.Tag as Conversation;
					return conversation != null;
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

			tran.CaptureMacroState("CommandDeleteConversation");

			var conversationToDelete = (Conversation)_trv.SelectedNode.Tag;

			tran.Helper.DeleteItem(_engine.Conversations, conversationToDelete);

			tran.Commit();
		}
	}
}

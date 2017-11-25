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
		//private readonly TreeView _trv;

		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandDeleteConversation(Engine engine, ITransactionProvider transactionProvider)
		{
			//_trv = trv;
			_engine = engine;
			_transactionProvider = transactionProvider;
		}

		public bool CanBeExecutedOn(params object[] obj)
		{
			var conversation = obj[0] as Conversation;
			return conversation != null;
		}


		public void InitiateExecution(params object[] obj)
		{
			if (!CanBeExecutedOn(obj))
			{
				throw new InvalidOperationException("Cannot be executed!");
			}

			var tran = _transactionProvider.GetCurrentTransaction();

			tran.CaptureMacroState("CommandDeleteConversation");

			var conversationToDelete = (Conversation)obj[0];

			tran.Helper.DeleteItem(_engine.Conversations, conversationToDelete);

			tran.Commit();
		}
	}
}

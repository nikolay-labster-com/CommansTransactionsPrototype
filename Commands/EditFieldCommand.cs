using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class EditFieldCommand: ICommand
	{
		private readonly ITransactionProvider _transactionProvider;

		public EditFieldCommand(ITransactionProvider transactionProvider)
		{
			_transactionProvider = transactionProvider;
		}
		public bool CanBeExecutedOn(params object[] obj)
		{
			if (obj[0] == null)
				return false;
			var propInfo = (PropertyInfo)obj[1];
			if (propInfo.Name == "Id")
				return false;
			return true;
		}

		public void InitiateExecution(params object[] obj)
		{
			if (!CanBeExecutedOn(obj))
			{
				throw new InvalidOperationException("Cannot be executed!");
			}

			var tran = _transactionProvider.GetCurrentTransaction();

			var propInfo = (PropertyInfo) obj[1];
			tran.CaptureMacroState(string.Format("Edit [{0}]", propInfo.Name));

			tran.Helper.SetValue(obj[0], propInfo, obj[2]);

			tran.Commit();

		}
	}
}

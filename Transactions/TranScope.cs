using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTransactionsPrototype.Transactions
{
	public interface ITransitionProvider
	{
		SwapTransitionResult GetCurrentTransitionAndCreateNewOne();
	}

	public class TranScope : IDisposable
	{
		//private readonly StateHistoryTree _entireHistory;
		private readonly List<ITransition> _transitionHistory = new List<ITransition>();
		private readonly ITransitionProvider _transitionInfoProvider;
		private bool _commited;
		private bool _rolledBack;
		public event EventHandler<TransactionCompletedEventArgs> TransactionCompleted;

		public TranScope(ITransitionProvider transitionInfoProvider)
		{
			//_entireHistory = entireHistory;
			_transitionInfoProvider = transitionInfoProvider;
		}

		public State CaptureMacroState(string label = null)
		{
			return InternalCaptureState(new State(label, true));
		}

		public State CaptureState(string label = null)
		{
			return InternalCaptureState(new State(label, false));
		}

		private State InternalCaptureState(State state)
		{
			var swapResult = _transitionInfoProvider.GetCurrentTransitionAndCreateNewOne();

			var prevTransition = swapResult.OldTransition;
			var currentTransition = swapResult.NewTransition;

			currentTransition.State1 = state;

			if (prevTransition != null)
			{
				prevTransition.State2 = state;
				prevTransition.Complete();
				_transitionHistory.Add(prevTransition);
			}

			Debug.WriteLine(string.Format("Captured [{0}]", state.Label));
			return state;
		}

		public void Commit()
		{
			EnsureNotCommitedOrRolledBack();
			OnTransationCompleted();
			_commited = true;
		}

		private void OnTransationCompleted()
		{
			var handler = TransactionCompleted;
			if (handler == null)
				throw new InvalidOperationException("Transaction cannot be completed without an owner which is listening for its completion!");
			CaptureState("Transaction completed");
			handler(this, new TransactionCompletedEventArgs(_transitionHistory));
		}

		public void Rollback()
		{
			EnsureNotCommitedOrRolledBack();
			_rolledBack = true;
		}

		private void EnsureNotCommitedOrRolledBack()
		{
			if (_commited || _rolledBack)
			{
				throw new InvalidOperationException(
					"This operation is not allowed when the transaction is commited or rolled back.");
			}
		}

		public void Dispose()
		{
			if (!_commited && !_rolledBack)
			{
				Rollback();
			}
		}

		// this property here is just for convenient access to
		// MicroCommandTransitionProvider from the user of TranScope perspective
		// it can be avoided if we pass two parameters to our commands (tran + helper)
		// it is set once in at the stage of creation of the transactionScope and must be never changed
		public MicroCommandHelper Helper { get; set; }
	}


	public class TransactionCompletedEventArgs : EventArgs
	{
		public TransactionCompletedEventArgs(List<ITransition> transitions)
		{
			Transitions = transitions;
		}
		public List<ITransition> Transitions { get; private set; }
	}

	public static class IdGenerator
	{
		private static long _id;

		public static long GetNextId()
		{
			return _id;
		}
	}

	[System.Diagnostics.DebuggerDisplay("{DebugInfo}")]
	public class State
	{
		public DateTime DateOfState { get; set; }
		public long Id { get; set; }
		public string Label { get; set; }
		public bool IsMacroState { get; set; }

		private string DebugInfo
		{
			get
			{
				if (string.IsNullOrEmpty(Label))
				{
					return DateOfState.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
				}
				return Label;
			}
		}

		public State(string label = null, bool isMacro = false)
		{
			Id = IdGenerator.GetNextId();
			DateOfState = DateTime.Now;
			Label = label;
			IsMacroState = isMacro;
		}
	}

	public interface ITransition
	{
		State State1 { get; set; }
		State State2 { get; set; }
		void Complete();
		void ExecuteForward();
		void ExecuteBackward();
	}

	public class SwapTransitionResult
	{
		public ITransition OldTransition { get; set; }
		public ITransition NewTransition { get; set; }

		public static SwapTransitionResult Swap<T>(ref T currentTransition, T newTransition)
			where T : ITransition
		{
			var rez = new SwapTransitionResult
			{
				OldTransition = currentTransition,
				NewTransition = newTransition
			};
			currentTransition = newTransition;
			return rez;
		}
	}


}

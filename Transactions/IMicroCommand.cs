using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandTransactionsPrototype.Transactions
{
	public interface IMicroCommand
	{
		bool CanReverse { get; }
		void ExecuteForward();
		void ExecuteReverse();
	}
	public class MicroCommandHelper
	{
		private MicroCommandTransitionProvider _provider;

		public MicroCommandHelper(MicroCommandTransitionProvider provider)
		{
			_provider = provider;
		}
		public void AddItem(IList list, object item)
		{
			var command = new McrInsertElementToCollection(list, item);
			_provider.AddCommand(command);
			command.ExecuteForward();
		}

		public void DeleteItem(IList list, object item)
		{
			var command = new McrDeleteElementFromCollection(list, item);
			_provider.AddCommand(command);
			command.ExecuteForward();
		}

		public void SetValue<T>(T obj, Expression<Func<T, object>> propertyLambda, object value)
		{
			var propertyInfo = ReflectionHelper.GetPropertyInfo(propertyLambda);
			var command = new McrSetProperty(obj, propertyInfo, value);
			_provider.AddCommand(command);
			command.ExecuteForward();
		}
	}
	public class MicroCommandTransitionProvider : ITransitionProvider
	{
		private MicroCommandTransition _currentTransition;

		public void AddCommand(IMicroCommand command)
		{
			_currentTransition.AddCommand(command);
		}

		public SwapTransitionResult GetCurrentTransitionAndCreateNewOne()
		{
			return SwapTransitionResult.Swap(ref _currentTransition, new MicroCommandTransition());
		}
	}
	public class MicroCommandTransition : ITransition
	{
		private readonly List<IMicroCommand> _commands = new List<IMicroCommand>();
		public State State1 { get; set; }
		public State State2 { get; set; }

		public void AddCommand(IMicroCommand command)
		{
			_commands.Add(command);
		}

		public void Complete()
		{
			// MicroCommandTransition may optimize commands here, but it is not essential
		}


		public void ExecuteForward()
		{
			foreach (var oneCommand in _commands)
			{
				oneCommand.ExecuteForward();
			}
		}

		public void ExecuteBackward()
		{
			for (int i = _commands.Count - 1; i >= 0; i--)
			{
				var oneCommand = _commands[i];
				oneCommand.ExecuteReverse();
			}
		}
	}

	public class McrInsertElementToCollection : IMicroCommand
	{
		private readonly IList _collection;
		private readonly object _item;
		private readonly int _index;

		public McrInsertElementToCollection(IList collection, object item)
			: this(collection, item, collection.Count)
		{
		}

		public McrInsertElementToCollection(IList collection, object item, int index)
		{
			_collection = collection;
			_item = item;
			_index = index;
		}

		public bool CanReverse
		{
			get { return true; }
		}

		public void ExecuteForward()
		{
			_collection.Insert(_index, _item);
		}

		public void ExecuteReverse()
		{
			_collection.Remove(_item);
		}
	}

	public class McrDeleteElementFromCollection : IMicroCommand
	{
		private readonly IList _collection;
		private readonly object _item;
		private int _index;

		public McrDeleteElementFromCollection(IList collection, object item)
		{
			_collection = collection;
			_item = item;
		}

		public bool CanReverse
		{
			get { return true; }
		}

		public void ExecuteForward()
		{
			_index = _collection.IndexOf(_item);
			_collection.RemoveAt(_index);
		}

		public void ExecuteReverse()
		{
			_collection.Insert(_index, _item);
		}
	}

	public class McrSetProperty : IMicroCommand
	{
		private readonly object _obj;
		private readonly PropertyInfo _property;
		private object _oldValue;
		private readonly object _newValue;

		public McrSetProperty(object obj, PropertyInfo property, object value)
		{
			_obj = obj;
			_property = property;
			_newValue = value;
		}
		public bool CanReverse
		{
			get { return true; }
		}

		public void ExecuteForward()
		{
			_oldValue = _property.GetValue(_obj);
			_property.SetValue(_obj, _newValue);
		}

		public void ExecuteReverse()
		{
			_property.SetValue(_obj, _oldValue);
		}
	}
}

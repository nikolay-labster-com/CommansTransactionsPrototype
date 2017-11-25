using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Commands;

namespace CommandTransactionsPrototype
{
	public class UiCommandControl
	{
		private ItemToCollectionDictionary<ICommand, ElementParamMakerPair> _allCommands;
		//private Dictionary<ICommand, Func<object[]>> _paramMakers;
		private Action _onCommandExecuted;
		private Action<Exception> _onCommandFailed;
		private Dictionary<Type, ICommand> _registeredCommand;

		public UiCommandControl(Action onCommandExecuted, Action<Exception> onCommandFailed)
		{
			_allCommands = new ItemToCollectionDictionary<ICommand, ElementParamMakerPair>();
			//_paramMakers = new Dictionary<ICommand, Func<object[]>>();
			_registeredCommand = new Dictionary<Type, ICommand>();
			_onCommandExecuted = onCommandExecuted;
			_onCommandFailed = onCommandFailed;
		}

		public void AssociateCommand<T>(Func<object[]> paramsMaker, params object[] controls)
			where T : ICommand
		{
			AssociateCommand(GetRegisteredCommand<T>(), paramsMaker, controls);
		}

		public void AssociateCommand(
			ICommand command,
			Func<object[]> paramsMaker,
			params object[] controls)
		{
			//_paramMakers.Add(command, paramsMaker);
			RegisterCommand(command);

			foreach (var oneControl in controls)
			{
				IGenericUiElement uiElement;
				if (oneControl is TextBox)
				{
					uiElement = new TextField((TextBox) oneControl);

				}
				else if (oneControl is Control)
				{
					uiElement = new ControlItem((Control)oneControl);

				}
				else if (oneControl is ToolStripItem)
				{
					uiElement = new MenuItem((ToolStripItem) oneControl);
				}
				else
				{
					throw new InvalidOperationException("Unsupported item was passed.");
				}
				uiElement.Activated += (sender, args) =>
				{
					try
					{
						command.InitiateExecution(paramsMaker());
						if (_onCommandExecuted != null)
						{
							_onCommandExecuted();
						}
						UpdateControlsAvailability();
					}
					catch (Exception ex)
					{
						if (_onCommandFailed != null)
						{
							_onCommandFailed(ex);
						}
						else
						{
							throw;
						}
					}
				};
				_allCommands.Add(command,
					new ElementParamMakerPair
					{
						Element = uiElement,
						ParamMaker = paramsMaker
					});
			}
		}

		public void RegisterCommand(ICommand command)
		{
			var type = command.GetType();
			ICommand existingCommand;
			if (_registeredCommand.TryGetValue(type, out existingCommand))
			{
				if (existingCommand != command)
				{
					throw new InvalidOperationException("Attempt to register another command of the same type.");
				}
			}
			else
			{
				_registeredCommand.Add(type, command);
			}
			//_registeredCommand.Add(command);
		}

		public T GetRegisteredCommand<T>()
			where T : ICommand
		{
			//return _registeredCommand.OfType<T>().Single();
			return (T)_registeredCommand[typeof(T)];
		}

		private class ItemToCollectionDictionary<TKey, TValue>
		{
			private Dictionary<TKey, List<TValue>> _dict = new Dictionary<TKey, List<TValue>>();
			private Dictionary<TValue, TKey> _dict2 = new Dictionary<TValue, TKey>();

			public void Add(TKey key, params TValue[] valueArr)
			{
				List<TValue> list;
				if (!_dict.TryGetValue(key, out list))
				{
					list = new List<TValue>();
					_dict[key] = list;
				}
				list.AddRange(valueArr);
				foreach (var oneValue in valueArr)
				{
					_dict2.Add(oneValue, key);
				}
			}

			public TKey GetKeyByValue(TValue value)
			{
				return _dict2[value];
			}

			public IEnumerable<KeyValuePair<TValue, TKey>> GetAllControls()
			{
				return _dict2;
			}
		}

		public void UpdateControlsAvailability()
		{
			foreach (var oneKvp in _allCommands.GetAllControls())
			{
				var paramsMaker = oneKvp.Key.ParamMaker;
				oneKvp.Key.Element.Enabled = oneKvp.Value.CanBeExecutedOn(paramsMaker());
			}
		}

		private class ElementParamMakerPair
		{
			public IGenericUiElement Element { get; set; }
			public Func<object[]> ParamMaker { get; set; }
		}

		private interface IGenericUiElement
		{
			bool Enabled { get; set; }
			bool Visible { get; set; }
			event EventHandler Activated;
		}

		private class MenuItem : IGenericUiElement
		{
			public event EventHandler Activated;

			private readonly ToolStripItem _item;

			public MenuItem(ToolStripItem item)
			{
				_item = item;
				_item.Click += Item_Click;
			}

			private void Item_Click(object sender, EventArgs e)
			{
				var handler = Activated;
				if (handler != null)
				{
					handler(sender, e);
				}
			}
			public bool Enabled
			{
				get { return _item.Enabled; }
				set { _item.Enabled = value; }
			}

			public bool Visible
			{
				get { return _item.Visible; }
				set { _item.Visible = value; }
			}
		}
		private class ControlItem : IGenericUiElement
		{
			public event EventHandler Activated;
			private readonly Control _item;

			public ControlItem(Control item)
			{
				_item = item;
				_item.Click += Item_Click;
			}
			private void Item_Click(object sender, EventArgs e)
			{
				var handler = Activated;
				if (handler != null)
				{
					handler(sender, e);
				}
			}
			public bool Enabled
			{
				get { return _item.Enabled; }
				set { _item.Enabled = value; }
			}

			public bool Visible
			{
				get { return _item.Visible; }
				set { _item.Visible = value; }
			}
		}

		private class TextField : IGenericUiElement
		{
			public event EventHandler Activated;
			private TextBox _textBox;
			private bool _trackTextBox = false;
			private bool _textChanged = false;

			public TextField(TextBox textBox)
			{
				_textBox = textBox;
				_textBox.Leave += TextBox_Leave;
				_textBox.TextChanged += TextBox_TextChanged;
				_textBox.EnabledChanged += TextBox_EnabledChanged;
			}

			private void TextBox_EnabledChanged(object sender, EventArgs e)
			{
				_trackTextBox = _textBox.Enabled;
				_textChanged = false;
			}

			private void TextBox_Leave(object sender, EventArgs e)
			{
				if (_textChanged)
				{
					var handler = Activated;
					if (handler != null)
					{
						handler(sender, e);
					}
					_textChanged = false;
				}
			}

			private void TextBox_TextChanged(object sender, EventArgs e)
			{
				if (_trackTextBox)
				{
					_textChanged = true;
				}
			}

			public bool Enabled
			{
				get { return _textBox.Enabled; }
				set { _textBox.Enabled = value; }
			}

			public bool Visible
			{
				get { return _textBox.Visible; }
				set { _textBox.Visible = value; }
			}

		}
	}
}

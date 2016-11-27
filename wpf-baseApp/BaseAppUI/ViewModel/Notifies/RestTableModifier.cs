using BaseAppUI.ViewModel.Payments;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseAppUI.Common;
namespace BaseAppUI.ViewModel.Notifies
{
    public enum RestTableActionType
    {
        Addition,
        Subtract,
        Cancel,
        Done,
        Reset
    }
    

    public class RestTableModifier : NotifyBase
    {
        private int _quantity;
        private string _tableNumber;
        private int? _inScale;
        private AlphaNumericKeybordButton _resetTableNumber;
        private List<AlphaNumericKeybordButton> _tableNumberButtons;

        private DelegateCommand<RestTableActionType> _customItemCommand;
        private DelegateCommand<AlphaNumericKeybordButton> _selectKeybordButton;

        Action<string> _addTableNumber;
        public RestTableModifier(Action<string> addTableNumber)
        {
            _quantity = 1;
            EnableMask = true;
            _addTableNumber = addTableNumber;
        }

        //Code added for Custom Item 5/6/2016
        private string _customitemname = "Table Item";
        public string CustomItemName
        {
            get { return _customitemname; }
            set { _customitemname = value; }
        }

        //Above Code added for Custom Item 5/6/2016

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public string TableNumber
        {
            get { return _tableNumber; }
            set
            {
                
                    _tableNumber = value;
                    OnPropertyChanged("TableNumber");
                

            }
        }

        public List<AlphaNumericKeybordButton> CalculatorButtons
        {
            get
            {

                return _tableNumberButtons ?? (_tableNumberButtons = new List<AlphaNumericKeybordButton>() {
                     new AlphaNumericKeybordButton{Label="A",Value="A",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="B",Value="B",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="C",Value="C",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="1",Value="1",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="2",Value="2",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="3",Value="3",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="4",Value="4",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="5",Value="5",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="6",Value="6",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="7",Value="7",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="8",Value="8",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="9",Value="9",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label=".",Value="",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="0",Value="0",Type=AlphaNumericKeybordButtonType.AlphaNumeric},
                     new AlphaNumericKeybordButton{Label="00",Value="",Type=AlphaNumericKeybordButtonType.AlphaNumeric},

                 });

            }
        }

        public AlphaNumericKeybordButton ResetTableNumber
        {
            get { return _resetTableNumber ?? (_resetTableNumber = new AlphaNumericKeybordButton { Type = AlphaNumericKeybordButtonType.Reset }); }

        }

        public DelegateCommand<AlphaNumericKeybordButton> SelectKeybordButton
        {
            get
            {
                return _selectKeybordButton ?? (_selectKeybordButton = new DelegateCommand<AlphaNumericKeybordButton>((e) =>
                {




                    switch (e.Type)
                    {

                        case AlphaNumericKeybordButtonType.AlphaNumeric:
                            TableNumber = TableNumber+ e.Value;
                           
                            break;
                        case AlphaNumericKeybordButtonType.Reset:
                            TableNumber = "";

                            break;
                        case AlphaNumericKeybordButtonType.Double:
                            break;
                    }






                }));
            }

        }

        public DelegateCommand<RestTableActionType> CustomItemCommand
        {
            get
            {
                return _customItemCommand ?? (_customItemCommand = new DelegateCommand<RestTableActionType>((e =>
                {

                    switch (e)
                    {
                        case RestTableActionType.Addition:
                            Quantity++;
                            break;
                        case RestTableActionType.Subtract:
                            if (Quantity > 1)
                                Quantity--;
                            break;

                        case RestTableActionType.Cancel:
                            this.CloseCommand.Execute(null);
                            break;

                        case RestTableActionType.Done:



                            if (_addTableNumber != null)
                                _addTableNumber.Invoke(TableNumber);
                            this.CloseCommand.Execute(null);

                            break;
                    }

                })));
            }
        }



    }
}

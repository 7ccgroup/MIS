using BaseAppUI.ViewModel.Payments;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Notifies
{
    public enum CustomItemActionType
    {
        Addition,
        Subtract,
        Cancel,
        Done,
        Reset
    }

    public class CustomItemModifier : NotifyBase
    {
        private int _quantity;
        private decimal _price;
        private int? _inScale;
        private KeybordButton _resetPrice;
        private List<KeybordButton> _calculatorButtons;

        private DelegateCommand<CustomItemActionType> _customItemCommand;
        private DelegateCommand<KeybordButton> _selectKeybordButton;

        Action<decimal,int,string> _addCustomItem;
        public CustomItemModifier(Action<decimal,int,string> addCustomItem)
        {
            _quantity = 1;
            EnableMask = true;
            _addCustomItem = addCustomItem;
        }

        //Code added for Custom Item 5/6/2016
        private string _customitemname="Custom Item";
        public string CustomItemName
        {
            get { return _customitemname; }
            set { _customitemname = value; }
        }

        //Above Code added for Custom Item 5/6/2016

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value;
            OnPropertyChanged("Quantity");
            }
        }

        public decimal Price
        {
            get { return _price; }
            set {
                if (value < 999999m)
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
              
            }
        }

        public List<KeybordButton> CalculatorButtons
        {
            get
            {

                return _calculatorButtons ?? (_calculatorButtons = new List<KeybordButton>() { 
                 
                     new KeybordButton{Label="1",Value=1},
                     new KeybordButton{Label="2",Value=2},
                     new KeybordButton{Label="3",Value=3},
                     new KeybordButton{Label="4",Value=4},
                     new KeybordButton{Label="5",Value=5},
                     new KeybordButton{Label="6",Value=6},
                     new KeybordButton{Label="7",Value=7},
                     new KeybordButton{Label="8",Value=8},
                     new KeybordButton{Label="9",Value=9},
                     new KeybordButton{Label=".",Type=KeybordButtonType.Scale},
                     new KeybordButton{Label="0",Value=0},
                     new KeybordButton{Label="00",Type=KeybordButtonType.Double},

                 });

            }
        }

        public KeybordButton ResetPrice
        {
            get { return _resetPrice ?? (_resetPrice = new KeybordButton { Type = KeybordButtonType.Reset }); }

        }

        public DelegateCommand<KeybordButton> SelectKeybordButton
        {
            get
            {
                return _selectKeybordButton ?? (_selectKeybordButton = new DelegateCommand<KeybordButton>((e) =>
                {




                    switch (e.Type)
                    {

                        case KeybordButtonType.Reset:
                            Price = 0m;
                            _inScale = null;
                            break;
                        case KeybordButtonType.Scale:
                            _inScale = 2;
                            break;
                        case KeybordButtonType.Double:
                            Price *= 100;
                            break;
                        case KeybordButtonType.Numeric:


                            decimal val = (long)Price;
                            decimal scale = (decimal)Price - val;

                            if (!_inScale.HasValue)
                            {
                                Price = (val * 10) + e.Value + scale;
                            }
                            else
                            {

                                if (_inScale.Value == 2)
                                {
                                    scale = e.Value / 10;
                                    _inScale = 1;
                                }
                                else
                                {
                                    decimal scale1 = (long)(scale * 10);
                                    scale = (decimal)((scale1 * 10) + e.Value) / 100;
                                    _inScale = 2;

                                }

                                Price = val + scale;

                            }



                            break;

                    }



                


                }));
            }

        }

        public DelegateCommand<CustomItemActionType> CustomItemCommand
        {
            get
            {
                return _customItemCommand ?? (_customItemCommand = new DelegateCommand<CustomItemActionType>((e =>
                {

                    switch (e)
                    {
                        case CustomItemActionType.Addition:
                            Quantity++;
                            break;
                        case CustomItemActionType.Subtract:
                            if (Quantity > 1)
                                Quantity--;
                            break;

                        case CustomItemActionType.Cancel:
                            this.CloseCommand.Execute(null);
                            break;

                        case CustomItemActionType.Done:

                          

                            if (_addCustomItem != null)
                                _addCustomItem.Invoke(Price,Quantity,CustomItemName);
                            this.CloseCommand.Execute(null);

                            break;
                    }

                })));
            }
        }


      
    }
}

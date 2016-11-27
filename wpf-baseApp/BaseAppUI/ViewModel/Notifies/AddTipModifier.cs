using BaseAppUI.ViewModel.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Notifies
{
    public enum AddTipActionType
    {
        Cancel,
        Done,
        Reset
    }

    public enum TipConverterType
    {
        Regular,
        Percent

    }

    public class TipConverter : NotifyProperty
    {
        public TipConverterType Type { get; set; }
        public string Label { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

    }
    public class AddTipModifier : NotifyBase
    {
        private int? _inScale;
        private KeybordButton _resetTip;
        private DelegateCommand<AddTipActionType> _addtipCommand;
        private List<KeybordButton> _calculatorButtons;
        private List<TipConverter> _tipConverters;
        private DelegateCommand<TipConverter> _selectTipConverterCommand;
        private DelegateCommand<KeybordButton> _selectKeybordButton;

        private Action<decimal> _addtipaction;
        private decimal _subtotal;

        public AddTipModifier(string txn_id, decimal tip_amt, Action<decimal> addtipaction)
        {
           // _subtotal = subtotal;
            //_addtipaction = addtipaction;
            //_tipValue = recenttip;

            base.EnableMask = true;

            SelectTipConverterCommand.Execute(TipConverters[0]);
        }

        private decimal _tipValue;

        public decimal TipValue
        {
            get { return _tipValue; }
            set
            {

                if (value < 999999m)
                {
                    _tipValue = value;

                    //OnPropertyChanged("DiscountValue");
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

        public List<TipConverter> TipConverters
        {
            get
            {
                return _tipConverters??(_tipConverters = new List<TipConverter>() {

                   new TipConverter{Type=TipConverterType.Regular,Label="$"},
                   new TipConverter{Type=TipConverterType.Percent,Label="%"}

               });
            }
        }

        public KeybordButton ResetTip
        {
            get { return _resetTip ?? (_resetTip = new KeybordButton { Type = KeybordButtonType.Reset }); }

        }

        public DelegateCommand<AddTipActionType> TipCommand
        {
            get
            {
                return _addtipCommand ?? (_addtipCommand = new DelegateCommand<AddTipActionType>((e =>
                {

                    switch (e)
                    {
                        case AddTipActionType.Cancel:
                            this.CloseCommand.Execute(null);
                            break;

                        case AddTipActionType.Done:


                            var val = Validate();

                            if (val == -1m)
                                return;





                            if (_addtipaction != null)
                                _addtipaction.Invoke(val);
                            this.CloseCommand.Execute(null);


                            break;
                    }

                })));
            }
        }

        private decimal Validate()
        {

            switch (SelectedConverter.Type)
            {

                case TipConverterType.Regular:

                    if (_subtotal > TipValue)
                        return TipValue;
                    else return -1m;
                case TipConverterType.Percent:

                    var discount = (decimal)(_subtotal * TipValue / 100);
                    if (_subtotal > discount)
                        return discount;
                    else return -1m;

                default: return -1m;

            }

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
                            TipValue = 0m;
                            _inScale = null;
                            break;
                        case KeybordButtonType.Scale:
                            _inScale = 2;
                            break;
                        case KeybordButtonType.Double:
                            TipValue *= 100;
                            break;
                        case KeybordButtonType.Numeric:


                            decimal val = (long)TipValue;
                            decimal scale = (decimal)TipValue - val;

                            if (!_inScale.HasValue)
                            {
                                TipValue = (val * 10) + e.Value + scale;
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

                                TipValue = val + scale;

                            }



                            break;

                    }






                }));
            }

        }



        private TipConverter _selectedConverter;

        public TipConverter SelectedConverter
        {
            get { return _selectedConverter; }
            set
            {
                _selectedConverter = value;
                OnPropertyChanged("SelectedConverter");
            }
        }

        public DelegateCommand<TipConverter> SelectTipConverterCommand
        {
            get
            {
                return _selectTipConverterCommand ?? (_selectTipConverterCommand = new DelegateCommand<TipConverter>((e) => {

                    if (SelectedConverter != e)
                    {

                        if (SelectedConverter != null)
                            SelectedConverter.IsSelected = false;


                        SelectedConverter = e;
                        SelectedConverter.IsSelected = true;

                    }

                }));
            }
        }

    }



}

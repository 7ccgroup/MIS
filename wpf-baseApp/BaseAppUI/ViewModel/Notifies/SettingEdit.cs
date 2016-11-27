using BaseAppUI.Common;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Notifies
{
    public enum SettingEditButton
    {
        Addition,
        Subtract,
        Delete,
        Done,
        Cancel,
        Save
    }
    public class SettingEdit : NotifyBase //This class is used to edit line settingSAA.
    {
        SettingScreenEdit _lineitem;
        public SettingEdit(SettingScreenEdit lineitem)
        {
            _lineitem = lineitem;
            SelectedEditItem = lineitem;
            IsPopup = true;
            EnableMask = true;

            // _quantity = _lineitem.;
        }

        //public string ItemDescription
        //{
        //    get { return _lineitem.vItemDesc1; }
        //}


        //below code is to modify quantity of lineItem.
        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("Subtotal");
            }
        }

        //public decimal Price
        //{
        //    get { return convert.decimal(_lineitem.vItemPrice); }
        //}

        //public decimal Subtotal
        //{
        //    get { return (decimal)Price * Quantity; }
        //}

        private SettingScreenEdit _SelectedEditItem;
        public SettingScreenEdit SelectedEditItem
        {
            get { return _SelectedEditItem; }
            set { _SelectedEditItem = _lineitem; }
        }

        private DelegateCommand<SettingEditButton> _settingEditCommand;

        public DelegateCommand<SettingEditButton> ItemEditCommand
        {
            get
            {
                return _settingEditCommand ?? (_settingEditCommand = new DelegateCommand<SettingEditButton>((e) => {

                    switch (e)
                    {

                        case SettingEditButton.Addition:
                            Quantity++;
                            break;
                        case SettingEditButton.Subtract:
                            if (Quantity < 2) return;
                            else Quantity--;
                            break;
                        case SettingEditButton.Cancel:

                            //    var ordervm = _lineitem._parent;
                            //    ordervm.OrderLines.Remove(_lineitem);
                            //    if (_lineitem._line.Id != 0)
                            //        GConfig.DAL.StateChange(_lineitem._line, System.Data.Entity.EntityState.Deleted);
                            //    ordervm.RefreshValues();
                            this.CloseCommand.Execute(null);


                            break;
                        case SettingEditButton.Save:
                            //        _lineitem.Quantity = Quantity;

                            //    SettingsBuilder _settingsBuilder = new SettingsBuilder();
                            GlobalPosSettings.UpdateConfig(_lineitem.SettingProperty01Name, _lineitem.SettingDisplay01Value);
                            GlobalPosSettings.UpdateConfig(_lineitem.SettingProperty02Name, _lineitem.SettingDisplay02Value);
                            GlobalPosSettings.UpdateConfig(_lineitem.SettingProperty03Name, _lineitem.SettingDisplay03Value);
                            GlobalPosSettings.UpdateConfig(_lineitem.SettingProperty04Name, _lineitem.SettingDisplay04Value);

                            GlobalPosSettings.UpdateConfig(_lineitem.SettingProperty05Name, _lineitem.SettingDisplay05Value);
                            //_settingsBuilder.SettingsUpdate(SelectedEditItem);
                            this.CloseCommand.Execute(null);
                            break;


                        default: break;
                    }

                }));
            }
        }
    }
}

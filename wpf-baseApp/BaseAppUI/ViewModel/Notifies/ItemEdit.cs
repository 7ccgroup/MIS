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
    public enum ItemEditButton
    {
        Addition,
        Subtract,
        Delete,
        Done,
        Cancel,
        Save
    }
    public class ItemEdit : NotifyBase //This class is used to edit line item on receipt in Register Screen SAA.
    {
        ItemListForDisplay _lineitem;
        public ItemEdit(ItemListForDisplay lineitem)
        {
            _lineitem = lineitem;
            SelectedEditItem = lineitem;
            IsPopup = true;
            EnableMask = true;

            // _quantity = _lineitem.;
        }

        public string ItemDescription
        {
            get { return _lineitem.vItemDesc1; }
        }


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

        private ItemListForDisplay _SelectedEditItem;
        public ItemListForDisplay SelectedEditItem
        {
            get { return _SelectedEditItem; }
            set { _SelectedEditItem = _lineitem; }
        }

        private DelegateCommand<ItemEditButton> _ItemEditCommand;

        public DelegateCommand<ItemEditButton> ItemEditCommand
        {
            get
            {
                return _ItemEditCommand ?? (_ItemEditCommand = new DelegateCommand<ItemEditButton>((e) => {

                    switch (e)
                    {

                        case ItemEditButton.Addition:
                            Quantity++;
                            break;
                        case ItemEditButton.Subtract:
                            if (Quantity < 2) return;
                            else Quantity--;
                            break;
                        case ItemEditButton.Cancel:
                            
                        //    var ordervm = _lineitem._parent;
                        //    ordervm.OrderLines.Remove(_lineitem);
                        //    if (_lineitem._line.Id != 0)
                        //        GConfig.DAL.StateChange(_lineitem._line, System.Data.Entity.EntityState.Deleted);
                        //    ordervm.RefreshValues();
                            this.CloseCommand.Execute(null);


                            break;
                        case ItemEditButton.Save:
                            //        _lineitem.Quantity = Quantity;

                            ItemsBuilder _itemsBuilder = new ItemsBuilder();
                            if (SelectedEditItem.ActionMode == "Item Add")
                            {
                                //_itemsBuilder.RetrieveNextItemID();
                                _itemsBuilder.ItemAdd(SelectedEditItem);
                            }
                            else
                            {
                                _itemsBuilder.ItemUpdate(SelectedEditItem);
                            }
                            this.CloseCommand.Execute(null);
                            break;


                        default: break;
                    }

                }));
            }
        }
    }
}

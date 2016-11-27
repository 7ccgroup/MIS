using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BaseAppUI.Controls
{

    public class ToggleItem:NotifyProperty
    {
        Action _selectionchanged = null;
        public  ToggleItem(Action selectionchanged=null ){
            _selectionchanged = selectionchanged;

    }
        public string Key { get; set; }
        public string Text { get; set; }
        public object Value { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
            OnPropertyChanged("IsSelected");
            if (_selectionchanged != null)
                _selectionchanged.Invoke();
                    
            }
        }
        public object Content { get; set; }


    }
   public class ToggleGroup:Control
    {
       static ToggleGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleGroup), new FrameworkPropertyMetadata(typeof(ToggleGroup)));
        }

       public IList<ToggleItem> Items
       {
           get { return (IList<ToggleItem>)GetValue(ItemsProperty); }
           set { SetValue(ItemsProperty, value); }
       }
       public object Value
       {
           get { return GetValue(ValueProperty); }
           set { SetValue(ValueProperty, value); }
       }

       public bool MultipleSelect
       {
           get { return (bool)GetValue(MultipleSelectProperty); }
           set { SetValue(MultipleSelectProperty, value); }
       }

       public bool ReverseSelect
       {
           get { return (bool)GetValue(ReverseSelectProperty); }
           set { SetValue(ReverseSelectProperty, value); }
       }

      
       public static readonly DependencyProperty ItemsProperty =
          DependencyProperty.Register("Items", typeof(IList<ToggleItem>), typeof(ToggleGroup), new PropertyMetadata(null));
       
       public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(object), typeof(ToggleGroup), new PropertyMetadata(null));

       public static readonly DependencyProperty MultipleSelectProperty =
       DependencyProperty.Register("MultipleSelect", typeof(bool), typeof(ToggleGroup), new PropertyMetadata(false));
       public static readonly DependencyProperty ReverseSelectProperty =
     DependencyProperty.Register("ReverseSelect", typeof(bool), typeof(ToggleGroup), new PropertyMetadata(true));
        public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(ToggleItem), typeof(ToggleGroup), new PropertyMetadata(null));
        public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ToggleGroup), new PropertyMetadata(null));


        public ToggleItem SelectedItem
        {
            get { return (ToggleItem)GetValue(SelectedItemProperty);}
            set { SetValue(SelectedItemProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

       private DelegateCommand<ToggleItem> _switchCommand;
       public DelegateCommand<ToggleItem> SwitchCommand
       {
           get
           {
               return _switchCommand ?? (_switchCommand = new DelegateCommand<ToggleItem>((e) =>
               {
                   if (e == null) return;

                   if (MultipleSelect || (ReverseSelect && SelectedItem!=null && SelectedItem==e))
                   {
                       e.IsSelected = !e.IsSelected;

                       if (!MultipleSelect)
                       {
                           if (e.IsSelected == false)
                               this.SetValue(ValueProperty, null);
                           else this.SetValue(ValueProperty, e.Value);
                       }
                      
                   }
                   else
                   {
                      

                       if ( e != SelectedItem)
                       {
                           if (SelectedItem != null)
                               SelectedItem.IsSelected = false;


                           SelectedItem = e;
                           SelectedItem.IsSelected = true;
                           this.SetValue(ValueProperty, SelectedItem.Value);

                       }
                  
                   }


                 
                   
               }));
           }
       }

       public override void OnApplyTemplate()
       {
           base.OnApplyTemplate();

           
           if(this.Items!=null&& this.Items.Any()){

               if (!MultipleSelect)
               {

                   var selected = this.Items.FirstOrDefault(n => n.Value == Value);

                   this.SwitchCommand.Execute(selected);

               }
          

              

           }
       }
    }
}

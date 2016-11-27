using BaseAppUI.Configuration;
using BaseAppUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Sections
{
   public class DashBoard : ISection
    {

       MainVM _parent;

       public DashBoard(MainVM parent)
       {
           _parent = parent;

       }

        DelegateCommand _closeWindowCommand;

        public DelegateCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ?? (_closeWindowCommand = new DelegateCommand(() =>
                {
                    if(BaseAppUI.Properties.Settings.Default.LoginScreen=="Show")
                        _parent.SwitchToView(SectionType.Login); //added by SAA to go back to login screen from Dashboard.
                    else
                        Application.Current.Shutdown();
                   

                }));
            }
        }



        private DelegateCommand<SectionType> _switchToSectionCommand;

        public DelegateCommand<SectionType> SwitchToSectionCommand
        {
            get
            {
                return _switchToSectionCommand ?? (_switchToSectionCommand = new DelegateCommand<SectionType>((e) =>
                {
                    if (GConfig.POSuserAccess == "ADMIN") //Added by SAA on 6/3/16 for user access
                        _parent.SwitchToView(e);

                    if (GConfig.POSuserAccess=="USER" && (e ==SectionType.Register || e ==SectionType.Orders ))
                        _parent.SwitchToView(e);


                }));
            }
        }



        public SectionType PreviousSection { get; set; }
        public SectionType SectionType
        {
            get { return Model.SectionType.Dashboard; }
        }
    }
}

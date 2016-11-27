using BaseAppUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Sections
{
    public interface ISection
    {
        SectionType PreviousSection { get; set; }
        SectionType SectionType { get;  }
    }
}

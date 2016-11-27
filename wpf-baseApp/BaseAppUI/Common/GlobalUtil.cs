using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Configuration;
using System.IO;
using System.Windows.Controls;

namespace BaseAppUI.Common
{
    public enum AlphaNumericKeybordButtonType
    {
        Numeric,
        Scale,
        Reset,
        Double,
        AlphaNumeric

    }
    public class TextBoxOutputter : TextWriter
    {
        TextBox textBox = null;
        public TextBoxOutputter(TextBox output)
        {
            textBox = output;
        }
        public override void Write(char value)
        {
            base.Write(value);
            textBox.Dispatcher.BeginInvoke(new Action(()=>{ textBox.AppendText(value.ToString()); }));
        }
        public override Encoding Encoding
        {
            get
            {
                return System.Text.Encoding.UTF8;
                //throw new NotImplementedException();
            }
        }

    }
    public class AlphaNumericKeybordButton
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public AlphaNumericKeybordButtonType Type { get; set; }

    }
    public static class GlobalUtil
    {

        public static List<TSource> ToList<TSource>(this DataTable datatable) where TSource : new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldsNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                  select new { Name = aProp.Name, type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType }).ToList();

            var dataTblFieldsNames = (from DataColumn aHeader in datatable.Columns select new { Name = aHeader.ColumnName, type = aHeader.DataType }).ToList();

            var commonFields = objFieldsNames.Intersect(dataTblFieldsNames).ToList();

            foreach (DataRow dataRow in datatable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                    var value = (dataRow[aField.Name] == DBNull.Value) ? null : dataRow[aField.Name];
                    propertyInfos.SetValue(aTSource, value, null);
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }


    }

    public class PosVariables
    {
        public static readonly DependencyProperty XProperty =
            DependencyProperty.RegisterAttached("X", typeof(double),
                typeof(PosVariables), new FrameworkPropertyMetadata(null));

        public static void SetX(UIElement element, double value)
        {
            element.SetValue(XProperty, value);
        }
        public static double GetX(UIElement element)
        {
            return (Double)element.GetValue(XProperty);
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.RegisterAttached("Y", typeof(double),
                typeof(PosVariables), new FrameworkPropertyMetadata(null));

        public static void SetY(UIElement element, double value)
        {
            element.SetValue(YProperty, value);
        }
        public static double GetY(UIElement element)
        {
            return (Double)element.GetValue(YProperty);
        }
    }

    public static class GlobalPosSettings
    {
        public static void GetAppsettings()
        {

            var config = ConfigurationManager.OpenExeConfiguration("C:\\ACdev\\GithubRepo\\NewRepo\\wpf-pos\\BaseAppUI.\\bin\\Debug\\BaseAppUI.vshost.exe");
            var SettingNames = new List<string>();
            foreach (ConfigurationSectionGroup csg in config.SectionGroups)
                SettingNames.AddRange(GetPosSettingsList(csg));
            foreach (ConfigurationSection cs in config.Sections)
                SettingNames.Add(cs.SectionInformation.SectionName);
            MessageBox.Show("hello");

        }
        public static void showConfig()
        {
            BaseAppUI.Properties.Settings.Default.SettingsKey.Count();
            var POScs = ConfigurationManager.GetSection("BaseAppUI.Properties.Settings");
            ClientSettingsSection cs = (ClientSettingsSection)POScs;
            MessageBox.Show(BaseAppUI.Properties.Settings.Default .Properties.Count.ToString());

            var SettingNames = new List<string>();

            foreach (var keyProp in BaseAppUI.Properties.Settings.Default.Properties)
            {
                System.Configuration.SettingsProperty vas;
                vas = (System.Configuration.SettingsProperty)keyProp;
                SettingNames.Add("Key : "+ vas.Name.ToString()+" Value: "+ vas.DefaultValue.ToString());
                Console.WriteLine("Key : {0}, Value:{1}", vas.Name,vas.DefaultValue);
            }
            
        }
        public static string GetConfig(string propertyName)
        {
          
            if (propertyName != "" && propertyName != null)
            {
                string xmlFileVshostPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                System.Xml.XmlDocument xmlFileVshost = new System.Xml.XmlDocument();
                xmlFileVshost.Load(xmlFileVshostPath);
                System.Xml.XmlNode Filexmlnode;
                string nodePath = "configuration/applicationSettings/BaseAppUI.Properties.Settings/setting[@name='" + propertyName + "']";
                Filexmlnode = xmlFileVshost.SelectSingleNode(nodePath);
                //  Filexmlnode.ChildNodes[0].InnerText = value;

                return Filexmlnode.ChildNodes[0].InnerText;
            }
            else
                return "";
        }
   
       
        public static List<string> GetPosSettingsList(ConfigurationSectionGroup ConfigSectionGroup)
        {
            var PosSettingsName = new List<string>();

            foreach (ConfigurationSectionGroup csg in ConfigSectionGroup.SectionGroups)
                PosSettingsName.AddRange(GetPosSettingsList(csg));

            foreach (ConfigurationSection cs in ConfigSectionGroup.Sections)
                PosSettingsName.Add(ConfigSectionGroup.SectionGroupName + "/" + cs.SectionInformation.SectionName);

            return PosSettingsName;
        }
        public static void UpdateConfig(string key,string value)
        {
           string sectiongroup = "applicationSettings";
            if (key != "" && key !=null)
            {
                string xmlFileVshostPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                System.Xml.XmlDocument xmlFileVshost = new System.Xml.XmlDocument();
                xmlFileVshost.Load(xmlFileVshostPath);
                System.Xml.XmlNode Filexmlnode;
                string nodePath = "configuration/applicationSettings/BaseAppUI.Properties.Settings/setting[@name='" + key + "']";
                Filexmlnode = xmlFileVshost.SelectSingleNode(nodePath);
                Filexmlnode.ChildNodes[0].InnerText = value;

                xmlFileVshost.Save(xmlFileVshostPath);
                //       _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(sectiongroup);

                string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.
                    GetExecutingAssembly().GetName().CodeBase);
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                string xmlFileExePath = AppPath + "\\BaseAppUI.exe.Config";

                System.Xml.XmlDocument xmlFileExe = new System.Xml.XmlDocument();
                xmlFileExe.Load(xmlFileExePath);
                System.Xml.XmlNode FilexmlExenode;
                string ExenodePath = "configuration/applicationSettings/BaseAppUI.Properties.Settings/setting[@name='" + key + "']";
                FilexmlExenode = xmlFileVshost.SelectSingleNode(ExenodePath);
                FilexmlExenode.ChildNodes[0].InnerText = value;

                xmlFileVshost.Save(xmlFileExePath);

                BaseAppUI.Properties.Settings.Default.Reload();
            }
        }
     
    }

}

//foreach (var keyProp in BaseAppUI.Properties.Settings.Default.Properties)
//{
//    System.Configuration.SettingsProperty vas;
//    vas = (System.Configuration.SettingsProperty)keyProp;
//    SettingNames.Add("Key : " + vas.Name.ToString() + " Value: " + vas.DefaultValue.ToString());
//    if (vas.Name.ToString() == propertyName)
//    {
//        Console.WriteLine("Key : {0}, Value:{1}", vas.Name, vas.DefaultValue);
//      //  MessageBox.Show(vas.DefaultValue.ToString());
//        return vas.DefaultValue.ToString();
//    }

//}
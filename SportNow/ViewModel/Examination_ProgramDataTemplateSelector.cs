using System;
using System.Diagnostics;
using SportNow.Model;
using Xamarin.Forms;

namespace SportNow.ViewModel
{
    public class Examination_ProgramDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate ExpandedTemplate { get; set; }
        public DataTemplate NotExpandedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            /*Debug.Print("(Examination_Program)item).isExpanded = "+(((Examination_Program)item).isExpanded));
            if (((Examination_Program)item).isExpanded == true)
            {
                Debug.Print("(Examination_Program)item).isExpanded  TRUE");
                return ExpandedTemplate;
            }
            else
            {
                Debug.Print("(Examination_Program)item).isExpanded  FALSE");
                return NotExpandedTemplate;
            }*/
            return ((Examination_Program)item).isExpanded == true ? ExpandedTemplate : NotExpandedTemplate;
        }
    }
}
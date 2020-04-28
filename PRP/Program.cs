using System;
using System.Windows.Forms;

using PRP.PPL.Data.MIS.UserManagement.ImageSelectorSingle;
using PRP.PPL.Data.MIS.UserManagement.ImageSelector;

namespace PRP
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MultipleImageUploader());
            Application.Run(new SingleImageUploader());
        }
    }
}



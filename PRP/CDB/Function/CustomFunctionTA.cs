using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRP.PPL.Function
{
    class CustomFunctionTA
    {
        public static void intKeyPressInput(System.Windows.Forms.KeyPressEventArgs e)
        {
            //****************** How to Use **********************// 
            //CustomFunctionTA.intKeyPressInput(e);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public static void floatKeyPressInput(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //****************** How to Use **********************// 
            //CustomFunctionTA.floatKeyPressInput(sender,e);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}

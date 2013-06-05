using System;
using Eto.Drawing;
using Eto.Forms;
using Eto.Xaml;


namespace CodeCreatorDSL
{
    class ButtonCommands : Form
    {
        void OnBtnExitClick(object sender, EventArgs e)
        {
            Dialog confirm = new Dialog();
            confirm.Title = "Confirm";

            var layout = new DynamicLayout(confirm, new Padding(20, 5), new Size(10, 10));
            layout.Add(new Label { Text = "Are You Sure That You Want to Close the Program? All non-saved Data will be Lost.", Font = new Font(SystemFont.Bold, 8), HorizontalAlign = HorizontalAlign.Center });

            var btn_yes = new Button { Text = "Yes" };
            var btn_no = new Button { Text = "No" };
            layout.AddCentered(btn_yes);
            layout.AddCentered(btn_no);

            confirm.WindowState = WindowState.Normal;

            btn_yes.Click += delegate
            {
                Close();
                confirm.Close();
            };

            btn_no.Click += delegate
            {
                confirm.Close();
            };

            confirm.ShowDialog(Parent);


        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public class CustomButton : Button
    {
        int oldWidth, oldHeight, oldX, oldY;
        int newWidth, newHeight, newX, newY;
        public CustomButton()
        {
            MouseEnter += (sender, args) =>
            {
                oldWidth = this.Width;
                oldHeight = this.Height;
                oldX = this.Location.X;
                oldY = this.Location.Y;
                newWidth = oldWidth * 9 / 10;
                newHeight = oldHeight * 9 / 10;
                newX = oldX + (oldWidth - newWidth) / 2;
                newY = oldY + (oldHeight - newHeight) / 2;
                Location = new Point(newX, newY);
                ClientSize = new Size(newWidth, newHeight);
            };

            MouseLeave += (sender, args) =>
            {
                oldWidth = this.Width;
                oldHeight = this.Height;
                oldX = this.Location.X;
                oldY = this.Location.Y;
                newWidth = oldWidth * 10 / 9;
                newHeight = oldHeight * 10 / 9;
                newX = oldX + (oldWidth - newWidth) / 2;
                newY = oldY + (oldHeight - newHeight) / 2;
                Location = new Point(newX, newY);
                ClientSize = new Size(newWidth, newHeight);
            };

            
        }

    }
}

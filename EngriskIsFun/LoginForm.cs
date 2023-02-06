using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.ClientSize = new Size(864, 480);
            this.Text = "Engrisk is fun?";
            DrawBackground();
            InitializeLoginBox();
        }

        private void InitializeLoginBox()
        {
            Panel loginBox = new Panel();
            loginBox.Size = new Size(240, 320);
            loginBox.Location = new Point(50, 50);
            loginBox.BackColor = Color.DarkKhaki;
            loginBox.BorderStyle = BorderStyle.FixedSingle;

            Label lbUsername = new Label();
            lbUsername.Text = "Tên đăng nhập";
            lbUsername.Font = new Font("Arial", 8, FontStyle.Bold);
            lbUsername.Size = new Size(90, 25);
            lbUsername.Location = new Point(20, 50);

            Label lbPassword = new Label();
            lbPassword.Text = "Mật khẩu";
            lbPassword.Font = new Font("Arial", 8, FontStyle.Bold);
            lbPassword.Size = new Size(90, 25);
            lbPassword.Location = new Point(20, 100);

            TextBox tbUsername = new TextBox();
            tbUsername.Size = new Size(90, 25);
            tbUsername.Location = new Point(120, 45);

            TextBox tbPassword = new TextBox();
            tbPassword.Size = new Size(90, 25);
            tbPassword.Location = new Point(120, 95);
            tbPassword.PasswordChar = '*';
            tbPassword.MaxLength = 8;

            Button signIn = new Button();
            signIn.Text = "Đăng nhập";
            signIn.Size = new Size(90, 30);
            signIn.Location = new Point(20, 150);
            signIn.Click += (sender, args) =>
            {
                if (tbUsername.Text == "")
                {
                    MessageBox.Show("Điền tên đăng nhập!");
                    return;
                }
                if (tbPassword.Text == "")
                {
                    MessageBox.Show("Điền mật khẩu!");
                    return;
                }
                User newUser = new User();
                newUser.UserID = new Random().Next(10000000, 100000000).ToString();
                newUser.Username = tbUsername.Text.ToString();
                newUser.Password = tbPassword.Text.ToString().Replace(" ", "");
                newUser.Level = 0;

                DataClasses1DataContext db = new DataClasses1DataContext();

                var username = db.Users.Where(a => a.Username == newUser.Username).Select(a => a.Username).ToList();
                if (username.Count == 0)
                {
                    MessageBox.Show("Tên đăng nhập không tồn tại!");
                }
                var password = db.Users.Where(a => a.Username == newUser.Username).Select(a => a.Password).ToList();
                if(password[0].ToString().Replace(" ","") == newUser.Password) {
                    MessageBox.Show("Đăng nhập thành công!");
                }
                else
                {
                    MessageBox.Show("Mật khẩu sai!");
                }
            };

            Button signUp = new Button();
            signUp.Text = "Đăng ký";
            signUp.Size = new Size(90, 30);
            signUp.Location = new Point(122, 150);
            signUp.Click += (sender, args) =>
            {
                if (tbUsername.Text == "")
                {
                    MessageBox.Show("Điền tên đăng nhập!");
                    return;
                }
                if (tbPassword.Text == "")
                {
                    MessageBox.Show("Điền mật khẩu!");
                    return;
                }
                User newUser = new User();
                newUser.UserID = new Random().Next(10000000, 100000000).ToString();
                newUser.Username = tbUsername.Text.ToString();
                newUser.Password = tbPassword.Text.ToString().Replace(" ", "");
                newUser.Level = 0;

                DataClasses1DataContext db = new DataClasses1DataContext();

                var username = db.Users.Where(a => a.Username == newUser.Username).Select(a => a.Username).ToList();
                // MessageBox.Show(username.ToString());
                if (username.Count != 0)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    return;
                }
                db.Users.InsertOnSubmit(newUser);
                db.SubmitChanges();
                MessageBox.Show("Đăng ký thành công!");
            };

            loginBox.Controls.Add(lbUsername);
            loginBox.Controls.Add(lbPassword);
            loginBox.Controls.Add(tbUsername);
            loginBox.Controls.Add(tbPassword);
            loginBox.Controls.Add(signIn);
            loginBox.Controls.Add(signUp);
            this.Controls.Add(loginBox);
        }

        private void DrawBackground()
        {
            this.BackgroundImage = Image.FromFile("Materials/background.png");
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Size = new Size(864, 480);
            this.Text = "Engrisk is fun";
            this.BackgroundImage = Image.FromFile("Materials/background.png");
            InitializeLoginBox();
        }

        private Panel loginBox = new Panel();

        private Label lbUsername = new Label();
        private Label lbPassword = new Label();
        private Label lbConfirm = new Label();

        private TextBox tbUsername = new TextBox();
        private TextBox tbPassword = new TextBox();
        private TextBox tbConfirm = new TextBox();

        private Button signIn = new Button();
        private Button signUp = new Button();
        private Button accept = new Button();
        private Button cancel = new Button();

        private void InitializeLoginBox()
        {
            loginBox.Size = new Size(240, 280);
            loginBox.Location = new Point(50, 50);
            loginBox.BackColor = Color.DarkKhaki;
            loginBox.BorderStyle = BorderStyle.FixedSingle;

            lbUsername.Text = "Tên đăng nhập";
            lbUsername.Font = new Font("Arial", 8, FontStyle.Bold);
            lbUsername.Size = new Size(90, 25);
            lbUsername.Location = new Point(20, 50);

            lbPassword.Text = "Mật khẩu";
            lbPassword.Font = new Font("Arial", 8, FontStyle.Bold);
            lbPassword.Size = new Size(90, 25);
            lbPassword.Location = new Point(20, 100);

            tbUsername.Size = new Size(90, 25);
            tbUsername.Location = new Point(120, 50);

            tbPassword.Size = new Size(90, 25);
            tbPassword.Location = new Point(120, 100);
            tbPassword.PasswordChar = '*';
            tbPassword.MaxLength = 8;

            lbConfirm.Text = "Xác nhận \nmật khẩu";
            lbConfirm.Font = new Font("Arial", 8, FontStyle.Bold);
            lbConfirm.Size = new Size(90, 30);
            lbConfirm.Location = new Point(20, 150);

            tbConfirm.Size = new Size(90, 25);
            tbConfirm.Location = new Point(120, 155);
            tbConfirm.PasswordChar = '*';
            tbPassword.MaxLength = 8;

            signIn.Text = "Đăng nhập";
            signIn.Size = new Size(90, 30);
            signIn.Location = new Point(20, 150);
            signIn.Click += SignIn_Click;

            signUp.Text = "Đăng ký";
            signUp.Size = new Size(90, 30);
            signUp.Location = new Point(122, 150);
            signUp.Click += (sender, args) =>
            {
                loginBox.Controls.Add(lbConfirm);
                loginBox.Controls.Add(tbConfirm);

                loginBox.Controls.Remove(signIn);
                loginBox.Controls.Remove(signUp);
                loginBox.Controls.Add(accept);
                loginBox.Controls.Add(cancel);
            };

            accept.Text = "Xác nhận";
            accept.Size = new Size(90, 30);
            accept.Location = new Point(20, 200);
            accept.Click += Accept_Click;

            cancel.Text = "Huỷ bỏ";
            cancel.Size = new Size(90, 30);
            cancel.Location = new Point(122, 200);
            cancel.Click += (sender, args) =>
            {
                loginBox.Controls.Remove(lbConfirm);
                loginBox.Controls.Remove(tbConfirm);

                loginBox.Controls.Add(signIn);
                loginBox.Controls.Add(signUp);
                loginBox.Controls.Remove(accept);
                loginBox.Controls.Remove(cancel);
            };

            loginBox.Controls.Add(lbUsername);
            loginBox.Controls.Add(lbPassword);
            loginBox.Controls.Add(tbUsername);
            loginBox.Controls.Add(tbPassword);
            loginBox.Controls.Add(signIn);
            loginBox.Controls.Add(signUp);
            this.Controls.Add(loginBox);
        }

        private void SignIn_Click(object sender, EventArgs e)
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
            newUser.UserName = tbUsername.Text.ToString();
            newUser.Password = tbPassword.Text.ToString().Replace(" ", "");
            newUser.Level = 0;

            dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();

            var username = db.Users.Where(a => a.UserName == newUser.UserName).Select(a => a.UserName).ToList();
            if (username.Count == 0)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại!");
            }
            var password = db.Users.Where(a => a.UserName == newUser.UserName).Select(a => a.Password).ToList();
            if (password[0].Replace(" ", "") == newUser.Password)
            {
                MainMenu mainMenu = new MainMenu();
                this.Visible = false;
                if (!mainMenu.IsDisposed) mainMenu.ShowDialog();
                this.Visible = true;
            }
            else
            {
                MessageBox.Show("Mật khẩu sai!");
            }
        }
        private void Accept_Click(object sender, EventArgs e)
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
            if(tbPassword.Text != tbConfirm.Text)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp!");
                return;
            }

            User newUser = new User();
            newUser.UserID = new Random().Next(10000000, 100000000).ToString();
            newUser.UserName = tbUsername.Text.ToString();
            newUser.Password = tbPassword.Text.ToString().Replace(" ", "");
            newUser.Level = 0;

            dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();

            var username = db.Users.Where(a => a.UserName == newUser.UserName).Select(a => a.UserName).ToList();
            if (username.Count != 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!");
                return;
            }
            db.Users.InsertOnSubmit(newUser);
            db.SubmitChanges();
            MessageBox.Show("Đăng ký thành công!");
            cancel.PerformClick();
        }
    }
}

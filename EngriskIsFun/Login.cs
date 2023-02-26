using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.Size = new Size(864, 480);
            this.Text = "ENGRISK IZ FUN";
            this.BackgroundImage = Image.FromFile("Materials/Backgrounds/login.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
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
            loginBox.Size = new Size(330, 220);
            loginBox.Location = new Point(350, 100);
            loginBox.BackColor = Color.White;
            loginBox.BorderStyle = BorderStyle.FixedSingle;

            lbUsername.Text = "Tên đăng nhập";
            lbUsername.Font = new Font("Arial", 13, FontStyle.Bold);
            lbUsername.ForeColor = Color.DodgerBlue;
            lbUsername.Size = new Size(150, 25);
            lbUsername.Location = new Point(20, 50);

            lbPassword.Text = "Mật khẩu";
            lbPassword.Font = new Font("Arial", 13, FontStyle.Bold);
            lbPassword.ForeColor = Color.DodgerBlue;
            lbPassword.Size = new Size(150, 25);
            lbPassword.Location = new Point(20, 100);

            tbUsername.Size = new Size(90, 25);
            tbUsername.Location = new Point(200, 50);

            tbPassword.Size = new Size(90, 25);
            tbPassword.Location = new Point(200, 100);
            tbPassword.PasswordChar = '*';
            tbPassword.MaxLength = 8;

            lbConfirm.Text = "Xác nhận m.khẩu";
            lbConfirm.Font = new Font("Arial", 12, FontStyle.Bold);
            lbConfirm.ForeColor = Color.DodgerBlue;
            lbConfirm.Size = new Size(150, 25);
            lbConfirm.Location = new Point(20, 155);

            tbConfirm.Size = new Size(90, 25);
            tbConfirm.Location = new Point(200, 155);
            tbConfirm.PasswordChar = '*';
            tbPassword.MaxLength = 8;

            signIn.Text = "Đăng nhập";
            signIn.Size = new Size(90, 30);
            signIn.Location = new Point(20, 170);
            signIn.Click += SignIn_Click;

            signUp.Text = "Đăng ký";
            signUp.Size = new Size(90, 30);
            signUp.Location = new Point(200, 170);
            signUp.Click += (sender, args) =>
            {
                tbUsername.Text = "";
                tbPassword.Text = "";
                loginBox.Controls.Add(lbConfirm);
                loginBox.Controls.Add(tbConfirm);

                loginBox.Controls.Remove(signIn);
                loginBox.Controls.Remove(signUp);
                loginBox.Controls.Add(accept);
                loginBox.Controls.Add(cancel);

                loginBox.Size = new Size(330, 280);
            };

            accept.Text = "Xác nhận";
            accept.Size = new Size(90, 30);
            accept.Location = new Point(20, 220);
            accept.Click += Accept_Click;

            cancel.Text = "Huỷ bỏ";
            cancel.Size = new Size(90, 30);
            cancel.Location = new Point(200, 220);
            cancel.Click += (sender, args) =>
            {
                loginBox.Controls.Remove(lbConfirm);
                loginBox.Controls.Remove(tbConfirm);

                loginBox.Controls.Add(signIn);
                loginBox.Controls.Add(signUp);
                loginBox.Controls.Remove(accept);
                loginBox.Controls.Remove(cancel);

                loginBox.Size = new Size(330, 220);
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
            newUser.UserName = tbUsername.Text.ToString();
            newUser.Password = tbPassword.Text.ToString().Replace(" ", "");
            newUser.Level = 0;

            dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();

            var username = db.Users.Where(a => a.UserName == newUser.UserName).Select(a => a.UserName).ToList();
            if (username.Count == 0)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại!");
                return;
            }
            var password = db.Users.Where(a => a.UserName == newUser.UserName).Select(a => a.Password).ToList();
            
            if (password[0].Replace(" ", "") == newUser.Password)
            {
                MainMenu mainMenu = new MainMenu(newUser.UserName);
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

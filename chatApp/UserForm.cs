using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatApp
{
    public partial class UserForm : Form
    {
        //Добавление сообщения
        public void AddMessage(string message)
            => messagesListBox.Items.Add(message);

        //Добавление нового юзера в список получателей сообщения
        public void AddUser(string user)
        {
            if (user != Text && !onlineUsersCheckedListBox.Items.Contains(user))
                onlineUsersCheckedListBox.Items.Add(user);
        }
        public void RemoveUser(string user)
            => onlineUsersCheckedListBox.Items.Remove(user);
        public UserForm()
        {
            InitializeComponent();
            timer1.Start();
            this.BackColor = Color.Silver;
            //выделение одним кликом
            onlineUsersCheckedListBox.CheckOnClick = true;
        }

        //Делегат и ивент на отправку сообщения
        public delegate void MessageSend(string to, string text, string from);
        public event MessageSend Send;

        //Обработчик клика
        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            //Обновление собственного чата
            AddMessage(String.Join(" - ",DateTime.Now.ToString("HH:mm") ,Text, messageTextBox.Text));
            //Публикуем ивент на каждого выделеного пользователя
            foreach (string recipient in onlineUsersCheckedListBox.CheckedItems)
                Send?.Invoke(recipient, messageTextBox.Text, this.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("HH:mm");
        }
    }
}

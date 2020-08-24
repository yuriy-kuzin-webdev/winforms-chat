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
    public partial class AdminForm : Form
    {
        //Коллекция форм для каждого юзера
        //Можно было использовать Application.OpenForms
        private List<UserForm> activeUsers;
        public AdminForm()
        {
            InitializeComponent();
            timer1.Start();
            this.StartPosition = FormStartPosition.CenterScreen;
            //Инициализация коллекции для дальнейшей работы
            activeUsers = new List<UserForm>();
        }

        //Регистрация нового юзера и открытие формы
        private void userNameButton_Click(object sender, EventArgs e)
        {
            //Проверка на уже существующее имя
            if (usersListBox.Items.Contains(userNameTextBox.Text))
            {
                MessageBox.Show("Try another name");
                return;
            }
            //Создание новой формы
            activeUsers.Add(new UserForm());
            //Указатель на новую форму
            var newBie = activeUsers.Last();
            //Имя формы(title) = юзернейм
            newBie.Text = userNameTextBox.Text;
            //Открытие формы
            newBie.Show();
            //Добавление в список нового юзера
            usersListBox.Items.Add(userNameTextBox.Text);
            //Обновление списка юзеров во всех открытых формах
            activeUsers.ForEach(user => activeUsers.ForEach(name => user.AddUser(name.Text)));

            //Подписка на ивент сообщения формы юзера
            newBie.Send += SendMessage;

            //Подписка на ивент закрытия окна
            newBie.FormClosing += (senderform, args) =>
            {
                //Удаление из списка форм юзеров
                activeUsers.Remove(senderform as UserForm);
                //копия имени юзера для удобства
                string leaver = (senderform as Form).Text;
                //Среди всех открытых окон
                activeUsers.ForEach(user =>
                    {
                        //Сообщение о покидании беседы
                        user.AddMessage(leaver + " leaves conversation.");
                        //Удаление из списка юзеров
                        user.RemoveUser(leaver);
                    });
                //Удаление из админского списка
                usersListBox.Items.Remove(leaver);
            };
        }

        //Обработчик события сообщения
        //Ошибка в линк запросе исключена невозможностью отправки сообщения несуществуемому юзеру
        public void SendMessage(string to, string text, string who)
            => activeUsers.FirstOrDefault(usr => usr.Text == to).AddMessage(String.Join(" - ",DateTime.Now.ToString("HH:mm"), who, text));

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("HH:mm");
        }
    }
}

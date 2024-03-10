using System.Windows;
using System.Windows.Input;

namespace InputBox
{
    /// <summary>
    /// Окно ввода.
    /// </summary>
    public partial class InputBoxWindow : Window
    {
        #region fields
        /// <summary>
        /// Статус окна.
        /// </summary>
        private WindowStatus _status;
        #endregion

        #region properties
        /// <summary>
        /// Результат введенный в TextBox.
        /// </summary>
        public int TextBoxResult { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="labelText">Текст на экране.</param>
        /// <param name="windowTitle">Заголовок окна.</param>
        /// <exception cref="ArgumentNullException">В качестве аргумента передан NULL.</exception>
        public InputBoxWindow(string labelText, string windowTitle)
        {
            if (String.IsNullOrWhiteSpace(labelText))
            {
                throw new ArgumentNullException(nameof(labelText), "Пустой текст для label.");
            }

            if (String.IsNullOrWhiteSpace(windowTitle))
            {
                throw new ArgumentNullException(nameof(windowTitle), "Пустой заголовок окна.");
            }

            InitializeComponent();
            this.label.Content = labelText;
            this.Title = windowTitle;
            _status = WindowStatus.Undefined;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Фильтрация вводимого текста.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы собатия.</param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку OK.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBox.Text))
            {
                return;
            }

            TextBoxResult = int.Parse(this.textBox.Text);
            _status = WindowStatus.OK;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку Cancel.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _status = WindowStatus.Cancel;
            this.Close();
        }

        /// <summary>
        /// Обработчик закрытия окна через крестик.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_status == WindowStatus.Undefined)
            {
                _status = WindowStatus.Cancel;
            }
        }

        #endregion

        #region public methods
        /// <summary>
        /// Показ окна.
        /// </summary>
        /// <returns>True, если пользователь нажал на OK.</returns>
        new public bool ShowDialog()
        {
            base.ShowDialog();

            return _status == WindowStatus.OK;
        }
        #endregion
    }
}

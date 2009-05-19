using System;
using System.Windows;
using System.Windows.Input;

namespace Unite.UI.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainView : DraggableWindow
    {
        public MainView(ViewModels.MainView viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            MessageToSend.AcceptsReturn = true;
            MessageToSend.AcceptsTab = true;
            _PreviousKey = Key.None;

            // This is all to manage Ctrl-Enter for newline and Enter to send message.
            // :( I tried using KeyBindings but didn't get it. Please help!
            MessageToSend.PreviewKeyDown += (s, e) =>
                                         {
                                             if(e.Key == Key.None || e.Key == Key.LeftCtrl)
                                             {
                                                 _PreviousKey = e.Key;
                                                 return;
                                             }
                                             if (_PreviousKey != Key.LeftCtrl &&
                                               e.Key == Key.Enter)
                                             {
                                                 _SendMessageCommand(null);
                                                 _PreviousKey = Key.None;
                                                 e.Handled = true;
                                                 return;
                                             }
                                             if (_PreviousKey == Key.LeftCtrl &&
                                                 e.Key == Key.Enter)
                                             {
                                                 _NewLineCommand();
                                                 _PreviousKey = Key.None;
                                                 return;
                                             }

                                             _PreviousKey = Key.None;
                                         };
            MessageToSend.PreviewKeyUp += (s, e) =>
                                              {
                                                  _PreviousKey = Key.None;
                                              };

        }

        private void _NewLineCommand()
        {
            var caretIndex = MessageToSend.CaretIndex;
            MessageToSend.Text = MessageToSend.Text.Insert(MessageToSend.CaretIndex, "\n");
            MessageToSend.CaretIndex = caretIndex + 1;
            _PreviousKey = Key.None;
        }

        private Key _PreviousKey;

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            ((ViewModels.MainView)DataContext).Dispose();
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Resizer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Resizer.CaptureMouse();
            _resizeMouseDownPoint = e.MouseDevice.GetPosition(null);
            _mouseDownSize = new Size(Width, Height);
            _resizing = true;
        }

        private void Resizer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _resizing = false;
            Resizer.ReleaseMouseCapture();
        }

        private void Resizer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizing)
                return;

            var currentPosition = e.MouseDevice.GetPosition(null);
            var moveVector = Point.Subtract(currentPosition, _resizeMouseDownPoint);

            if (_resizing)
            {
                ResizeWindow(moveVector);
                return;
            }
        }

        private void ResizeWindow(Vector dragVector)
        {
            var newWidth = _mouseDownSize.Width  + dragVector.X;
            var newHeight = _mouseDownSize.Height + dragVector.Y;
            Width = Math.Max(newWidth, 200);
            Height = Math.Max(newHeight, 300);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void _SendMessageCommand(object param)
        {
            SendMessage.Focus(); //need to cause MessageToSend to lose focus so binding will update viewmodel
            SendMessage.Command.Execute(null);
            OnMessageSent();
            MessageToSend.Focus();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            OnMessageSent();
        }

        private void OnMessageSent()
        {
            Recipient.Focus();
        }
    }
}

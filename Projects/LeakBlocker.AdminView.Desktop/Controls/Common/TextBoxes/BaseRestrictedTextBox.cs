using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.TextBoxes
{
    internal abstract class BaseRestrictedTextBox : TextBox
    {
        private string previousText;

        protected BaseRestrictedTextBox()
        {
            VerticalContentAlignment = VerticalAlignment.Center;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)//Это надо для запрета ctrl-z и пр. Минут - сбрасывает каретку
        {
            if (previousText != null && IsCorrect(previousText) && !IsCorrect(Text))
            {
                Text = previousText;
                return;
            }

            previousText = Text;

            base.OnTextChanged(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)//Это надо для запрета ввода плохих символов
        {
            string oldText = Text;
            string newText = GetProposedText(e.Text);

            if (IsCorrect(oldText) && !IsCorrect(newText))
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }

        private string GetProposedText(string newText)
        {
            var text = Text;

            if (SelectionStart != -1)
            {
                text = text.Remove(SelectionStart, SelectionLength);
            }

            text = text.Insert(CaretIndex, newText);

            return text;
        }

        protected abstract bool IsCorrect(string text);
    }
}

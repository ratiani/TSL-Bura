using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace TS.Gambling.Bura
{

    /// <summary>
    /// Summary description for BuraMessage
    /// </summary>
    public class BuraMessage
    {
        public BuraMessage()
        {
        }

        public static string GetMessage(string messageText, MessageOption[] options)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<div class='MessageBox' style='z-index:10;'>");
            builder.AppendFormat("<div class='Text'>{0}</div>", messageText);
            builder.Append("<div class='MessageOptions'>");
            foreach (MessageOption option in options)
            {
                builder.AppendFormat(
                    @"
                    <div class='Option {2}' onclick='{1}'>
                        <div class='Left'></div>
                        <div class='Middle'>{0}</div>
                        <div class='Right'></div>
                        <div class='clearer'></div>
                    </div>",
                    option.OptionText, option.OptionAction, option.OptionColor);
            }
            builder.Append("<div class='clearer'></div></div>");
            builder.Append("</div>");
            return builder.ToString();
        }

    }

    public class MessageOption
    {
        public MessageOption(string text, string action, string color)
        {
            _optionText = text;
            _optionAction = action;
            _optionColor = color;
        }

        private string _optionText;
        private string _optionAction;
        private string _optionColor;

        public string OptionColor
        {
            get { return _optionColor; }
            set { _optionColor = value; }
        }

        public string OptionAction
        {
            get { return _optionAction; }
            set { _optionAction = value; }
        }

        public string OptionText
        {
            get { return _optionText; }
            set { _optionText = value; }
        }

    }

}
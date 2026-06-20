using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;
using System.Windows;
using System.Net.Sockets;

namespace WPF_ChatRoom.Models
{
    public class ChatMessage
    {
        public string EndPoint { get; set; }
        public string Content { get; set; }
        public bool IsSelf { get; set; }  // 是否自己发的
        public DateTime Time { get; set; }

        public ChatMessage(string EndPoint,string Content, bool isSelf)
        {
            this.EndPoint = EndPoint;
            this.Content = Content;
            this.IsSelf = isSelf;
            Time=DateTime.Now;
        }

        // 自己发的靠右（蓝色），别人发的靠左（灰色）
        public HorizontalAlignment Alignment
        {
            get
            {
                if (IsSelf)
                    return HorizontalAlignment.Right;
                else
                    return HorizontalAlignment.Left;
            }
        }

        public Brush BubbleColor =>
            IsSelf ? Brushes.DodgerBlue : Brushes.Gray;
    }
}

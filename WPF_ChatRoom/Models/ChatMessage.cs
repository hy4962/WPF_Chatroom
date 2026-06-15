using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;
using System.Windows;
using System.Net.Sockets;

namespace WPF_ChatRoom.Models
{
    internal class ChatMessage
    {
        
        public string Content { get; set; }
        public bool IsSelf { get; set; }  // 是否自己发的

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

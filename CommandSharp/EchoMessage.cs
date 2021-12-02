/* EchoMessage.cs
 * ----------------
 * PROJECT: CommandSharp
 * AUTHOR: WinMister332
 * LICENSE: MIT (https://opensource.org/licenses/MIT)
 * ----------------
 * NOTICE:
 *      You must include a copy of "license.txt" if you use CommandSharp. If you're using code pulled from the repo, you must also include this header.
 * ----------------
 * Copyright (c) 2017-2021 NerdHub Technologies, All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommandSharp
{
    public sealed class EchoMessage
    {
        private List<MessageNode> messages;

        public EchoMessage(params MessageNode[] nodes)
        {
            messages = new List<MessageNode>();
            messages.AddRange(nodes);
        }

        public MessageNode[] GetMessageNodes() => messages.ToArray();

        public void Display()
        {
            //Get the "orginal" color.
            var col = Console.ForegroundColor;
            //Get each node.
            foreach (MessageNode node in GetMessageNodes())
            {
                var msgCol = node.GetMessageColor();
                var msgType = node.GetMessageType();
                string message = "";
                if (msgType == MessageType.TEXT)
                    message = node.GetMessageText();
                else if (msgType == MessageType.DIRECTORY_PATH)
                {
                    var x = node.GetMessageText();
                    if (!(x.ToLower().Equals(GlobalSettings.CurrentDirectory)))
                        message = GlobalSettings.CurrentDirectory;
                    else
                        message = x;
                }
                else if (msgType == MessageType.MACHINE_NAME)
                {
                    var x = node.GetMessageText();
                    if (!(x.ToLower().Equals(GlobalSettings.MachineName)))
                        message = GlobalSettings.MachineName;
                    else
                        message = x;
                }
                else
                {
                    var x = node.GetMessageText();
                    if (!(x.ToLower().Equals(GlobalSettings.CurrentUser)))
                        message = GlobalSettings.CurrentUser;
                    else
                        message = x;
                }

                Console.ForegroundColor = msgCol;
                Console.Write(message);
                Console.ForegroundColor = col;
            }
        }
    }

    public sealed class MessageNode
    {
        private string _message;
        private MessageType _messageType;
        private ConsoleColor _color;
        
        MessageNode(MessageType type, ConsoleColor color, string message)
        {
            _message = message;
            _color = color;
            _messageType = type;
        }

        public MessageType GetMessageType() => _messageType;
        public ConsoleColor GetMessageColor() => _color;
        public string GetMessageText() => _message;

        public static MessageNode NewMessageNode(string message, ConsoleColor color = ConsoleColor.White, MessageType messageType = MessageType.TEXT)
            => new MessageNode(messageType, color, message);

        public static MessageNode NEWLINE = NewMessageNode(Environment.NewLine);
        public static MessageNode WHITESPACE = NewMessageNode(" ");
        public static MessageNode USERNAME = NewMessageNode(GlobalSettings.CurrentUser, ConsoleColor.DarkCyan, MessageType.USERNAME);
        public static MessageNode MACHINE_NAME = NewMessageNode(GlobalSettings.MachineName, ConsoleColor.Green, MessageType.MACHINE_NAME);
        public static MessageNode CURRENT_DIRECTORY = NewMessageNode(GlobalSettings.CurrentDirectory, ConsoleColor.Yellow, MessageType.DIRECTORY_PATH);
    }

    public enum MessageType
    {
        //Plain-text
        TEXT = 0,
        USERNAME = 1,
        MACHINE_NAME = 2,
        DIRECTORY_PATH = 3
    }
}

﻿namespace ET
{
    public class MessageAttribute: BaseAttribute
    {
        public ushort Opcode
        {
            get;
        }

        public MessageAttribute(ushort opcode = 0)
        {
            this.Opcode = opcode;
        }
    }
}
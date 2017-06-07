using System;

namespace cocosocket4unity
{
    public class LVProtocol : Protocol
    {
        private int status;
        private int h;
        private int l;
        private short len;
        private ByteBuf frame;

        public LVProtocol()
        {

        }
        /**
		 * 分帧逻辑
		 * 
		 **/
        public ByteBuf TranslateFrame(ByteBuf src)
        {
            while (src.ReadableBytes() > 0)
            {
                switch (status)
                {
                    case 0://获取长度高位
                        h = src.ReadByte();
                        status = 1;
                        break;
                    case 1://获取长度低位
                        l = src.ReadByte();
                        len = (short)(((h << 8) & 0x0000ff00) | (l));
                        frame = new ByteBuf(len + 2);
                        frame.WriteShort(len);//写入长度先
                        status = 2;
                        break;
                    case 2:
                        int min = frame.WritableBytes();//当前帧可写入的
                        min = src.ReadableBytes() < min ? src.ReadableBytes() : min;
                        if (min > 0)
                        {
                            frame.WriteBytes(src, min);
                        }
                        if (frame.WritableBytes() <= 0)
                        {
                            status = 0;//帧写满了
                            return frame;
                        }
                        break;
                }
            }
            return null;
        }
        /**
		 * 头部长度
		 * 
		 */
        public int HeaderLen()
        {
            return 2;
        }
    }
}


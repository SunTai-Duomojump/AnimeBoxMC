using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace AnimeBoxMC.Model.Media
{
    /// <summary>
    /// 使用Bass.dll实现的播放逻辑
    /// </summary>
    public class BassPlayer : ISongPlayer, IDisposable
    {
        //BassNet.Registration("a44281071@sina.com", "2X333118282922");
        public BassPlayer()
        {
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            _PlayerState = PlayState.Ready;
            _UpdateTimer.Tick += _UpdateTimer_Tick; //时间触发器事件绑定
        }

        private int _channel = 0;         //歌曲装载通道
        private const int INTERVAL = 1;   //时间触发器间隔时间（毫秒）    
        private BASSTimer _UpdateTimer;   //当前播放的时间触发器

        #region 对象属性

        private SongBase _Song;
        /// <summary>
        /// 获取或设置正在装载的音乐
        /// </summary>
        public SongBase Song
        {
            get { return _Song; }
            set
            {
                _Song = value;
                this.Open(_Song.FilePath);
            }
        }

        private PlayState _PlayerState;
        /// <summary>
        /// 当前播放的状态
        /// </summary>
        public PlayState PlayerState
        {
            get { return _PlayerState; }
        }

        /// <summary>
        /// 获取当前音频的总时间长度
        /// </summary>
        public double Duration
        {
            get { return Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetLength(_channel)); }
        }
        /// <summary>
        /// 获取或设置当前播放时间
        /// </summary>
        public double Position
        {
            get { return Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetPosition(_channel)); }
            set { Bass.BASS_ChannelSetPosition(_channel, Bass.BASS_ChannelSeconds2Bytes(_channel, value)); }
        }

        /// <summary>
        /// 获取或设置整体声音大小
        /// </summary>
        public float Volume
        {
            get { return Bass.BASS_GetVolume(); }
            set { Bass.BASS_SetVolume(value); }
        }

        #endregion

        #region 对象方法

        /// <summary>
        /// _UpdateTimer触发器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _UpdateTimer_Tick(object sender, EventArgs e)
        {
            //我什么都没干
        }

        /// <summary>
        /// 加载歌曲文件至播放通道
        /// </summary>
        /// <param name="path"></param>
        private void Open(string path)
        {
            if (0 != _channel)//有歌曲已经加载
            {
                Bass.BASS_StreamFree(_channel);
                _PlayerState = PlayState.Ready;
            }
            _UpdateTimer = new BASSTimer(INTERVAL);//计时触发器重置
            _channel = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);//将歌曲载入通道
            if (0 != _channel)//歌曲成功已经加载
            {
                float channelVol = 1.0f;//当前音频通道的音量
                Bass.BASS_ChannelSetAttribute(_channel, BASSAttribute.BASS_ATTRIB_VOL, channelVol);
                _PlayerState = PlayState.Stop;
            }
            else
            {
                throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
            }
        }
        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            if (PlayerState != PlayState.Stop || PlayerState != PlayState.Pause)
            { return; }

            if (Bass.BASS_ChannelPlay(_channel, false))
            {
                _UpdateTimer.Start();
                _PlayerState = PlayState.Play;
            }
            else
            {
                throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
            }
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (PlayerState != PlayState.Play)
            { return; }

            if (Bass.BASS_ChannelPause(_channel))
            {
                _UpdateTimer.Stop();
                _PlayerState = PlayState.Pause;
            }
            else
            {
                throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
            }
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            if (PlayerState != PlayState.Play)
            { return; }

            if (Bass.BASS_ChannelStop(_channel))
            {
                _UpdateTimer = new BASSTimer(INTERVAL);//计时触发器重置               
                Bass.BASS_ChannelSetPosition(_channel, Bass.BASS_ChannelSeconds2Bytes(_channel, 0)); //初始播放位置
                _PlayerState = PlayState.Stop;
            }
            else
            {
                throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
            }
        }
        /// <summary>
        /// 清空歌曲占用，播放器处于空闲状态
        /// </summary>
        public void Free()
        {
            if (PlayerState == PlayState.Ready)
            { return; }

            if (Bass.BASS_SampleFree(_channel))
            {
                _UpdateTimer.Dispose();
                _PlayerState = PlayState.Ready;
            }
            else
            {
                throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
            }
        }

        #endregion

        /// <summary>
        /// 销毁本对象，慎用
        /// </summary>
        public void Dispose()
        {
            _UpdateTimer.Dispose();
            //Bass.BASS_SampleFree(_channel);
            Bass.BASS_Free();
        }
    }
}

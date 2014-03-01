using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeBoxMC.Model.Media
{
    /// <summary>
    /// 播放状态
    /// </summary>
    public enum PlayState
    {
        /// <summary>
        /// 已经就绪，但是未加载音频
        /// </summary>
        Ready,
        /// <summary>
        /// 已经停止
        /// </summary>
        Stop,
        /// <summary>
        /// 正在播放
        /// </summary>
        Play,
        /// <summary>
        /// 暂停中
        /// </summary>
        Pause
    }

    /// <summary>
    /// I 播放器
    /// </summary>
    interface ISongPlayer
    {
        /// <summary>
        /// 当前播放状态
        /// </summary>
        PlayState PlayerState { get; }
        /// <summary>
        /// 正在装载播放的音乐
        /// </summary>
        SongBase Song { get; set; }
        /// <summary>
        /// 声音大小
        /// </summary>
        float Volume { get; set; }
        /// <summary>
        /// 当前播放时间
        /// </summary>
        double Position { get; set; }
        /// <summary>
        /// 总的持续时间
        /// </summary>
        double Duration { get; }
        /// <summary>
        /// 播放
        /// </summary>
        void Play();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();
        /// <summary>
        /// 停止播放
        /// </summary>
        void Stop();
        /// <summary>
        /// 处于空闲
        /// </summary>
        void Free();
    }
}

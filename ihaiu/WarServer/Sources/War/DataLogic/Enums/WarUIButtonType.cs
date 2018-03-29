namespace Games.PB
{
    public enum ButtonState
    {
        None,
        /// <summary>
        /// 按下
        /// </summary>
        ButtonDown,
        /// <summary>
        /// 抬起
        /// </summary>
        ButtonUp,
        /// <summary>
        /// 朝向操作方向
        /// </summary>
        LookAt,
        /// <summary>
        /// 自动寻怪
        /// </summary>
        AutoSearch,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel,
        /// <summary>
        /// 瞄准中
        /// </summary>
        Aimed,
    }
}
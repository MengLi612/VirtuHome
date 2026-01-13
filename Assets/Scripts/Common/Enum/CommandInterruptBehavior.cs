namespace Common.Enum
{
    /// <summary>
    /// 命令中断时的行为
    /// </summary>
    public enum CommandInterruptBehavior
    {
        /// <summary>
        /// 直接中断并执行新命令（默认）
        /// </summary>
        CancelAndExecuteNew,

        /// <summary>
        /// 忽略新命令，继续执行当前命令
        /// </summary>
        IgnoreNew,

        /// <summary>
        /// 将新命令加入队列，等待当前完成后再执行
        /// </summary>
        QueueNew,

        /// <summary>
        /// 如果当前命令正在执行，则取消当前并执行新命令
        /// </summary>
        CancelCurrentAndExecuteNew,

        /// <summary>
        /// 并行执行，不中断当前命令
        /// </summary>
        ExecuteParallel
    }
}

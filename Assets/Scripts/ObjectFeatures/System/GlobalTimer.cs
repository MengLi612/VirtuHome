using System;
using Common.Concrete.Unit;
using Common.Interface;
using Core;
using UnityEngine;

public abstract class AbstractGlobalTimer : ComponentBehavior
{
    public abstract float GlobalTime { get; set; }
    public abstract int Hours { get; set; }
    public abstract int Minutes { get; set; }
    public abstract int Seconds { get; set; }

    public Action<TimePoint> SecondChanged;
    public Action<TimePoint> MinuteChanged;
    public Action<TimePoint> HourChanged;

    #region 类行为
    public abstract void SetGlobalTime(int hours, int minutes, int seconds);
    public abstract void ResetGlobalTime();
    public abstract void ResumeGlobalTime();
    public abstract void PauseGlobalTime();
    #endregion
}


/// <summary>
/// 全局计时器，用于游戏内的时间管理。
/// </summary>
public class GlobalTimer : AbstractGlobalTimer, ISingletonable<GlobalTimer>
{
    private static GlobalTimer _instance;
    public static GlobalTimer Instance => _instance;

    public static float RealTime => DateTime.Now.Hour * 3600f + DateTime.Now.Minute * 60f + DateTime.Now.Second;
    [field: SerializeField]
    public override float GlobalTime { get; set; } = 0f;

    public override int Hours
    {
        get => Mathf.FloorToInt(GlobalTime / 3600f);
        set
        {
            if (0 <= value && value <= 23 && value != Hours)
            {
                GlobalTime = value * 3600 + Minutes * 60 + Seconds;
                HourChanged?.Invoke(new TimePoint(Hours, Minutes, Seconds));
            }
        }
    }
    public override int Minutes
    {
        get => Mathf.FloorToInt(GlobalTime % 3600f / 60f);
        set
        {
            if (0 <= value && value <= 59 && value != Minutes)
            {
                GlobalTime = Hours * 3600 + value * 60 + Seconds;
                MinuteChanged?.Invoke(new TimePoint(Hours, Minutes, Seconds));
            }
        }
    }
    public override int Seconds
    {
        get => Mathf.FloorToInt(GlobalTime % 60f);
        set
        {
            if (0 <= value && value <= 59 && value != Seconds)
            {
                GlobalTime = Hours * 3600 + Minutes * 60 + value;
                SecondChanged?.Invoke(new TimePoint(Hours, Minutes, Seconds));
            }
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        GlobalTime = RealTime;
    }

    private void Update()
    {
        GlobalTime += Time.deltaTime;
    }

    #region 类行为
    public void UpdateToRealTime()
    {
        GlobalTime = RealTime;
    }
    public override void SetGlobalTime(int hours, int minutes, int seconds)
    {
        GlobalTime = hours * 3600f + minutes * 60f + seconds;
    }
    public override void ResumeGlobalTime()
    {
        Time.timeScale = 1f;
    }
    public override void PauseGlobalTime()
    {
        Time.timeScale = 0f;
    }
    public override void ResetGlobalTime()
    {
        GlobalTime = 0f;
    }
    public string GetFormattedTime()
    {
        return $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";
    }
    #endregion
}

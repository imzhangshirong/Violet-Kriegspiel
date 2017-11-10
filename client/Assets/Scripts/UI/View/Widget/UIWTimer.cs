using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIWTimer : UIWidgetBase
{
    public int remainTime;
    public UILabel label;
    public bool autoRunTimer;
    public string timeFormat;
    /// <summary>
    /// 倒计时结束通知
    /// </summary>
    public string timeUpEventKey;

    bool m_IsTimering = false;
    int m_RemainTime;
    private void OnEnable()
    {
        if (autoRunTimer) StartTimer();
    }


    /// <summary>
    /// 开始倒计时
    /// </summary>
    public void StartTimer()
    {
        if (!m_IsTimering)
        {
            m_RemainTime = remainTime;
            m_IsTimering = true;
            StartCoroutine(DoTimer());
        }
    }
    /// <summary>
    /// 结束倒计时
    /// </summary>
    public void StopTimer()
    {
        m_IsTimering = false;
    }

    public int GetCurrentRemainTime()
    {
        return m_RemainTime;
    }

    IEnumerator DoTimer()
    {
        while (m_RemainTime > 0 && m_IsTimering)
        {
            m_RemainTime--;
            label.text = GetRemainTimeString(timeFormat);
            yield return new WaitForSeconds(1f);
        }
        if (timeUpEventKey != "") Push(timeUpEventKey,null);
        m_IsTimering = false;
    }

    string GetRemainTimeString(string format)
    {
        int[] timeBase = {1, 60, 60, 24 };
        int[] times = { 0, 0, 0 ,0};
        int remainT = m_RemainTime;
        for(int i=1;i< timeBase.Length; i++)
        {
            if (remainT / timeBase[i] > 0)
            {
                times[i-1] = remainT % timeBase[i];
                remainT = remainT / timeBase[i];
                
            }
            else
            {
                times[i-1] = remainT;
                break;
            }
        }
        if (format.IndexOf("[d]") == -1 && format.IndexOf("[dd]") == -1)
        {
            times[2] += times[3] * timeBase[3];
            times[3] = 0;
        }
        if (format.IndexOf("[h]") == -1 && format.IndexOf("[hh]") == -1)
        {
            times[1] += times[2] * timeBase[2];
            times[2] = 0;
        }
        if (format.IndexOf("[m]") == -1 && format.IndexOf("[mm]") == -1)
        {
            times[0] += times[1] * timeBase[1];
            times[1] = 0;
        }
        if (format.IndexOf("[s]") == -1 && format.IndexOf("[ss]") == -1)
        {
            times[0] = 0;
        }
        string re = format;
        re = re.Replace("[d]", times[3].ToString());
        re = re.Replace("[dd]", ((times[3]<10)?"0":"")+times[3].ToString());
        re = re.Replace("[h]", times[2].ToString());
        re = re.Replace("[hh]", ((times[2] < 10) ? "0" : "") + times[2].ToString());
        re = re.Replace("[m]", times[1].ToString());
        re = re.Replace("[mm]", ((times[1] < 10) ? "0" : "") + times[1].ToString());
        re = re.Replace("[s]", times[0].ToString());
        re = re.Replace("[ss]", ((times[0] < 10) ? "0" : "") + times[0].ToString());
        return re;
    }

}
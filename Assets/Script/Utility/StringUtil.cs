using UnityEngine;
using System.Collections;
using System.Text;

public class StringUtil
{
    static StringBuilder mStringBuilder = new StringBuilder();

    public static string FloatToTime(float _time)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);

        int min = (int)(_time / 60);
        int sec = (int)(_time - (min * 60));
        int msec = (int)((_time - (min * 60) - sec) * 1000.0f);

        mStringBuilder.Append(min + ":");
        mStringBuilder.AppendFormat("{0:00}:", sec);
        mStringBuilder.AppendFormat("{0:000}", msec);

        return mStringBuilder.ToString();
    }

    public static string FloatToString(float _value, int _underCount)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);

        mStringBuilder.AppendFormat("{0:F" + _underCount.ToString() + "}", _value);

        return mStringBuilder.ToString();
    }

    public static string IntToString2(float _value)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);

        mStringBuilder.AppendFormat("{0, 2:00}", _value);

        return mStringBuilder.ToString();
    }

    public static string TwoMix(string _wordA, string _wordB)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);
        mStringBuilder.Append(_wordA);
        mStringBuilder.Append(_wordB);

        return mStringBuilder.ToString();
    }

    public static string ThreeMix(string _wordA, string _wordB, string _wordC)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);
        mStringBuilder.Append(_wordA);
        mStringBuilder.Append(_wordB);
        mStringBuilder.Append(_wordC);

        return mStringBuilder.ToString();
    }
}

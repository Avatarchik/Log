using UnityEngine;
using System.Collections;
using System.Text;

public class StringUtil
{
    static StringBuilder mStringBuilder = new StringBuilder();

    ///<summary>소수점 두자릿수까지 표현하는 시간 문자열로 변환 : 변활할 소수점 값</summary>
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

    ///<summary>원하는 소수점 자릿수까지 표현하는 문자열로 변환 : 변활할 소수점 값</summary>
    public static string FloatToString(float _value, int _underCount)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);

        mStringBuilder.AppendFormat("{0:F" + _underCount.ToString() + "}", _value);

        return mStringBuilder.ToString();
    }

    ///<summary>앞 두자리까지 표현하는 문자열로 변환 : 변활할 소수점 값</summary>
    public static string FloatTo2FrontString(float _value)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);

        mStringBuilder.AppendFormat("{0, 2:00}", _value);

        return mStringBuilder.ToString();
    }

    ///<summary>두 문자열을 섞는다.</summary>
    public static string TwoMix(string _wordA, string _wordB)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);
        mStringBuilder.Append(_wordA);
        mStringBuilder.Append(_wordB);

        return mStringBuilder.ToString();
    }

    ///<summary>세 문자열을 섞는다.</summary>
    public static string ThreeMix(string _wordA, string _wordB, string _wordC)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);
        mStringBuilder.Append(_wordA);
        mStringBuilder.Append(_wordB);
        mStringBuilder.Append(_wordC);

        return mStringBuilder.ToString();
    }

    ///<summary>매크로 문자를 다른 문자열로 치환</summary>
    public static string MacroString(string _word, string _change)
    {
        mStringBuilder.Remove(0, mStringBuilder.Length);
        mStringBuilder.Append(_word);
        int startIndex = _word.IndexOf("[]");
        mStringBuilder.Replace("[]", _change, startIndex, 2);

        return mStringBuilder.ToString();
    }
}

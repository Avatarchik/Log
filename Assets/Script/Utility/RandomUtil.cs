using UnityEngine;
using System.Collections;

public class RandomUtil {
    public static bool IsDiceHandred(int _value)
    {
        int randomValue = Random.Range(0, 100) + 1;
        if (randomValue <= _value)
            return true;

        return false;
    }

    public static int DamageRange(int _value1, int _value2)
    {        
        int randomValue = Random.Range(_value1, _value2 + 1);

        return randomValue;
    }
}

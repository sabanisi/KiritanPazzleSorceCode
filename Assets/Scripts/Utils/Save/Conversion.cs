using System;
using System.Collections.Generic;

public class Conversion
{
    public static  int CharToInt(char _chara)
    {
       foreach(var (chara,num) in dictionary)
       {
            if (chara == _chara)
            {
                return num;
            }
       }
        return -1;
    }

    public static char IntToChar(int _num)
    {
        foreach(var (chara, num) in dictionary)
        {
            if (num == _num)
            {
                return chara;
            }
        }
        return 'ん';
    }

    private static List<(char chara, int num)> dictionary = new List<(char chara, int num)>()
    {
        ('あ',0),
        ('い',1),
        ('う',2),
        ('え',3),
        ('お',4),
        ('か',5),
        ('き',6),
        ('く',7),
        ('け',8),
        ('こ',9),
        ('さ',10),
        ('し',11),
        ('す',12),
        ('せ',13),
        ('そ',14),
        ('た',15),
        ('ち',16),
        ('つ',17),
        ('て',18),
        ('と',19),
        ('な',20),
        ('に',21),
        ('ぬ',22),
        ('ね',23),
        ('の',24),
        ('は',25),
        ('ひ',26),
        ('ふ',27),
        ('へ',28),
        ('ほ',29),
        ('ま',30),
        ('み',31),
        ('む',32)
    };
}

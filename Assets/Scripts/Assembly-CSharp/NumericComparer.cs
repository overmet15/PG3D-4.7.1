using System.Collections;
using UnityEngine;

public class NumericComparer : IComparer
{
    private static int baseLngth = "multi_skin_".Length;

    public int Compare(object x, object y)
    {
        Texture textureX = x as Texture;
        Texture textureY = y as Texture;
        if (textureX != null && textureY != null)
        {
            string name = textureX.name.Substring(baseLngth);
            string name2 = textureY.name.Substring(baseLngth);
            int num = int.Parse(name);
            int num2 = int.Parse(name2);
            return num - num2;
        }
        else
        {
            return 0;
        }
    }
}

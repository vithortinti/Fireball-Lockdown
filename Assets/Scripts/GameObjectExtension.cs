using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameObjectExtension
{
    public static Vector2 Get2DCenter(this GameObject go, GameObject gameObject)
    {
        float gox = gameObject.transform.position.x;
        float goy = gameObject.transform.position.y;

        float xcenter = gox + (gox / 2);
        float ycenter = goy + (goy / 2);

        return new Vector2(xcenter, ycenter);
    }
}


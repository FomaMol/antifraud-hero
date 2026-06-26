using System.Collections.Generic;
using UnityEngine;

namespace common.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2Int ToVector2Int(this Vector3Int vector3)
        {
            return new Vector2Int(vector3.x, vector3.y);
        }

        public static Vector3Int ToVector3Int(this Vector2Int vector3)
        {
            return new Vector3Int(vector3.x, vector3.y, 0);
        }

        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, vector2.y, 0);
        }

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }

        public static Vector3 GetCenter(this IEnumerable<Vector3> points)
        {
            var sum = Vector3.zero;
            var count = 0;
            foreach (var point in points)
            {
                sum += point;
                ++count;
            }
            return sum / count;
        }

        public static bool IsOnScreen(this Vector3 worldPosition)
        {
            var viewportPos = Camera.main!.WorldToViewportPoint(worldPosition);
            var isOnScreen = viewportPos is { z: > 0, x: > 0 and < 1, y: > 0 and < 1 };
            return isOnScreen;
        }

        public static bool IsOnLeftSide(this Vector3 worldPosition)
        {
            var viewportPos = Camera.main!.WorldToViewportPoint(worldPosition);
            var isOnLeftSide = viewportPos is { x: <= 0 };
            return isOnLeftSide;
        }

        public static bool IsOnScreen(this Vector2 worldPosition)
        {
            return IsOnScreen(worldPosition.ToVector3());
        }
    }
}
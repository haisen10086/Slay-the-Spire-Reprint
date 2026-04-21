using UnityEngine;

public static class MouseUtil 
{
    private static Camera camera = Camera.main;

    //将鼠标位置映射到一个“总是面向相机、且固定经过世界点 (0,0,zValue)”的平面上，返回该平面上的世界坐标。
    public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
    {
        Plane dragPlane = new(camera.transform.forward, new Vector3(0, 0, zValue));
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}

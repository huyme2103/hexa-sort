using UnityEngine;
using System;
using System.Collections.Generic;
public class Hexagon : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] private new Collider collider;
 
    public HexStack HexStack {  get; private set; }
    public Color Color
    {
        get { return renderer.material.color; }
        set { renderer.material.color = value; }
    }

    public void Configure(HexStack hexStack)
    {
        this.HexStack = hexStack;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    public void DisableCollider() => collider.enabled = false;

//public void MoveToLocal(Vector3 targetLocalPos)
//{
//        LeanTween.cancel(gameObject);
//        float delay = transform.GetSiblingIndex() * 0.01f;

//        LeanTween.moveLocal(gameObject, targetLocalPos, .2f)
//            .setEase(LeanTweenType.easeInOutSine)
//            .setDelay(delay);

//        Vector3 direction = (targetLocalPos - transform.localPosition).With(y: 0).normalized;
//        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

//        LeanTween.rotateAround(gameObject, rotationAxis, 180, .2f)
//            .setEase(LeanTweenType.easeInOutSine)
//            .setDelay(delay);

//        LeanTween.value(gameObject, 0, 1, 0.3f)
//            .setOnUpdate((float val) => {
//                renderer.material.SetColor("_EmissionColor", Color.yellow * val);
//            })
//            .setLoopPingPong(1)
//            .setDelay(delay);
//    }


    public void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, 0.2f)
            .setEase(LeanTweenType.easeInBack)
            .setDelay(delay)
            .setOnComplete(() => Destroy(gameObject));
    }

    public void MoveToLocal(Vector3 targetLocalPos, float duration = 0.2f, float delay = 0f)
    {
        // Hủy tween cũ (nếu có) để không bị chồng chéo
        LeanTween.cancel(gameObject);

        // Di chuyển
        LeanTween.moveLocal(gameObject, targetLocalPos, duration)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);

        // Xoay theo hướng di chuyển (chỉ trong mặt phẳng XZ)
        Vector3 direction = targetLocalPos - transform.localPosition;
        direction.y = 0;
        direction.Normalize();
        if (direction.sqrMagnitude > 0.0001f)
        {
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
            LeanTween.rotateAround(gameObject, rotationAxis, 180f, duration)
                .setEase(LeanTweenType.easeInOutSine)
                .setDelay(delay);
        }

     
        // Hiệu ứng phát sáng (Emission flash)
        renderer.material.EnableKeyword("_EMISSION"); // Bắt buộc bật emission
        LeanTween.value(gameObject, 0f, 1f, duration + 0.1f)
            .setOnUpdate((float val) =>
            {
                renderer.material.SetColor("_EmissionColor", Color.white * val);
            })
            .setLoopPingPong(1) // chạy lên rồi xuống một lần
            .setDelay(delay);

    }

}

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
public void MoveToLocal(Vector3 targetLocalPos)
{
    // Bắt đầu bằng việc "lật" Hexagon
    LeanTween.rotateY(gameObject, 90f, 0.1f) // Xoay 90 độ để giống như lật đi
        .setEase(LeanTweenType.easeInBack)
        .setOnComplete(() =>
        {
            // Sau khi lật nửa vòng, đưa về vị trí mới
            LeanTween.moveLocal(gameObject, targetLocalPos, 0.2f)
                .setEase(LeanTweenType.easeInOutSine);

            // Lật ngược trở lại (từ 90 về 0)
            LeanTween.rotateY(gameObject, 0f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setDelay(0.2f); // delay để nó khớp với lúc di chuyển xong
        })
        .setDelay(transform.GetSiblingIndex() * 0.1f);
}



    public void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, 0.3f)
            .setEase(LeanTweenType.easeInBack)
            .setDelay(delay)
            .setOnComplete(() => Destroy(gameObject));
    }
}

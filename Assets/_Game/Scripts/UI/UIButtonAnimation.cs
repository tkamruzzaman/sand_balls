using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform rect;

    private readonly float duration = 0.2f;
    private readonly float percentage = 0.9f;
    private Vector3 initialScale;

    void Reset()
    {
        rect = gameObject.GetComponent<RectTransform>();
        GetComponent<Button>().transition = Selectable.Transition.None;
    }

    void Awake()
    {
        initialScale = rect.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rect != null)
        {
            rect.DOScale(initialScale * percentage, duration).SetEase(Ease.OutQuad).SetUpdate(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (rect != null)
        {
            rect.DOScale(initialScale, duration).SetEase(Ease.OutQuad).SetUpdate(true);
        }
    }
}

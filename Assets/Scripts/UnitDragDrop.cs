using PathCreation;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler, IDropHandler
{
    [SerializeField] public Canvas canvas;
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public Vector3 itemPosition;
    public PathCreator pathCreator;
    public UnitEnum unitTypeId;
    public float unitRange;
    public GameObject circle;
    public RaycastHit hitData;

    void Start()
    {
        itemPosition = rectTransform.anchoredPosition;
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        circle.transform.localScale *= unitRange*2;


    }
    public void OnDrag(PointerEventData eventData)
    {
        circle.transform.position = this.transform.position;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitData, 1000))
        {
            circle.transform.position = new Vector3(hitData.point.x, hitData.point.y +0.1f, hitData.point.z);
            if (hitData.collider.gameObject.tag == "Path" || hitData.collider.gameObject.tag == "Enemy" || hitData.collider.gameObject.tag == "Unit")
            {
                circle.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, .5f);
            }
            else
            {
                circle.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, .5f);
                
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitData,1000))
        {
            if (hitData.collider.gameObject.tag != "Path" && hitData.collider.gameObject.tag != "Enemy" && hitData.collider.gameObject.tag != "Unit")
            {
                this.transform.position = hitData.point;
                GameManager.Instance.CreateUnit(unitTypeId, this.transform.position);
            }

            rectTransform.anchoredPosition = itemPosition;
            
            circle.transform.localScale /= unitRange*2;

        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        
    }
    public void OnDrop(PointerEventData eventData)
    {

    }
}

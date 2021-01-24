using UnityEngine;
using UnityEngine.UI;

public class UnitCanvas : MonoBehaviour
{
    public RaycastHit rayHit;
    public Button upgradeButton;
    public GameObject Content;
    public RectTransform CanvasRect;
    void Start()
    {
        CanvasRect = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out rayHit, 500.0f))
            {

                if (rayHit.collider.gameObject.tag == "Unit")
                {
                    GameManager.Instance.OnUnitClick(rayHit.collider.gameObject.GetComponent<UnitManager>());
                }


            }
            else
            {
                GameManager.Instance.OnUnitClick(null);
            }
        }
    }
    public void OpenCanvas()
    {
        if (GameManager.Instance.activeUnitManager == null)
        {
            CloseCanvas();
            return;
        }
        UpdateCanvas();
        Content.SetActive(true);
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(GameManager.Instance.activeUnitManager.gameObject.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        Content.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
    }

    public void CloseCanvas()
    {
        Content.SetActive(false);
    }

    public void UpdateCanvas()
    {
        if (GameManager.Instance.activeUnitManager == null)
        {
            return;
        }

        upgradeButton.interactable = GameManager.Instance.activeUnitManager.isUpgradable();
    }
    public void UpgradeUnit()
    {
        if (GameManager.Instance.activeUnitManager == null || GameManager.Instance.activeUnitManager.isUpgradable() == false)
        {
            return;
        }
        GameManager.Instance.activeUnitManager.UnitUp();
    }
    public void UnitSelling()
    {
        GameManager.Instance.activeUnitManager.UnitSelling();
        Content.SetActive(false);
    }

    //public void StartWave()
    //{
    //    GameManager.Instance.CreateWave();
        
    //}
}

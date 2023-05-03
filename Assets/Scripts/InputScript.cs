using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    [SerializeField] private Image selectionRect;
    [SerializeField] private RectTransform guiCanvasRect;
    [SerializeField] private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();
    [Range(0f, 30f)]
    [SerializeField] private float formationRadius = 15f;

    private Vector2 selectionStartMousePos;
    private Vector2 selectionEndMousePos;

    [SerializeField] private UnitGroup unitGroup;

    private void Start()
    {
        selectionStartMousePos = Vector2.zero;
        selectionEndMousePos = Vector2.zero;

        unitGroup = new UnitGroup();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            unitGroup.SetGroup(selectedUnits);

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D[] objectHit = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero, 20f);
            List<RaycastHit2D> listOfHits = new List<RaycastHit2D>(objectHit);

            AIUnit enemy = null;
            listOfHits.Find((hit) => hit.collider.TryGetComponent(out enemy));

            if (enemy != null)
            {
                unitGroup.Attack(enemy);
                return;
            }

            unitGroup.MoveGroup(mouseWorldPos, formationRadius);
        }
        if (Input.GetMouseButtonDown(0))
        {
            selectionRect.gameObject.SetActive(true);
            selectionStartMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            selectionEndMousePos = Input.mousePosition;
            UpdateSelection();
        }
        if (Input.GetMouseButtonUp(0))
        {
            selectionRect.gameObject.SetActive(false);
        }
    }

    private void OnGUI()
    {
        if (Input.GetMouseButton(0))
        {
            DrawSelectionRectangle();
        }
    }

    private void UpdateSelection()
    {
        UnselectUnits();

        Vector2 selectionStartWorldMousePos = Camera.main.ScreenToWorldPoint(selectionStartMousePos);
        Vector2 selectionEndWorldMousePos = Camera.main.ScreenToWorldPoint(selectionEndMousePos);

        Collider2D[] colliders = Physics2D.OverlapAreaAll(selectionStartWorldMousePos, selectionEndWorldMousePos);


        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out SelectableUnit unit))
            {
                selectedUnits.Add(unit);
                unit.Select();
                unit.DeathEventSubscribe(RemoveFromSelect);
                if (selectionStartWorldMousePos == selectionEndWorldMousePos)
                {
                    break;
                }
            }
        }
    }

    private void UnselectUnits()
    {
        foreach (SelectableUnit unit in selectedUnits)
        {
            if (unit == null) continue;
            unit.Deselect();
            unit.DeathEventUnsubscribe(RemoveFromSelect);
        }
        selectedUnits.Clear();
    }

    private void DrawSelectionRectangle()
    {
        Vector2 screenStartMousePos = selectionStartMousePos;
        Vector2 screenEndMousePos = selectionEndMousePos;

        screenStartMousePos.y = Mathf.Abs(Screen.height - screenStartMousePos.y);
        screenEndMousePos.y = Mathf.Abs(Screen.height - screenEndMousePos.y);

        float inset = Mathf.Min(screenStartMousePos.x, screenEndMousePos.x);
        float size = Mathf.Abs(screenEndMousePos.x - screenStartMousePos.x);
        selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, size);

        inset = Mathf.Min(screenStartMousePos.y, screenEndMousePos.y);
        size = Mathf.Abs(screenEndMousePos.y - screenStartMousePos.y);
        selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, size);
    }

    private void RemoveFromSelect(Unit unit)
    {
        SelectableUnit selectable = selectedUnits.Find(s => ReferenceEquals(unit, s.Unit));
        selectedUnits.Remove(selectable);
    }
}

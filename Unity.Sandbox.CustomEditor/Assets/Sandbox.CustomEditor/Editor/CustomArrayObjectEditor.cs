using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(CustomArrayObject))]
public class CustomArrayObjectEditor : Editor
{
    CustomArrayObject customArrayObject;

    public override VisualElement CreateInspectorGUI()
    {
        customArrayObject = (CustomArrayObject)target;

        var inspector = new VisualElement();

        var customInspector = new VisualElement();
        customInspector.Add(MakeListView(customArrayObject));

        inspector.Add(MakeSampleSectionHeader("Custom Inspector:"));
        inspector.Add(customInspector);

        var defaultInspector = new VisualElement();
        InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);

        inspector.Add(MakeSampleSectionHeader("Default Inspector:"));
        inspector.Add(defaultInspector);

        return inspector;
    }

    private VisualElement MakeListView(CustomArrayObject customArrayObject)
    {
        var listView = new ListView(
            customArrayObject.namedInts,
            -1,
            MakeNamedIntInspector,
            BindNamedIntInspector
        );

        listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        listView.showFoldoutHeader = false;
        listView.showAddRemoveFooter = true;
        listView.reorderMode = ListViewReorderMode.Animated;
        listView.pickingMode = PickingMode.Position;
        listView.reorderable = true;

        listView.onAdd = HandleAddItem;
        listView.onRemove = HandleRemoveItem;

        return listView;
    }

    private void HandleAddItem(BaseListView view)
    {
        Array.Resize(ref customArrayObject.namedInts, customArrayObject.namedInts.Length + 1);
        customArrayObject.namedInts[customArrayObject.namedInts.Length - 1] = CreateNewNamedInt();

        view.itemsSource = customArrayObject.namedInts;
        view.RefreshItems();
        view.ScrollToItem(view.itemsSource.Count);
    }

    private void HandleRemoveItem(BaseListView view)
    {
        Array.Resize(ref customArrayObject.namedInts, customArrayObject.namedInts.Length - 1);
        view.itemsSource = customArrayObject.namedInts;
        view.RefreshItems();
        view.ScrollToItem(view.itemsSource.Count);
    }

    private void HandleItemsRemoved(IEnumerable<int> indices)
    {
        var indicesToKeep = Enumerable.Range(0, customArrayObject.namedInts.Length).Except(indices);

        customArrayObject.namedInts = indicesToKeep
            .Select(x => customArrayObject.namedInts[x])
            .ToArray();
    }

    private void HandleItemsAdded(IEnumerable<int> indices)
    {
        foreach (var _ in indices)
        {
            customArrayObject.namedInts.Append(CreateNewNamedInt());
        }
    }

    private VisualElement MakeNamedIntInspector()
    {
        return new VisualElement();
    }

    private void BindNamedIntInspector(VisualElement element, int index)
    {
        if (customArrayObject.namedInts.Length <= index)
        {
            customArrayObject.namedInts.Append(CreateNewNamedInt());
        }

        var namedInt = customArrayObject.namedInts[index];

        element.Clear();

        element.Add(new TextField("name") { dataSource = namedInt, bindingPath = name });

        element.Add(new IntegerField("value") { value = namedInt.value });
    }

    private VisualElement MakeSampleSectionHeader(string text)
    {
        var label = new Label(text);
        label.style.fontSize = 20;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;

        return label;
    }

    private NamedInt CreateNewNamedInt()
    {
        return new NamedInt()
        {
            name = $"New Named Int {customArrayObject.namedInts.Length}",
            value = 0,
        };
    }
}

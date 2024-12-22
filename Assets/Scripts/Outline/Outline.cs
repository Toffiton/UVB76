// Outline.cs
// QuickOutline
//
// Created by Chris Nolet on 3/30/18.
// Copyright © 2018 Chris Nolet. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Outline : MonoBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    // Режимы обводки
    public enum Mode
    {
        OutlineAll, // Показывать обводку всегда
        OutlineVisible, // Показывать обводку только для видимых объектов
        OutlineHidden, // Показывать обводку только для скрытых объектов
        OutlineAndSilhouette, // Обводка и силуэт
        SilhouetteOnly // Только силуэт
    }

    public Mode OutlineMode
    {
        get { return outlineMode; }
        set
        {
            outlineMode = value;
            needsUpdate = true;
        }
    }

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            outlineColor = value;
            needsUpdate = true;
        }
    }

    public float OutlineWidth
    {
        get { return outlineWidth; }
        set
        {
            outlineWidth = value;
            needsUpdate = true;
        }
    }

    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }

    [SerializeField]
    private Mode outlineMode;

    [SerializeField]
    private Color outlineColor = Color.white;

    [SerializeField, Range(0f, 10f)]
    private float outlineWidth = 5f;

    [Header("Optional")]
    [SerializeField, Tooltip("Предварительный расчет: вычисления для вершин выполняются в редакторе и сохраняются с объектом. "
        + "Если отключено: вычисления выполняются в Awake() во время выполнения, что может вызвать задержку для больших сеток.")]
    private bool precomputeOutline;

    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();

    private Renderer[] renderers;
    private Material outlineMaskMaterial;
    private Material outlineFillMaterial;
    private bool isOutlineVisible = false; // Флаг для контроля видимости обводки

    private bool needsUpdate;

    private bool firstLoad = true;

    void Awake()
    {
        // Кэширование рендеров
        renderers = GetComponentsInChildren<Renderer>();

        // Создание материалов для обводки
        outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
        outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

        outlineMaskMaterial.name = "OutlineMask (Instance)";
        outlineFillMaterial.name = "OutlineFill (Instance)";

        // Загрузка или генерация сглаженных нормалей
        LoadSmoothNormals();

        // Немедленное применение свойств материалов
        needsUpdate = true;
    }

    void OnEnable()
    {
        foreach (var renderer in renderers)
        {
            // Добавление шейдеров для обводки
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    void OnValidate()
    {
        // Обновление свойств материалов
        needsUpdate = true;

        // Очистка кэша при отключении предварительного расчета или при повреждении данных
        if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count)
        {
            bakeKeys.Clear();
            bakeValues.Clear();
        }

        // Генерация сглаженных нормалей при включении предварительного расчета
        if (precomputeOutline && bakeKeys.Count == 0)
        {
            Bake();
        }
    }

    void LateUpdate()
    {
        Debug.Log("LateUpdate");
        if (!isOutlineVisible)
        {
            HideOutline();
        }
    }

    void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;
            UpdateMaterialProperties();
        }
    }

    void OnDisable()
    {
        foreach (var renderer in renderers)
        {
            // Удаление шейдеров для обводки
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(outlineMaskMaterial);
            materials.Remove(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    void OnDestroy()
    {
        // Уничтожение экземпляров материалов
        Destroy(outlineMaskMaterial);
        Destroy(outlineFillMaterial);
    }

    void Bake()
    {
        // Генерация сглаженных нормалей для каждой сетки
        var bakedMeshes = new HashSet<Mesh>();

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            // Пропуск дубликатов
            if (!bakedMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Сохранение сглаженных нормалей
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            bakeKeys.Add(meshFilter.sharedMesh);
            bakeValues.Add(new ListVector3() { data = smoothNormals });
        }
    }

    void LoadSmoothNormals()
    {
        // Загрузка или генерация сглаженных нормалей
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            // Пропуск, если нормали уже применены
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Загрузка или генерация сглаженных нормалей
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Сохранение сглаженных нормалей в UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Объединение подсеток
            var renderer = meshFilter.GetComponent<Renderer>();

            if (renderer != null)
            {
                CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
            }
        }

        // Очистка UV3 для скинированных мешей
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            // Пропуск, если UV3 уже очищены
            if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                continue;
            }

            // Очистка UV3
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

            // Объединение подсеток
            CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
        }
    }

    List<Vector3> SmoothNormals(Mesh mesh)
    {
        // Группировка вершин по местоположению
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Копирование нормалей в новый список
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Усреднение нормалей для групп вершин
        foreach (var group in groups)
        {
            // Пропуск одиночных вершин
            if (group.Count() == 1)
            {
                continue;
            }

            // Вычисление средней нормали
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }

            smoothNormal.Normalize();

            // Назначение усредненной нормали каждой вершине
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    void CombineSubmeshes(Mesh mesh, Material[] materials)
    {
        // Пропуск мешей с одной подсеткой
        if (mesh.subMeshCount == 1)
        {
            return;
        }

        // Пропуск, если количество подсеток превышает количество материалов
        if (mesh.subMeshCount > materials.Length)
        {
            return;
        }

        // Добавление объединенной подсетки
        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

    void UpdateMaterialProperties()
    {
        // Применение свойств в соответствии с режимом
        outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

        switch (outlineMode)
        {
            case Mode.OutlineAll:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", firstLoad ? 0 : outlineWidth);
                break;

            case Mode.OutlineVisible:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_OutlineWidth", firstLoad ? 0 : outlineWidth);
                break;

            case Mode.OutlineHidden:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", firstLoad ? 0 : outlineWidth);
                break;

            case Mode.OutlineAndSilhouette:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", firstLoad ? 0 : outlineWidth);
                break;

            case Mode.SilhouetteOnly:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                break;
        }

        firstLoad = false;
    }

    public void ShowOutline()
    {
        Debug.Log("ShowOutline");
        SetOutlineVisibility(true);
    }

    public void HideOutline()
    {
        Debug.Log("HideOutline");
        SetOutlineVisibility(false);
    }

    public void SetOutlineVisibility(bool visible)
    {
        if (isOutlineVisible == visible) return; // Избегаем лишних вызовов
        isOutlineVisible = visible;

        float outlineWidth = visible ? this.outlineWidth : 0f;

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.HasProperty("_OutlineWidth"))
                {
                    material.SetFloat("_OutlineWidth", outlineWidth);
                }
            }
        }
    }
}
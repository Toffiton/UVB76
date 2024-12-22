using UnityEditor;
using UnityEngine;

public class ConvertMaterialsToURP : MonoBehaviour
{
    [MenuItem("Tools/Convert Materials to URP Lit")]
    public static void ConvertMaterials()
    {
        // Найти все материалы в проекте
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        int count = 0;

        foreach (string guid in materialGuids)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            if (material != null && material.shader.name == "Standard")
            {
                // Сохраним старые текстуры
                Texture albedoTexture = material.GetTexture("_MainTex");
                Color albedoColor = material.GetColor("_Color");
                Texture normalMap = material.GetTexture("_BumpMap");
                float metallic = material.GetFloat("_Metallic");
                float smoothness = material.GetFloat("_Glossiness");

                // Заменить шейдер на URP Lit
                material.shader = Shader.Find("Universal Render Pipeline/Lit");

                // Перенести текстуры и параметры
                material.SetTexture("_BaseMap", albedoTexture);
                material.SetColor("_BaseColor", albedoColor);
                material.SetTexture("_BumpMap", normalMap);
                material.SetFloat("_Metallic", metallic);
                material.SetFloat("_Smoothness", smoothness);

                count++;
                Debug.Log($"Материал обновлён: {materialPath}");
            }
        }

        Debug.Log($"Готово! Обновлено материалов: {count}");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

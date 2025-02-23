using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHandMovement : MonoBehaviour
{
    private SkinnedMeshRenderer handRenderer;
    private Transform[] bones;
    private List<Transform> fingerBones = new List<Transform>();

    private void Start()
    {
        // Ищем SkinnedMeshRenderer в объекте
        handRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (handRenderer == null)
        {
            Debug.LogError($"[{gameObject.name}] SkinnedMeshRenderer не найден!");
            return;
        }

        bones = handRenderer.bones;

        // Фильтруем кости пальцев (проверяем по названию)
        foreach (Transform bone in bones)
        {
            if (bone.name.ToLower().Contains("finger") || bone.name.ToLower().Contains("hand"))
            {
                fingerBones.Add(bone);
            }
        }

        if (fingerBones.Count == 0)
        {
            Debug.LogWarning($"[{gameObject.name}] Кости пальцев не найдены! Проверь названия костей.");
        }

        // Запускаем анимацию пальцев
        StartCoroutine(AnimateFingers());
    }

    private IEnumerator AnimateFingers()
    {
        while (true)
        {
            foreach (var finger in fingerBones)
            {
                float randomBend = Random.Range(-30f, 30f); // Случайный угол сгиба
                finger.localRotation = Quaternion.Euler(randomBend, 0, 0);
            }

            float delay = Random.Range(0.5f, 1.5f); // Рандомный интервал между движениями
            yield return new WaitForSeconds(delay);
        }
    }
}
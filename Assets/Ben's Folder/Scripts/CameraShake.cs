using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    bool shaking = false;

    public IEnumerator Shake(float duration, float size) 
    {
        Vector3 startPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration) 
        {
            float x = Random.Range(-1.0f, 1.0f) * size;
            float y = Random.Range(-1.0f, 1.0f) * size;

            transform.localPosition = new Vector3(x, y, startPos.z);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        transform.localPosition = startPos;

    }
}

using UnityEngine;

public class ColorEffectCenter : MonoBehaviour
{
    [SerializeField] private Material mat;
    private void Awake()
    {

    }


    private void Update()
    {
        mat.SetVector("Impact_Center", transform.position);

    }

}

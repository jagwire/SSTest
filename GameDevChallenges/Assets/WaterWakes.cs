using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WaterWakes : MonoBehaviour
{

    Mesh waterMesh;
    MeshFilter waterMeshFilter;
    float waterWidth = 3f;
    float gridSpacing = 0.1f;

    //part 2
    public float alpha = 0.9f;
    int P = 8; //kernel size
    float g = -9.81f; //gravity
    float[,] storedKernelArray;




    void Start()
    {
        waterMeshFilter = this.GetComponent<MeshFilter>();
        List<Vector3[]> height_tmp = GenerateWaterMesh.GenerateWater(waterMeshFilter, waterWidth, gridSpacing);

        waterMesh = waterMeshFilter.mesh;

        BoxCollider boxCollider = this.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(waterWidth / 2.0f, 0.0f, waterWidth / 2.0f);
        boxCollider.size = new Vector3(waterWidth, 0.1f, waterWidth);

        transform.position = new Vector3(-waterWidth / 2.0f, 0f, -waterWidth / 2.0f);

        //part 2
        storedKernelArray = new float[P * 2 + 1, P * 2 + 1];
        PrecomputeKernelValues();
    }

	// Update is called once per frame
	void Update () {
	
	}


    void PrecomputeKernelValues()
    {

        float G_zero = CalculateG_zero();

        for (int k = -P; k <= P; k++)
        {
            for (int l = -P; l <= P; l++)
            {
                storedKernelArray[k + P, l + P] = CalculateG((float)k, (float)l, G_zero);
            }
        }
    }

    private float CalculateG(float k, float l, float G_zero)
    {
        float delta_q = 0.001f;
        float sigma = 1f;
        float r = Mathf.Sqrt(k * k + l * l);

        float G = 0f;
        for (int n = 1; n <= 10000; n++)
        {
            float q_n = ((float)n * delta_q);
            float q_n_square = q_n * q_n;

            G += q_n_square * Mathf.Exp(-sigma * q_n_square) * BesselFunction(q_n * r);

        }

        G /= G_zero;
        return G;

    }

    private float CalculateG_zero()
    {
        float delta_q = 0.001f;
        float sigma = 1f;
        float G_zero = 0f;

        for (int n = 1; n <= 10000; n++)
        {
            float q_n_square = ((float)n * delta_q) * ((float)n * delta_q);
            G_zero += q_n_square * Mathf.Exp(-sigma * q_n_square);
        }

        return G_zero;
    }

    private float BesselFunction(float x)
    {
        float J_zero_of_X = 0f;

        if (x <= 3.0f) { Debug.Log("Smaller"); }

        if (x <= 3f)
        {
            //Ignored the small rest term at the end
            J_zero_of_X =
                1f -
                    2.2499997f * Mathf.Pow(x / 3f, 2f) +
                    1.2656208f * Mathf.Pow(x / 3f, 4f) -
                    0.3163866f * Mathf.Pow(x / 3f, 6f) +
                    0.0444479f * Mathf.Pow(x / 3f, 8f) -
                    0.0039444f * Mathf.Pow(x / 3f, 10f) +
                    0.0002100f * Mathf.Pow(x / 3f, 12f);
        }
        //9.4.3
        //3 <= x <= infinity
        else
        {
            //Ignored the small rest term at the end
            float f_zero =
                0.79788456f -
                    0.00000077f * Mathf.Pow(3f / x, 1f) -
                    0.00552740f * Mathf.Pow(3f / x, 2f) -
                    0.00009512f * Mathf.Pow(3f / x, 3f) -
                    0.00137237f * Mathf.Pow(3f / x, 4f) -
                    0.00072805f * Mathf.Pow(3f / x, 5f) +
                    0.00014476f * Mathf.Pow(3f / x, 6f);

            //Ignored the small rest term at the end
            float theta_zero =
                x -
                    0.78539816f -
                    0.04166397f * Mathf.Pow(3f / x, 1f) -
                    0.00003954f * Mathf.Pow(3f / x, 2f) -
                    0.00262573f * Mathf.Pow(3f / x, 3f) -
                    0.00054125f * Mathf.Pow(3f / x, 4f) -
                    0.00029333f * Mathf.Pow(3f / x, 5f) +
                    0.00013558f * Mathf.Pow(3f / x, 6f);

            //Should be cos and not acos
            J_zero_of_X = Mathf.Pow(x, -1f / 3f) * f_zero * Mathf.Cos(theta_zero);
        }

        return J_zero_of_X;
    }
}
